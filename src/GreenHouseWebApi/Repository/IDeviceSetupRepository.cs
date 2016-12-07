using System.Linq;
using System.Threading.Tasks;
using GreenHouseWebApi.Model;

namespace GreenHouseWebApi.Repository
{
    public interface IDeviceSetupRepository : IDatabaseRepository<DeviceConfiguration>
    {
        DeviceSetup Update(DeviceSetup deviceSetup);
        WateringArea UpdateWateringArea(int deviceId, WateringArea wateringArea);
    }
}
