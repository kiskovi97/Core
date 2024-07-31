using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;

namespace Kiskovi.Core
{
  internal static class JsonUtilities
  {
    static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
    {
      Formatting = Formatting.Indented,
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      Converters = { new ColorJsonConverter() },
    };

    public static async Task<T> LoadJSONAsync<T>(string uri) where T : class
    {
      if (string.IsNullOrEmpty(uri) || !File.Exists(uri))
      {
        return null;
      }

      return await ReadFileToJSONAsync<T>(uri);
    }

    private static async Task<T> ReadFileToJSONAsync<T>(string uri) where T : class
    {
      T obj = null;
      try
      {
        var content = await File.ReadAllTextAsync(uri);
        if (!string.IsNullOrEmpty(content))
        {
          obj = JsonConvert.DeserializeObject<T>(content, settings);
        }
      }
      catch (Exception exp)
      {
        Debug.LogWarning(exp);
      }
      finally
      {
      }
      return obj;
    }

    public async static Task SaveJSONAsync(string uri, object data)
    {
      if (data == null)
      {
        return;
      }

      try
      {
        string fileDir = Path.GetDirectoryName(uri);
        if (string.IsNullOrEmpty(fileDir) == false)
        {
          if (Directory.Exists(fileDir) == false)
          {
            Directory.CreateDirectory(fileDir);
          }
        }

        string json = JsonConvert.SerializeObject(data, settings);
        await File.WriteAllTextAsync(uri, json);
      }
      catch (Exception exp)
      {
        Debug.LogWarning(exp);
      }
      finally
      {
      }
    }
  }
}
