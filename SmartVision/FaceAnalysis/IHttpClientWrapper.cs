using System.Net.Http;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public interface IHttpClientWrapper
    {
        Task<string> Post(string url, MultipartFormDataContent httpContent, bool repeatedRequest = false);
    }
}