using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;
using Todo.Interface;
using Todo.Models;

namespace Todo.Services
{
    public class GravatarAPI: IGravatarAPI
    {
        public async Task<GravatarModel> Get(string email)
        {
            StringBuilder url=new StringBuilder();
            url.Append("http://www.gravatar.com");
            url.Append("/");
            url.Append(Gravatar.GetHash(email));
            url.Append(".json");

            return await BaseHttp.Get<GravatarModel>(null
                , url.ToString());
        }
    }
}
