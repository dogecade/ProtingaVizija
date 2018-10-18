using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class FaceApiCalls
    {
        private const string rootUrl = "https://api-us.faceplusplus.com/facepp/v3/";
        private readonly string detectUrl = rootUrl + "detect";
        private readonly string createUrl = rootUrl + "faceset/create";
        private readonly string addUrl = rootUrl + "faceset/addface";
        private readonly string searchUrl = rootUrl + "search";
        private readonly string removeUrl = rootUrl + "faceset/removeface";
        private readonly string getDetailUrl = rootUrl + "faceset/getdetail";
        private readonly string deleteFacesetUrl = rootUrl + "faceset/delete";

        private readonly HttpClientWrapper httpClientWrapper;

        public FaceApiCalls(HttpClientWrapper httpClientWrapper)
        {
            this.httpClientWrapper = httpClientWrapper;
        }

        /// <summary>
        /// Calls frame analyze API
        /// </summary>
        /// <param name="image">Image in byte array format</param>
        /// <returns>FrameAnalysisJSON</returns>
        public async Task<string> AnalyzeFrame(byte[] image)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent imageContent = new StringContent(Convert.ToBase64String(image));

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(keyContent, "api_key");
                formData.Add(secretContent, "api_secret");
                formData.Add(imageContent, "image_base64");

                return await httpClientWrapper.Post(detectUrl, formData);
            }
        }

        /// <summary>
        /// Creates a new faceset
        /// </summary>
        /// <param name="facesetName">Name of the faceset</param>
        /// <returns>CreateFaceSetJSON</returns>
        public async Task<string> CreateNewFaceset(string facesetName)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent facesetNameContent = new StringContent(facesetName);

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(keyContent, "api_key");
                formData.Add(secretContent, "api_secret");
                formData.Add(facesetNameContent, "display_name");

                return await httpClientWrapper.Post(createUrl, formData);
            }
        }

        /// <summary>
        /// Adds face to the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Face token</param>
        /// <returns>AddFaceJSON</returns>
        public async Task<string> AddFaceToFaceset(string facesetToken, string faceToken)
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

                return await httpClientWrapper.Post(addUrl, formData);
            }
        }

        /// <summary>
        /// Removes face from the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Facetoken</param>
        /// <returns>RemoveFaceJSON</returns>
        public async Task<string> RemoveFaceFromFaceset(string facesetToken, string faceToken)
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

                return await httpClientWrapper.Post(removeUrl, formData);
            }
        }

        /// <summary>
        /// Searches for the face in the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Face token</param>
        /// <returns>FoundFacesJSON</returns>
        public async Task<string> SearchFaceInFaceset(string facesetToken, string faceToken)
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

                return await httpClientWrapper.Post(searchUrl, formData);
            }
        }

        /// <summary>
        /// Returns details of the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>FacesetDetailsJSON</returns>
        public async Task<string> GetFacesetDetail(string facesetToken)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent facesetTokenContent = new StringContent(facesetToken);

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(keyContent, "api_key");
                formData.Add(secretContent, "api_secret");
                formData.Add(facesetTokenContent, "faceset_token");

                return await httpClientWrapper.Post(getDetailUrl, formData);
            }
        }

        /// <summary>
        /// Deletes the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>DeleteFacesetJSON</returns>
        public async Task<string> DeleteFaceset(string facesetToken)
        {
            HttpContent keyContent = new StringContent(Keys.apiKey);
            HttpContent secretContent = new StringContent(Keys.apiSecret);
            HttpContent facesetTokenContent = new StringContent(facesetToken);

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(keyContent, "api_key");
                formData.Add(secretContent, "api_secret");
                formData.Add(facesetTokenContent, "faceset_token");

                return await httpClientWrapper.Post(deleteFacesetUrl, formData);
            }
        }
    }
}
