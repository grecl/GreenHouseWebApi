using GreenHouseWebApi.Model;

namespace GreenHouse.BusinessService.Device
{
    public interface IGreenHouseSparkDeviceService
    {
        SparkDeviceOperationState RequestCurrentState(string sparkCoreId);
        double RequestCurrentTemperature(string sparkCoreId);
        double RequestCurrentAirHumidityPercentage(string sparkCoreId);
        double RequestCurrentAirDewpoint(string sparkCoreId);

        double RequestCurrentLightLevelPercentage();
        double RequestCurrentSoilMoisturePercentage(string sparkCoreId, WateringLocation wateringLocation);

        /// <summary>
        /// Starts the watering process
        /// </summary>
        /// <param name="wateringLocation"></param>
        /// <returns>Convention: -1 Failure</returns>
        void StartWatering(WateringLocation wateringLocation);

        void SaveDeviceConfiguration(DeviceSetup deviceSetup);
    }
}