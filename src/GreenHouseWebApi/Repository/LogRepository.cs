using System.Collections.Generic;
using System.Linq;
using GreenHouse.DataModel.Log;
using Microsoft.EntityFrameworkCore;

namespace GreenHouseWebApi.Repository
{
    public class LogRepository<TLogEntry> : ILogRepostory<TLogEntry> where TLogEntry : LogEntryBase
    {
        private GreenHouseDatabaseContext _dbContext;

        public LogRepository(GreenHouseDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }


        public TLogEntry GetSingle(int id)
        {
           return  GetLogEntryDbSet().SingleOrDefault(log => log.ID == id) as TLogEntry;
        }

        private DbSet<TLogEntry> GetLogEntryDbSet()
        {

            if (typeof (TLogEntry) == typeof (AirHumidityLogEntry))
            {
                return _dbContext.AirHumidityLogEntries as DbSet<TLogEntry>;
            }
            else
            {
                return _dbContext.SoilHumidityLogEntries as DbSet<TLogEntry>;
            }
        }

        public TLogEntry Add(TLogEntry item)
        {
            GetLogEntryDbSet().Add(item);
            _dbContext.SaveChanges();

            return item;
        }

        public void Delete(int id)
        {
            var logentry = GetSingle(id);

                if (logentry != null)
                {
                    GetLogEntryDbSet().Remove(logentry);
                }

                _dbContext.SaveChanges();
        }

        public ICollection<TLogEntry> GetAll()
        {
            DbSet<TLogEntry> dbSet = GetLogEntryDbSet();
            List<TLogEntry> logs = dbSet.ToList();
            return logs;
        }

        public int Count()
        {
            DbSet<TLogEntry> dbSet = GetLogEntryDbSet();
            return dbSet.Count();
        }

        public TLogEntry Update(TLogEntry item)
        {
            DbSet<TLogEntry> dbSet = GetLogEntryDbSet();
            dbSet.Update(item);
            _dbContext.SaveChanges();
            return item;
        }
    }

    public interface ILogRepostory<TLogEntry> : IDatabaseRepository<TLogEntry> where TLogEntry : LogEntryBase
    {
    }
}
