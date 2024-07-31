
using System.Threading.Tasks;

namespace Kiskovi.Core
{
    public interface ISaveSystem
    {
        public Task<T> GetData<T>() where T : class, IData, new();

        public Task SaveData<T>(T data) where T : class, IData, new();
    }
}
