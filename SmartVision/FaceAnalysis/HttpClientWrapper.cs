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

        public string Post(string url, MultipartFormDataContent httpContent)
        {
            return PostRequest(url, httpContent).Result;
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

                    throw new Exception(responseString);
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