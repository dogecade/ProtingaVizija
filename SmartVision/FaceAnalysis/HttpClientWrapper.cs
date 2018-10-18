using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient httpClient;
        static HttpClientWrapper()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> Post(string url, MultipartFormDataContent httpContent)
        {
            return await PostRequest(url, httpContent);
        }

        private async Task<string> PostRequest(string url, MultipartFormDataContent httpContent)
        {
            try
            {
                using (var response = await httpClient.PostAsync(url, httpContent).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return responseString;
                    }
                    
                    throw new Exception(response.StatusCode.ToString());
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}