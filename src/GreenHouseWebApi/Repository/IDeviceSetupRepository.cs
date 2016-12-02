using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHouseWebApi.Model;

namespace GreenHouseWebApi.Repository
{
    public interface IDeviceSetupRepository : IDatabaseRepository<DeviceSetup>
    {

    }

    public class DeviceSetupRepository : IDeviceSetupRepository
    {
        public DeviceSetup GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        public DeviceSetup Add(DeviceSetup item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<DeviceSetup> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public DeviceSetup Update(int id, DeviceSetup item)
        {
            throw new NotImplementedException();
        }
    }
}
