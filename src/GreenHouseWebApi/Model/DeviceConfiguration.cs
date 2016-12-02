using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenHouseWebApi.Model
{
    public class DeviceConfiguration : DeviceSetup
    {
        [Key]
        public string SparkCoreId { get; set; }

        public string SparkCoreAccessToken { get; set; }

        public ICollection<WateringArea> WateringAreas { get; set; }
    }
}