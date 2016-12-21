using System;
using System.Collections.Generic;
using System.Linq;
using GreenHouse.DataModel.Log;
using GreenHouseWebApi.Repository;

namespace GreenHouse.BusinessService.Data
{
    public class LogDataService<TLogEntry> : ILogDataService<TLogEntry>
        where TLogEntry : LogEntryBase
    {
        private readonly ILogRepostory<TLogEntry> _logRepository;
        
        public LogDataService(ILogRepostory<TLogEntry> logRepository)
        {
            _logRepository = logRepository;
        }

        public IEnumerable<TLogEntry> GetAllLogEntries()
        {
            return _logRepository.GetAll();
        }
        
        public void Save(TLogEntry logEntry)
        {
            _logRepository.Add(logEntry);
        }
    }
}