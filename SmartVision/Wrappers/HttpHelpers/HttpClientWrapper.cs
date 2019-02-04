using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Helpers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient httpClient;
        private string API = "http://viltomas.eu/api/";

        static HttpClientWrapper()
        {
            httpClient = new HttpClient();
        }

        public string PostSync(string url, HttpContent httpContent)
        {
            return Post(url, httpContent).Result;
        }

        public async Task<string> Post(string url, HttpContent httpContent)
        {
            try
            {
                using (var response = await httpClient.PostAsync(url, httpContent))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return responseString;
                    else
                    {
                        throw new HttpRequestException(responseString);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e);
                return null;
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
        public async Task<string> PostImageToApiString(Bitmap img)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            ImageConverter converter = new ImageConverter();
            byte[] imgArray = (byte[])converter.ConvertTo(img, typeof(byte[]));
            form.Add(new ByteArrayContent(imgArray, 0, imgArray.Length), "user_picture", "user_picture.jpg");
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(API + "/ImageUpload"), form);
            string temp = await response.Content.ReadAsStringAsync();
            return temp.Replace(@"""", string.Empty).Replace("/", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
        }

        public async Task<HttpContent> PostRelToApi(Object missingContact)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri(API + "/MissingContact"), missingContact);
            return response.Content;
        }
    }
}