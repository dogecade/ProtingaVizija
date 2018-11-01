using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient httpClient;
        private string API = "http://viltomas.eu/api/"; //localhost:portas palikti HTTP
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
                using (var response = await httpClient.SendAsync(httpRequestMessage))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return responseString;
                    else
                    {
                        throw new HttpRequestException(response.ToString() + '\n' + responseString + '\n');
                    }
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

        public async Task<string> Get(string url)
        {
            try
            {
                using (var response = await httpClient.GetAsync(url).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return responseString;
                    else
                    {
                        throw new HttpRequestException(response.ToString() + '\n' + responseString + '\n');
                    }
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<HttpContent> PostMissingPersonToApiAsync(Object missingPerson)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri(API + "/MissingPersons"), missingPerson);
            return response.Content;
        }
        public async Task<HttpContent> PostContactPersonToApiAsync(Object contactPerson)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri(API + "/ContactPersons"), contactPerson);
            return response.Content;
        }

        public async Task<HttpContent> PostImageToApi(Bitmap img)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            ImageConverter converter = new ImageConverter();
            byte[] imgArray = (byte[])converter.ConvertTo(img, typeof(byte[]));
            form.Add(new ByteArrayContent(imgArray, 0, imgArray.Length), "user_picture", "user_picture.jpg");
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(API + "/ImageUpload"), form);
            return response.Content;
        }

        public async Task<HttpStatusCode> PostRelToApi(Object missingContact)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri(API + "/MissingContact"), missingContact);
            return response.StatusCode;
        }
    }
}