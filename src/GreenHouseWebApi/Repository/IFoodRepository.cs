using System.Collections.Generic;
using GreenHouseWebApi.Model;

namespace GreenHouseWebApi.Repository
{
    public interface IFoodRepository
    {
        FoodItem GetSingle(int id);
        FoodItem Add(FoodItem item);
        void Delete(int id);
        ICollection<FoodItem> GetAll();
        int Count();
        FoodItem Update(int id, FoodItem item);
    }
}   