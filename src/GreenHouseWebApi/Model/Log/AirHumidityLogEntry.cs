using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenHouse.DataModel.Log
{
    public class AirHumidityLogEntry : LogEntryBase
    {
        public double RelativeHumidity { get; set; }
        public double TemperatureInCelsius { get; set; }
        public double DewPointTemperatureInCelsius { get; set; }
    }
}