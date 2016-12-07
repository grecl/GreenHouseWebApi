using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHouseWebApi.Dto
{
    public class DeviceSetupDto
    {
        public int Id { get; set; }

        public bool PollingEnabled { get; set; }
        public string PostBackUrl { get; set; }
        public int PostBackPort { get; set; }
        public int PostBackIntervalMinutes { get; set; }

        public ICollection<int> WateringAreaIds { get; set; }
    }
}
