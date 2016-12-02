using GreenHouseWebApi.Dto;

namespace GreenHouseWebApi.Model
{
    public class DeviceSetup
    {
        public bool PollingEnabled { get; set; }
        public string PostBackUrl { get; set; }
        public int PostBackPort { get; set; }
        public int PostBackIntervalMinutes { get; set; }
    }
}