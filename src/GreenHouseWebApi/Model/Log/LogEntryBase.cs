using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace GreenHouse.DataModel.Log
{
    public class LogEntryBase
    {
        public int ID { get; set; } 
        public DateTime LogDateUtc { get; set; } 
    }
}