using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Interface
{
    public interface IGravatarAPI
    {
        Task<GravatarModel> Get(string email);
    }
}
