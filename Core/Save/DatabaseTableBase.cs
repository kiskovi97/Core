using System.Threading.Tasks;

using UnityEngine;

namespace Kiskovi.Core
{
    
    public interface ILocalDatabaseTable
    {
        Task Clear();

        Task SaveToDisk();
    }

    public abstract class LocalDatabaseTableBase<T> : ILocalDatabaseTable where T : class, IData, new()
    {
        private T _data;
        private SaveSystem _saveSystem;

        public async DatabaseTableBase(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            await LoadFromDisk();
        }

        private virtual async Task LoadFromDisk()
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
    }
}
