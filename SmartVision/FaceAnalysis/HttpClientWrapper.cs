using System.Net.Http;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class HttpClientWrapper
    {
        private static readonly HttpClient httpClient;
        static HttpClientWrapper()
        {
            httpClient = new HttpClient();
        }
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent httpContent)
        {
            return await httpClient.PostAsync(url, httpContent).ConfigureAwait(false);
        }
        public async Task<string> ReadStringAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}