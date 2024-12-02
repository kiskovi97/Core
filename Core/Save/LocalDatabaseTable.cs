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
        private T _data;
        private SaveSystem _saveSystem;

        public async LocalDatabaseTable(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            await LoadFromDisk();
        }

        protected virtual async Task LoadFromDisk()
        {
            _data = await _saveSystem.GetData<T>();
        }

        public virtual async Task SaveToDisk()
        {
            await _saveSystem.SaveData(_data);
        }

        public virtual async Task Clear()
        {
            _data = new T();
            await SaveToDisk();
        }

        public async void StartSave()
        {
            await SaveToDisk();
        }
    }
}
