using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient httpClient;
        private string API = "http://localhost:52814/api/";
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
            try
            {

                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
                {
                    Version = HttpVersion.Version10,
                    Content = httpContent
                })
                using (var response = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return responseString;
                    else
                        throw new HttpRequestException(response.ToString());
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
        }
        public async Task<HttpStatusCode> PostMissingPersonToApiAsync(Object missingPerson)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri(API + "/MissingPersons"),missingPerson);
            return response.StatusCode;
        }
        public async Task<HttpStatusCode> PostContactPersonToApiAsync(Object contactPerson)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri(API + "/ContactPersons"), contactPerson);
            return response.StatusCode;
        }
    }
}