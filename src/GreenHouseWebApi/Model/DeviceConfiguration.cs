using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenHouseWebApi.Model
{
    public class DeviceConfiguration : DeviceSetup
    {
        //todo: define as unique
        public string SparkCoreId { get; set; }

        public string SparkCoreAccessToken { get; set; }

        public ICollection<WateringArea> WateringAreas { get; set; }
    }
}