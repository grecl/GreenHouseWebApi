using GreenHouseWebApi.Model;

namespace GreenHouseWebApi.Dto
{
    public class WateringAreaDto
    {
        public int Id { get; set; }

        public int DeviceConfigurationId { get; set; }

        public WateringLocation WateringLocation { get; set; }
            
        public string AreaName { get; set; }
        public int? MaxSoilHumidity { get; set; }
        public int? MinSoilHumidity { get; set; }
    }
}