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

        protected virtual async Task LoadFromDisk()
        {
            Data = await _saveSystem.GetData<T>();
        }

        public virtual async Task SaveToDisk()
        {
            await _saveSystem.SaveData(Data);
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
