using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GreenHouseWebApi.Model;
using ParticleSDK;
using ParticleSDK.Models;


namespace GreenHouse.BusinessService.Device
{
    public class GreenHouseSparkDeviceService : IGreenHouseSparkDeviceService
    { 
        private const string SPARKFUNCTION_ACTION_STARTWATERING = "water";
        private const string SPARKFUNCTION_ACTION_SAVECONFIG = "saveConfig";

        private const string SPARKVARIABLE_CURRENTSTATE = "currentState";
        private const string SPARKVARIABLE_AIRTEMPERATURE = "airtemp";
        private const string SPARKVARIABLE_AIRDEWPOINT = "airDewpoint";
        private const string SPARKVARIABLE_AIRHUMIDITY = "airHumidity";
        private const string SPARKVARIABLE_LIGHTLEVEL = "lightLevel";
        private const string SPARKVARIABLE_ZONEONE_SOILHUMIDITY = "soilHumZone1";
        private const string SPARKVARIABLE_ZONETWO_SOILHUMIDITY = "soilHumZone2";
        
        public GreenHouseSparkDeviceService()
        {
            
        }

        public async Task<SparkDeviceOperationState> RequestCurrentState(DeviceConfiguration deviceConfig)
        {
            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleVariableResponse currentStateResult = await device.GetVariableAsync(SPARKVARIABLE_CURRENTSTATE);

            try
            {
                SparkDeviceOperationState operationState = (SparkDeviceOperationState)int.Parse(currentStateResult.Result);
                return operationState;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Enum value not supported for SparkDeviceOperationState");    
            }  
        }

        private static async Task<ParticleDevice> LoginAndGetParticleDevice(DeviceConfiguration deviceConfig)
        {
            string accessToken = deviceConfig.SparkCoreAccessToken;
            string deviceId = deviceConfig.SparkCoreId;
            await ParticleCloud.SharedCloud.TokenLoginAsync(accessToken);
            var device = await ParticleCloud.SharedCloud.GetDeviceAsync(deviceId);
            return device;
        }

        public async Task<double> RequestCurrentTemperature(DeviceConfiguration deviceConfig)
        {
            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleVariableResponse response = await device.GetVariableAsync(SPARKVARIABLE_AIRTEMPERATURE);

            return double.Parse(response.Result);
        }

        public async Task<double> RequestCurrentAirHumidityPercentage(DeviceConfiguration deviceConfig)
        {
            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleVariableResponse response = await device.GetVariableAsync(SPARKVARIABLE_AIRHUMIDITY);

            return double.Parse(response.Result);
        }

        public async Task<double> RequestCurrentAirDewpoint(DeviceConfiguration deviceConfig)
        {
            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleVariableResponse response = await device.GetVariableAsync(SPARKVARIABLE_AIRDEWPOINT);

            return double.Parse(response.Result);
        }

        public async Task<double> RequestCurrentLightLevelPercentage(DeviceConfiguration deviceConfig)
        {
            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleVariableResponse response = await device.GetVariableAsync(SPARKVARIABLE_LIGHTLEVEL);

            return double.Parse(response.Result);
        }

        public async Task<double> RequestCurrentSoilMoisturePercentage(DeviceConfiguration deviceConfig, WateringLocation wateringLocation)
        {
            string variableName = wateringLocation == WateringLocation.First
                ? SPARKVARIABLE_ZONEONE_SOILHUMIDITY
                : SPARKVARIABLE_ZONETWO_SOILHUMIDITY;

            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleVariableResponse response = await device.GetVariableAsync(variableName);

            return double.Parse(response.Result);
        } 


        /// <summary>
        /// Starts the watering process
        /// </summary>
        /// <param name="wateringLocation"></param>
        /// <returns>Convention: -1 Failure</returns>
        public async void StartWatering(DeviceConfiguration deviceConfig, WateringLocation wateringLocation)
        {
            ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

            ParticleFunctionResponse functionResponse = await device.RunFunctionAsync(SPARKFUNCTION_ACTION_STARTWATERING, Enum.GetName(typeof (WateringLocation), wateringLocation));

            
            if (functionResponse.ReturnValue < 0)
            {
                throw new Exception("Error occurred while starting the watering process");
            }
        }

        public async void SaveDeviceConfiguration(DeviceConfiguration deviceConfig)
        {

            if (deviceConfig != null)
            {
                string setupString = CreateDeviceConfigurationString(deviceConfig);

                ParticleDevice device = await LoginAndGetParticleDevice(deviceConfig);

                //send each config param separately to minimize problems with the size restriction (63 char)
                foreach (string configParameter in setupString.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(configParameter))
                    {
                        ParticleFunctionResponse functionResponse = await device.RunFunctionAsync(SPARKFUNCTION_ACTION_SAVECONFIG, configParameter);
                        
                        if (functionResponse.ReturnValue < 0)
                        {
                            throw new Exception("Error occurred while saving the confguration params");
                        }
                    }
                }
            }
        }

        private string CreateDeviceConfigurationString(DeviceConfiguration deviceConfig)
        {
            StringBuilder configStringBuilder = new StringBuilder();

            configStringBuilder.Append("$pollingon:");
            configStringBuilder.Append(deviceConfig.PollingEnabled?1:0);
            configStringBuilder.Append(";");

            configStringBuilder.Append("$interval:");
            configStringBuilder.Append(deviceConfig.PostBackIntervalMinutes);
            configStringBuilder.Append(";");

            WateringArea firstWateringArea= deviceConfig.WateringAreas.Single(w => w.WateringLocation == WateringLocation.First);
            configStringBuilder.Append("$water1max:");
            configStringBuilder.Append(firstWateringArea.MaxSoilHumidity);
            configStringBuilder.Append(";");

            configStringBuilder.Append("$water1min:");
            configStringBuilder.Append(firstWateringArea.MinSoilHumidity);
            configStringBuilder.Append(";");

            WateringArea secondWateringArea = deviceConfig.WateringAreas.Single(w => w.WateringLocation == WateringLocation.First);
            configStringBuilder.Append("$water2max:");
            configStringBuilder.Append(secondWateringArea.MaxSoilHumidity);
            configStringBuilder.Append(";");

            configStringBuilder.Append("$water2min:");
            configStringBuilder.Append(secondWateringArea.MaxSoilHumidity);
            configStringBuilder.Append(";");

            return configStringBuilder.ToString();

        }
      
    }

}