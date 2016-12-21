using System;
using System.Collections.Generic;
using GreenHouse.DataModel.Log;

namespace GreenHouse.BusinessService.Data
{
    public interface ILogDataService<TLogEntry>
        where TLogEntry : LogEntryBase
    {
        IEnumerable<TLogEntry> GetAllLogEntries();

        void Save(TLogEntry logEntry);
        
    }
}