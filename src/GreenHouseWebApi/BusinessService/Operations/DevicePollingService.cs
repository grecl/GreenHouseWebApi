using System;
using System.Threading.Tasks;
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

        public async void ExecutePollAction(int deviceId)
        {
            //Check Device state
            SparkDeviceOperationState? currentState;
            DeviceConfiguration deviceConfig = _deviceSetupRepository.GetSingle(deviceId);
            if(deviceConfig == null)
            {
                throw new ArgumentException($"Device configuration not found for ID: {deviceId}");
            }
            currentState = await ReadCurrentStateFromDevice(deviceConfig);

            if (currentState.HasValue)
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

        private async void RetrieveValuesFromSparkDevice(DeviceConfiguration deviceConfig)
        {
            AirHumidityLogEntry airAirHumidityLogEntry = await RetrieveAirHumidityLogEntryFromDevice(deviceConfig);
            if (airAirHumidityLogEntry != null)
            {
                _airHumidityLogDataService.Save(airAirHumidityLogEntry);
            }

            SoilHumidityLogEntry soilHumidityLogEntryWateringAreaOne =await RetrieveSoilHumidityLogEntryFromDevice(deviceConfig, WateringLocation.First);

            if (soilHumidityLogEntryWateringAreaOne != null)
            {
                _soilHumidityLogDataServiceService.Save(soilHumidityLogEntryWateringAreaOne);
            }

            SoilHumidityLogEntry soilHumidityLogEntryWateringAreaTwo = await RetrieveSoilHumidityLogEntryFromDevice(deviceConfig, WateringLocation.Second);
            if (soilHumidityLogEntryWateringAreaTwo != null)
            {
                _soilHumidityLogDataServiceService.Save(soilHumidityLogEntryWateringAreaTwo);
            }
        }

        private async Task<SparkDeviceOperationState?> ReadCurrentStateFromDevice(DeviceConfiguration deviceConfig)
        {
            bool success;

            var currentState = SparkDeviceOperationState.BootedNoConfiguration;

            try
            {
                currentState = await _greenHouseSparkDeviceService.RequestCurrentState(deviceConfig);
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                //Todo: log this error
            }

            if (success)
            {
                return currentState;
            }
            return null;
        }

        private async Task<SoilHumidityLogEntry> RetrieveSoilHumidityLogEntryFromDevice(DeviceConfiguration deviceConfig, WateringLocation wateringLocation)
        {
            bool success;

            double soilHumidityWateringArea = 0;

            try
            {
                soilHumidityWateringArea = await _greenHouseSparkDeviceService.RequestCurrentSoilMoisturePercentage(deviceConfig, wateringLocation);
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                //Todo: log this error
            }

            if (success)
            {
               var soilHumidityLogEntry = new SoilHumidityLogEntry
                {
                    LogDateUtc = DateTime.UtcNow,
                    WateringLocation = wateringLocation,
                    PercentageOfHumidity = soilHumidityWateringArea
                };

                return soilHumidityLogEntry;
            }
            else
            {
                return null;
            }
        }

        private async Task <AirHumidityLogEntry> RetrieveAirHumidityLogEntryFromDevice(DeviceConfiguration deviceConfig)
        {

            bool success;

            double airTemperature = 0;
            double airHumidity = 0;
            double airDewPoint = 0;

            try
            {
                airTemperature = await _greenHouseSparkDeviceService.RequestCurrentTemperature(deviceConfig);
                airHumidity = await _greenHouseSparkDeviceService.RequestCurrentAirHumidityPercentage(deviceConfig);
                airDewPoint = await _greenHouseSparkDeviceService.RequestCurrentAirDewpoint(deviceConfig);

                success = true;
            }
            catch (Exception e)
            {
                success = false;
                //Todo: log this error
            }

            if (success)
            {
                var airAirHumidityLogEntry = new AirHumidityLogEntry
                {
                    LogDateUtc = DateTime.UtcNow,
                    TemperatureInCelsius = airTemperature,
                    RelativeHumidity = airHumidity,
                    DewPointTemperatureInCelsius = airDewPoint
                };
                return airAirHumidityLogEntry;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}