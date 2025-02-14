using System.Threading.Tasks;

using UnityEngine;

namespace Kiskovi.Core
{
    
    public interface ILocalDatabaseTable
    {
        Task Clear();

        Task SaveToDisk();

        void StartSave();
    }

    public abstract class LocalDatabaseTable<T> : ILocalDatabaseTable where T : class, IData, new()
    {
        private ISaveSystem _saveSystem;

        protected T Data { get; private set; } = new T();

        public LocalDatabaseTable(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            LoadFromDisk();
        }

        protected virtual void LoadFromDisk()
        {
            Data = _saveSystem.GetDataSync<T>();
        }

        public virtual async Task SaveToDisk()
        {
            await _saveSystem.SaveDataAsync(Data);
        }

        public virtual async Task Clear()
        {
            Data = new T();
            await SaveToDisk();
        }

        public async void StartSave()
        {
            await SaveToDisk();
        }
    }
}
