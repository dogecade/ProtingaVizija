using System;
using System.Diagnostics;
using System.Net;
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

        public string PostSync(string url, MultipartFormDataContent httpContent)
        {
            return Post(url, httpContent).Result;
        }

        public async Task<string> Post(string url, MultipartFormDataContent httpContent, bool repeatedRequest = false)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Version = HttpVersion.Version10,
                Content = httpContent
            };
            try
            {
                using (var response = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return responseString;
                    else
                        throw new HttpRequestException(responseString);
                }
            }

            catch (HttpRequestException e)
            {
                if (e.Message == "Error while copying content to a stream." && repeatedRequest == false)
                {
                    Debug.WriteLine("Bad response, attempting to send request again");
                    return await Post(url, httpContent, true);
                }                  
                else
                {
                    Debug.WriteLine(e);
                    return null;
                }                   
            }
            finally
            {
                httpRequestMessage.Dispose();
            }
        }
    }
}