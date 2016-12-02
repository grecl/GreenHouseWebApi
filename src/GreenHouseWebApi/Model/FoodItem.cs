using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHouseWebApi.Model
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public DateTime Created { get; set; }
    }
}
