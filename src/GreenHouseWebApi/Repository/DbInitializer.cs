using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using GreenHouseWebApi.Model;

namespace GreenHouseWebApi.Repository
{
    public static class DbInitializer
    {
        public static void Initialize(GreenHouseDatabaseContext context)
        {
            context.Database.EnsureCreated();

            // Look for any device configurations
            if (context.DeviceConfigurations.Any())
            {
                return;   // DB has been seeded
            }

            var deviceConfiguration = new DeviceConfiguration
            {
                SparkCoreId = "300037001447343339383037",
                SparkCoreAccessToken = "4c98879cda2840fbfdedfa6c6b872264a4015961",
                PollingEnabled = true,
                PostBackUrl = "greuter.azure.com",
                PostBackPort = 8080,
                PostBackIntervalMinutes = 5,

                WateringAreas = new List<WateringArea>(),

            };

            WateringArea areaOne = new WateringArea
            {
                DeviceConfiguration = deviceConfiguration,
                AreaName = "BachSeite",
                MaxSoilHumidity = 80,
                MinSoilHumidity = 50,
                WateringLocation = WateringLocation.First
            };

            WateringArea areaTwo = new WateringArea
            {
                DeviceConfiguration = deviceConfiguration,
                AreaName = "FeldSeite",
                MaxSoilHumidity = 90,
                MinSoilHumidity = 40,
                WateringLocation = WateringLocation.Second
            };


            deviceConfiguration.WateringAreas.Add(areaOne);
            deviceConfiguration.WateringAreas.Add(areaTwo);

            context.DeviceConfigurations.Add(deviceConfiguration);
       
            context.SaveChanges();

        }
    }
}
