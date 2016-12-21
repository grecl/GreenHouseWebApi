using System.Threading.Tasks;
using GreenHouseWebApi.Model;

namespace GreenHouse.BusinessService.Device
{
    public interface IGreenHouseSparkDeviceService
    {
        Task<SparkDeviceOperationState> RequestCurrentState(DeviceConfiguration deviceConfig);
        Task<double> RequestCurrentTemperature(DeviceConfiguration deviceConfig);
        Task<double> RequestCurrentAirHumidityPercentage(DeviceConfiguration deviceConfig);
        Task<double> RequestCurrentAirDewpoint(DeviceConfiguration deviceConfig);
        Task<double> RequestCurrentLightLevelPercentage(DeviceConfiguration deviceConfig);
        Task<double> RequestCurrentSoilMoisturePercentage(DeviceConfiguration deviceConfig, WateringLocation wateringLocation);

        /// <summary>
        /// Starts the watering process
        /// </summary>
        /// <returns>Convention: -1 Failure</returns>
        void StartWatering(DeviceConfiguration deviceConfig, WateringLocation wateringLocation);

        void SaveDeviceConfiguration(DeviceConfiguration deviceConfig);
    }
}