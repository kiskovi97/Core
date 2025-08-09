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

        public static T LoadJSONSync<T>(string uri) where T : class
        {
            if (string.IsNullOrEmpty(uri) || !File.Exists(uri))
            {
                return null;
            }

            return ReadFileToJSONSync<T>(uri);
        }

        private static async Task<T> ReadFileToJSONAsync<T>(string uri) where T : class
        {
            T obj = null;
            try
            {
                var content = await File.ReadAllTextAsync(uri).ConfigureAwait(false);
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

        private static T ReadFileToJSONSync<T>(string uri) where T : class
        {
            T obj = null;
            try
            {
                var content = File.ReadAllText(uri);
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

                await Task.Run(async () =>
                {
                    string json = JsonConvert.SerializeObject(data, settings);
                    await File.WriteAllTextAsync(uri, json);
                }).ConfigureAwait(false);
            }
            catch (Exception exp)
            {
                Debug.LogWarning(exp);
            }
            finally
            {
            }
        }

        public static void SaveJSONSync(string uri, object data)
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
                File.WriteAllText(uri, json);
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
