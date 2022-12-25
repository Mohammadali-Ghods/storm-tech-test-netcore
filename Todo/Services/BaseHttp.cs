using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services
{
    public static class BaseHttp
    {
        public static async Task<T> Get<T>(Dictionary<string, string> headers, string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                if (headers != null)
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                var responce = await client.GetAsync(url);
                if (responce.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responceString = await responce.Content.ReadAsStringAsync();
                    T resultContent = JsonConvert.DeserializeObject<T>(responceString);
                    return resultContent;
                }
                else 
                {
                    return default;
                }
            }
        }
    }
}
