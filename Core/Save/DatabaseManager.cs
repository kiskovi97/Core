using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    public interface IRemoteDatabaseTable
    {
        Task FetchRemote();
    }

    public interface IDatabaseManager
    {
        public class DatabaseChangedSignal { }

        Task ClearAll();

        Task FetchRemote();

        Task SaveToDisk();
    }

    internal class DatabaseManager : IDatabaseManager
    {
        public static bool IsInitialized { get; set; } = false;

        private SignalBus _signalBus;

        private IEnumerable<ILocalDatabaseTable> _localTables;
        private IEnumerable<IRemoteDatabaseTable> _remoteTables;


        internal DatabaseManager(IEnumerable<ILocalDatabaseTable> localTables, IEnumerable<IRemoteDatabaseTable> remoteTables, SignalBus signalBus)
        {
            _localTables = localTables;
            _remoteTables = remoteTables;
            _signalBus = signalBus;

            IsInitialized = true;
            
            _signalBus.TryFire(new IDatabaseManager.DatabaseChangedSignal());
        }

        public async Task ClearAll()
        {
            foreach (var table in _localTables)
            {
                await table.Clear();
            }
            
            _signalBus.TryFire(new IDatabaseManager.DatabaseChangedSignal());
        }

        public async Task FetchRemote()
        {
            foreach(var table in _remoteTables)
                await table.FetchRemote();

            _signalBus.TryFire(new IDatabaseManager.DatabaseChangedSignal());
        }

        public async Task SaveToDisk()
        {
            Debug.Log("Saving Database");

            foreach (var database in _localTables)
            {
                await database.SaveToDisk();
            }

            _signalBus.TryFire(new IDatabaseManager.DatabaseChangedSignal());
        }
    }
}
