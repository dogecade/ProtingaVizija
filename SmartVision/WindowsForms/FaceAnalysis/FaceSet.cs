using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WindowsForms.FaceAnalysis
{
    class Faceset
    {
        private static string createUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/create";
        private static string addUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/async/addface";
        private static string searchUrl = "https://api-us.faceplusplus.com/facepp/v3/search";
        private static string removeUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/delete";
        private static string getDetailUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/getdetail";


        private static readonly HttpClient client = new HttpClient();
        private string facesetToken;

        public Faceset(string facesetToken)
        {
            this.facesetToken = facesetToken;
        }
        public async Task<string> CreateNewFaceset(string facesetName)
        {
            try
            {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent facesetNameContent = new StringContent(facesetName);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetNameContent, "display_name");

                    var response = await client.PostAsync(createUrl, formData);

                    string responseString = await response.Content.ReadAsStringAsync();

                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

        public async Task<string> AddFace(string faceToken)
        {
            try
            {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent facesetTokenContent = new StringContent(facesetToken);
                HttpContent faceTokenContent = new StringContent(faceToken);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                    formData.Add(faceTokenContent, "face_tokens");

                    var response = await client.PostAsync(addUrl, formData).ConfigureAwait(false);

                    string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

        public async Task<string> RemoveFace(string faceToken)
        {
            try
            {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent facesetTokenContent = new StringContent(facesetToken);
                HttpContent faceTokenContent = new StringContent(faceToken);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                    formData.Add(faceTokenContent, "face_tokens");

                    var response = await client.PostAsync(removeUrl, formData);

                    string responseString = await response.Content.ReadAsStringAsync();

                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

        public async Task<string> SearchFaces(string faceToken)
        {
            try
            {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent faceTokenContent = new StringContent(faceToken);
                HttpContent facesetTokenContent = new StringContent(facesetToken);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(faceTokenContent, "face_token");
                    formData.Add(facesetTokenContent, "faceset_token");

                    var response = await client.PostAsync(searchUrl, formData);

                    string responseString = await response.Content.ReadAsStringAsync();

                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

        public async Task<string>GetDetail()
        {
            try
            {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent facesetTokenContent = new StringContent(facesetToken);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");

                    var response = await client.PostAsync(getDetailUrl, formData);

                    string responseString = await response.Content.ReadAsStringAsync();

                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }
    }
}
