using System.Collections.Generic;
using System.Linq;
using GreenHouseWebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenHouseWebApi.Repository
{
    public class DeviceSetupRepository : IDeviceSetupRepository
    {
        private GreenHouseDatabaseContext _dbContext;

        public DeviceSetupRepository(GreenHouseDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DeviceConfiguration GetSingle(int id)
        {
            var deviceConfig = _dbContext.DeviceConfigurations.Include(dc=>dc.WateringAreas).SingleOrDefault(dev => dev.Id == id);

            return deviceConfig;
        }

        public DeviceConfiguration Add(DeviceConfiguration item)
        {
            _dbContext.DeviceConfigurations.Add(item);
            _dbContext.SaveChanges();

            return item;
        }

        public void Delete(int id)
        {
            var deviceConfig = GetSingle(id);

            if (deviceConfig != null)
            {
                _dbContext.DeviceConfigurations.Remove(deviceConfig);
            }
            _dbContext.SaveChanges();
        }

        public ICollection<DeviceConfiguration> GetAll()
        {
            List<DeviceConfiguration> deviceSetups = _dbContext.DeviceConfigurations.Include(dc => dc.WateringAreas).ToList();
            return deviceSetups;
        }

        public int Count()
        {
            return _dbContext.DeviceConfigurations.Count();
        }

        public DeviceConfiguration Update(DeviceConfiguration item)
        {
            _dbContext.DeviceConfigurations.Update(item);
            _dbContext.SaveChanges();
            return item;
        }

        public DeviceSetup Update(DeviceSetup deviceSetup)
        {
            DeviceConfiguration existingConfiguration = GetSingle(deviceSetup.Id);
            if (existingConfiguration != null)
            {
                _dbContext.Update(existingConfiguration);

                existingConfiguration.PollingEnabled = deviceSetup.PollingEnabled;
                existingConfiguration.PostBackIntervalMinutes = deviceSetup.PostBackIntervalMinutes;
                existingConfiguration.PostBackUrl = deviceSetup.PostBackUrl;
                existingConfiguration.PostBackPort = deviceSetup.PostBackPort;
                
                _dbContext.SaveChanges();
            }

            return existingConfiguration;
        }

        public WateringArea UpdateWateringArea(int deviceId, WateringArea wateringArea)
        {

            DeviceConfiguration existingConfiguration = GetSingle(deviceId);
            if (existingConfiguration != null)
            {
                var wateringAreaToUpdate =
                    existingConfiguration.WateringAreas.SingleOrDefault(w => w.Id == wateringArea.Id);
                if (wateringAreaToUpdate != null)
                {


                    _dbContext.Update(wateringAreaToUpdate);

                    wateringAreaToUpdate.AreaName = wateringArea.AreaName;
                    wateringAreaToUpdate.MaxSoilHumidity = wateringArea.MaxSoilHumidity;
                    wateringAreaToUpdate.MinSoilHumidity = wateringArea.MinSoilHumidity;

                    _dbContext.SaveChanges();

                    return wateringAreaToUpdate;
                }
            }
            return null;
        }
    }
}