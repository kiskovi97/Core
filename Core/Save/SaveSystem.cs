using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Kiskovi.Core
{
  public class SaveSystem : ISaveSystem
  {
    private string PersistentDataPath = Application.persistentDataPath;

    public SaveSystem(string pPersistentDataPath)
    {
      PersistentDataPath = pPersistentDataPath;
    }

    public async Task<T> GetData<T>() where T : class, IData, new()
    {
      var url = GetFileName(typeof(T));
      var data = await JsonUtilities.LoadJSONAsync<T>(url);
      if (data == null)
        return new T();
      return data;
    }

    public async Task<T> GetData<T>(string fileName) where T : class, IData, new()
    {
      var data = await JsonUtilities.LoadJSONAsync<T>(fileName);
      if (data == null)
        return new T();
      return data;
    }

    public async Task SaveData<T>(T data) where T : class, IData, new()
    {
      if (data == null) return;
      var url = GetFileName(typeof(T));
      await JsonUtilities.SaveJSONAsync(url, data);
    }

    public async Task SaveData<T>(T data, string fileName) where T : class, IData, new()
    {
      if (data == null) return;
      await JsonUtilities.SaveJSONAsync(fileName, data);
    }

    private string GetFileName(Type type)
    {
      return Path.Combine(PersistentDataPath, type.FullName + ".json");
    }
  }
}
