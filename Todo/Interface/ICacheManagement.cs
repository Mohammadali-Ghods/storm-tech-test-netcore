using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Interface
{
    public interface ICacheManagement<T>
    {
        public Task<IEnumerable<T>> GetListCache(string cacheKey);
        public Task<T> GetSingleCache(string cacheKey);
        public void SetSingleCache(string cacheKey, T t, int minutes);
        public void SetListCache(string cacheKey, List<T> t, int minutes);

    }
}
