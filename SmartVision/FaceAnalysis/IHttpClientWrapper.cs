using System.Net.Http;

namespace FaceAnalysis
{
    public interface IHttpClientWrapper
    {
        string Post(string url, MultipartFormDataContent httpContent);
    }
}