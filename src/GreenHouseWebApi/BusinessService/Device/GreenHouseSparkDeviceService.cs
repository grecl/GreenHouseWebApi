using System;
using System.Linq;
using System.Text;
using GreenHouseWebApi.Model;

namespace GreenHouse.BusinessService.Device
{
    public class GreenHouseSparkDeviceService : IGreenHouseSparkDeviceService
    {
        /*
        private const string SPARKFUNCTION_ACTION_STARTWATERING = "water";
        private const string SPARKFUNCTION_ACTION_SAVECONFIG = "saveConfig";

        private const string SPARKVARIABLE_CURRENTSTATE = "currentState";
        private const string SPARKVARIABLE_AIRTEMPERATURE = "airtemp";
        private const string SPARKVARIABLE_AIRDEWPOINT = "airDewpoint";
        private const string SPARKVARIABLE_AIRHUMIDITY = "airHumidity";
        private const string SPARKVARIABLE_LIGHTLEVEL = "lightLevel";
        private const string SPARKVARIABLE_ZONEONE_SOILHUMIDITY = "soilHumZone1";
        private const string SPARKVARIABLE_ZONETWO_SOILHUMIDITY = "soilHumZone2";

        private readonly SparkClient _sparkClient;
        private string _accessToken;
        private string _coreId;
        

        public GreenHouseSparkDeviceService()
        {
            _accessToken = System.Web.Configuration.WebConfigurationManager.AppSettings["SparkApi:AccessToken"];
            _coreId = System.Web.Configuration.WebConfigurationManager.AppSettings["SparkApi:CoreId"];
            _sparkClient = new SparkClient(_accessToken, _coreId);
        }

        public SparkDeviceOperationState RequestCurrentState(string sparkCoreId)
        {
            SparkVariableResult result = _sparkClient.GetVariable(SPARKVARIABLE_CURRENTSTATE);

            if (result.HasError)
            {
                throw new SparkDeviceException("Error occurred while requesting current state", result);
            }

            try
            {
                SparkDeviceOperationState operationState = (SparkDeviceOperationState) result.GetIntValue();
                return operationState;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Enum value not supported for SparkDeviceOperationState");    
            }
            
        }

        public double RequestCurrentTemperature(string sparkCoreId)
        {
            SparkVariableResult result = _sparkClient.GetVariable(SPARKVARIABLE_AIRTEMPERATURE);

            if (result.HasError)
            {
                throw new SparkDeviceException("Error occurred while requesting current air temperature", result);
            }

            return result.GetDoubleValue();
        }

        public double RequestCurrentAirHumidityPercentage(string sparkCoreId)
        {
            SparkVariableResult result = _sparkClient.GetVariable(SPARKVARIABLE_AIRHUMIDITY);

            if (result.HasError)
            {
                throw new SparkDeviceException("Error occurred while requesting current air humidity", result); 
            }

            return result.GetDoubleValue();
        }

        public double RequestCurrentAirDewpoint(string sparkCoreId)
        {
            SparkVariableResult result = _sparkClient.GetVariable(SPARKVARIABLE_AIRDEWPOINT);

            if (result.HasError)
            {
                throw new SparkDeviceException("Error occurred while requesting current air dewpoint", result);
            }

            return result.GetDoubleValue();
        }

        public double RequestCurrentLightLevelPercentage()
        {
            SparkVariableResult result = _sparkClient.GetVariable(SPARKVARIABLE_LIGHTLEVEL);

            if (result.HasError)
            {
                throw new SparkDeviceException("Error occurred while requesting current light level", result);
            }

            return result.GetDoubleValue();
        }

        public double RequestCurrentSoilMoisturePercentage(string sparkCoreId, WateringLocation wateringLocation)
        {
            string variableName = wateringLocation == WateringLocation.First
                ? SPARKVARIABLE_ZONEONE_SOILHUMIDITY
                : SPARKVARIABLE_ZONETWO_SOILHUMIDITY;
            SparkVariableResult result = _sparkClient.GetVariable(variableName);

            if (result.HasError)
            {
                throw new SparkDeviceException("Error occurred while requesting current soil humidity", result);
            }

            return result.GetDoubleValue();
        } 


        /// <summary>
        /// Starts the watering process
        /// </summary>
        /// <param name="wateringLocation"></param>
        /// <returns>Convention: -1 Failure</returns>
        public void StartWatering(WateringLocation wateringLocation)
        {
           int result = _sparkClient.ExecuteFunctionReturnValue(SPARKFUNCTION_ACTION_STARTWATERING, Enum.GetName(typeof(WateringLocation), wateringLocation));
            if (result < 0)
            {
                throw new SparkDeviceException("Error occurred while starting the watering process");
            }
        }

        public void SaveDeviceConfiguration(DeviceConfiguration deviceConfig)
        {
            if (deviceConfig != null)
            {
                string setupString = CreateDeviceConfigurationString(deviceConfig);

                //send each config param separately to minimize problems with the size restriction (63 char)
                foreach (string configParameter in setupString.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(configParameter))
                    {
                        var result = _sparkClient.ExecuteFunctionReturnValue(SPARKFUNCTION_ACTION_SAVECONFIG,
                            configParameter);
                        if (result < 0)
                        {
                            throw new SparkDeviceException("Error occurred while saving the confguration params");
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
        */
        public SparkDeviceOperationState RequestCurrentState(string sparkCoreId)
        {
            throw new NotImplementedException();
        }

        public double RequestCurrentTemperature(string sparkCoreId)
        {
            throw new NotImplementedException();
        }

        public double RequestCurrentAirHumidityPercentage(string sparkCoreId)
        {
            throw new NotImplementedException();
        }

        public double RequestCurrentAirDewpoint(string sparkCoreId)
        {
            throw new NotImplementedException();
        }

        public double RequestCurrentLightLevelPercentage()
        {
            throw new NotImplementedException();
        }

        public double RequestCurrentSoilMoisturePercentage(string sparkCoreId, WateringLocation wateringLocation)
        {
            throw new NotImplementedException();
        }

        public void StartWatering(WateringLocation wateringLocation)
        {
            throw new NotImplementedException();
        }

        public void SaveDeviceConfiguration(DeviceSetup deviceSetup)
        {
            throw new NotImplementedException();
        }
    }
}