using System;
using GreenHouseWebApi.Model;
using GreenHouseWebApi.Repository; 
using GreenHouse.BusinessService.Data;
using GreenHouse.BusinessService.Device;
using GreenHouse.DataModel.Log;
using Hangfire;

namespace GreenHouseWebApi.BusinessService.Operations
{
    public class DevicePollingService : IDevicePollingService
    {
        private readonly IDeviceSetupRepository _deviceSetupRepository;
        private readonly IGreenHouseSparkDeviceService _greenHouseSparkDeviceService;
        private readonly ILogDataService<AirHumidityLogEntry> _airHumidityLogDataService;
        private readonly ILogDataService<SoilHumidityLogEntry> _soilHumidityLogDataServiceService;
        
        private string _recurringDevicePollingTaskName = "ReccurringDevicePoll";

        public DevicePollingService(IDeviceSetupRepository deviceSetupRepository,
            IGreenHouseSparkDeviceService greenHouseSparkDeviceService,
            ILogDataService<AirHumidityLogEntry> airHumidityLogDataService,
            ILogDataService<SoilHumidityLogEntry> soilHumidityLogDataServiceService)
        {
            _deviceSetupRepository = deviceSetupRepository;
            _greenHouseSparkDeviceService = greenHouseSparkDeviceService; 
            _airHumidityLogDataService = airHumidityLogDataService;
            _soilHumidityLogDataServiceService = soilHumidityLogDataServiceService;
        }

        public void PollDevice(int deviceId)
        {
            //Execute this in Background, because it will take a while and the core might be blocked
            BackgroundJob.Enqueue(() => ExecutePollAction(deviceId));

            //if polling is not done by the device, to it using this website //todo: check if this is initiated, seems to be improper to be done in this method
            DeviceSetup deviceSetup = _deviceSetupRepository.GetSingle(deviceId);

            if (deviceSetup != null && !deviceSetup.PollingEnabled)
            {
                var minutes = deviceSetup.PostBackIntervalMinutes;
                RecurringJob.AddOrUpdate(_recurringDevicePollingTaskName, () => ExecutePollAction(deviceId),
                    "*/" + minutes + " * * * *");
            }
        }

        public void ExecutePollAction(int deviceId)
        {
            //Check Device state
            SparkDeviceOperationState currentState;
            DeviceConfiguration deviceConfig = _deviceSetupRepository.GetSingle(deviceId);
            if(deviceConfig == null)
            {
                throw new ArgumentException($"Device configuration not found for ID: {deviceId}");
            }
            if (TryCurrentStateFromDevice(deviceConfig, out currentState))
            {
                if (currentState == SparkDeviceOperationState.BootedNoConfiguration)
                {
                    SaveConfigurationOnSparkDevice(deviceConfig);
                }   

                else
                {
                    RetrieveValuesFromSparkDevice(deviceConfig);
                }
            }
        }

        #region Private helper

        private void SaveConfigurationOnSparkDevice(DeviceConfiguration deviceConfig)
        {
            _greenHouseSparkDeviceService.SaveDeviceConfiguration(deviceConfig);
        }

        private void RetrieveValuesFromSparkDevice(DeviceConfiguration deviceConfig)
        {
            AirHumidityLogEntry airAirHumidityLogEntry;
            if (TryRetrieveAirHumidityLogEntryFromDevice(deviceConfig.SparkCoreId, out airAirHumidityLogEntry))
            {
                _airHumidityLogDataService.Save(airAirHumidityLogEntry);
            }

            SoilHumidityLogEntry soilHumidityLogEntryWateringAreaOne;

            if (TryRetrieveSoilHumidityLogEntryFromDevice(deviceConfig.SparkCoreId, out soilHumidityLogEntryWateringAreaOne,
                WateringLocation.First))
            {
                _soilHumidityLogDataServiceService.Save(soilHumidityLogEntryWateringAreaOne);
            }

            SoilHumidityLogEntry soilHumidityLogEntryWateringAreaTwo;

            if (TryRetrieveSoilHumidityLogEntryFromDevice(deviceConfig.SparkCoreId, out soilHumidityLogEntryWateringAreaTwo,
                WateringLocation.Second))
            {
                _soilHumidityLogDataServiceService.Save(soilHumidityLogEntryWateringAreaTwo);
            }
        }

        private bool TryCurrentStateFromDevice(DeviceConfiguration deviceConfig, out SparkDeviceOperationState currentState)
        {
            bool success;

            currentState = SparkDeviceOperationState.BootedNoConfiguration;

            try
            {
                currentState = _greenHouseSparkDeviceService.RequestCurrentState(deviceConfig.SparkCoreId);
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                //Todo: log this error
            }

            return success;
        }

        private bool TryRetrieveSoilHumidityLogEntryFromDevice(string sparkCoreId, out SoilHumidityLogEntry soilHumidityLogEntry, WateringLocation wateringLocation)
        {
            bool success;

            double soilHumidityWateringArea = 0;

            try
            {
                soilHumidityWateringArea =
                    _greenHouseSparkDeviceService.RequestCurrentSoilMoisturePercentage(sparkCoreId, wateringLocation);
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                //Todo: log this error
            }

            if (success)
            {
                soilHumidityLogEntry = new SoilHumidityLogEntry
                {
                    LogDateUtc = DateTime.UtcNow,
                    WateringLocation = wateringLocation,
                    PercentageOfHumidity = soilHumidityWateringArea
                };
            }
            else
            {
                soilHumidityLogEntry = null;
            }

            return success;
        }

        private bool TryRetrieveAirHumidityLogEntryFromDevice(string sparkCoreId, out AirHumidityLogEntry airAirHumidityLogEntry)
        {

            bool success;

            double airTemperature = 0;
            double airHumidity = 0;
            double airDewPoint = 0;

            try
            {
                airTemperature = _greenHouseSparkDeviceService.RequestCurrentTemperature(sparkCoreId);
                airHumidity = _greenHouseSparkDeviceService.RequestCurrentAirHumidityPercentage(sparkCoreId);
                airDewPoint = _greenHouseSparkDeviceService.RequestCurrentAirDewpoint(sparkCoreId);

                success = true;
            }
            catch (Exception e)
            {
                success = false;
                //Todo: log this error
            }

            if (success)
            {
                airAirHumidityLogEntry = new AirHumidityLogEntry
                {
                    LogDateUtc = DateTime.UtcNow,
                    TemperatureInCelsius = airTemperature,
                    RelativeHumidity = airHumidity,
                    DewPointTemperatureInCelsius = airDewPoint
                };
            }
            else
            {
                airAirHumidityLogEntry = null;
            }

            return success;

        }

        #endregion
    }
}