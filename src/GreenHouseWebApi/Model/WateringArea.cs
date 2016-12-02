using GreenHouseWebApi.Repository;

namespace GreenHouseWebApi.Model
{
    public class WateringArea
    {
        public int Id { get; set; }

        public DeviceConfiguration DeviceConfiguration { get; set; }
        public WateringLocation WateringLocation { get; set; }

        public string AreaName { get; set; }
        public int? MaxSoilHumidity { get; set; }
        public int? MinSoilHumidity { get; set; }
    }
}