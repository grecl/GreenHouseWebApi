using System.Collections.Generic;

namespace GreenHouseWebApi.Repository
{
    public interface IDatabaseRepository<T>
    {
        T GetSingle(int id);
        T Add(T item);
        void Delete(int id);
        ICollection<T> GetAll();
        int Count();
        T Update(T item);
    }
}