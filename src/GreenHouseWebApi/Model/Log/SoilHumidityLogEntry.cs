using System;
using System.Collections.Generic;
using System.Linq;
using GreenHouseWebApi.Model;

namespace GreenHouse.DataModel.Log
{
    public class SoilHumidityLogEntry : LogEntryBase
    {
        public WateringLocation WateringLocation { get; set; }
        public double PercentageOfHumidity { get; set; }
    }
}