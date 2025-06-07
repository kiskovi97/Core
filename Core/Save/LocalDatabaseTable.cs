using System.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace Kiskovi.Core
{
    
    public interface ILocalDatabaseTable
    {
        Task Clear();

        Task SaveToDisk();

        void StartSave();
    }

    public abstract class LocalDatabaseTable<T> : ILocalDatabaseTable, ITickable where T : class, IData, new()
    {
        private ISaveSystem _saveSystem;

        protected T Data { get; private set; } = new T();

        protected bool isSaving;
        protected bool needSaving;

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
            if (isSaving)
            {
                needSaving = true;
                return;
            }
            isSaving = true;
            try
            {
                await _saveSystem.SaveDataAsync(Data);
            } catch(System.Exception e)
            {
                Debug.LogException(e);
            }
            isSaving = false;
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

        public void Tick()
        {
            if (!isSaving && needSaving)
            {
                needSaving = false;
                StartSave();
            }
        }
    }
}
