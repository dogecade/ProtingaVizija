using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace WindowsForms.FaceAnalysis
{
    public class Faceset
    {
        private static string createUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/create";
        private static string addUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/async/addface";
        private static string searchUrl = "https://api-us.faceplusplus.com/facepp/v3/search";
        private static string removeUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/removeface";
        private static string getDetailUrl = "https://api-us.faceplusplus.com/facepp/v3/faceset/getdetail";

        private static readonly HttpClient client = new HttpClient();
        private string facesetToken;

        public Faceset(string facesetToken)
        {
            this.facesetToken = facesetToken;
        }
        public static async Task<string> CreateNewFaceset(string facesetName)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent facesetNameContent = new StringContent(facesetName);

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetNameContent, "display_name");

                    using (var response = await client.PostAsync(createUrl, formData))
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return responseString;
                        }

                        throw new Exception(responseString);
                    }
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<string> AddFace(string faceToken)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent facesetTokenContent = new StringContent(facesetToken);
            HttpContent faceTokenContent = new StringContent(faceToken);

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                    formData.Add(faceTokenContent, "face_tokens");

                    using (var response = await client.PostAsync(addUrl, formData).ConfigureAwait(false))
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return responseString;
                        }

                        throw new Exception(responseString);
                    }
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<string> RemoveFace(string faceToken)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent facesetTokenContent = new StringContent(facesetToken);
            HttpContent faceTokenContent = new StringContent(faceToken);

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                    formData.Add(faceTokenContent, "face_tokens");

                    using (var response = await client.PostAsync(removeUrl, formData))
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return responseString;
                        }

                        throw new Exception(responseString);
                    }
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<string> SearchFaces(string faceToken)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent faceTokenContent = new StringContent(faceToken);
            HttpContent facesetTokenContent = new StringContent(facesetToken);

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(faceTokenContent, "face_token");
                    formData.Add(facesetTokenContent, "faceset_token");

                    using (var response = await client.PostAsync(searchUrl, formData))
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return responseString;
                        }

                        throw new Exception(responseString);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<string> GetDetail()
        {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent facesetTokenContent = new StringContent(facesetToken);

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");

                    using (var response = await client.PostAsync(getDetailUrl, formData))
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return responseString;
                        }

                        throw new Exception(responseString);
                    }
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
