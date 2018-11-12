using System.Net.Http;
using System.Threading.Tasks;

namespace Helpers
{
    public interface IHttpClientWrapper
    {
        Task<string> Post(string url, MultipartFormDataContent httpContent);
        Task<string> Get(string url);
    }
}