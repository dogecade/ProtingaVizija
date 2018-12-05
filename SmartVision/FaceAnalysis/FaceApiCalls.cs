using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Constants;
using Helpers;
using Newtonsoft.Json;

namespace FaceAnalysis
{
    public class FaceApiCalls
    {
        public ApiKeySet ApiKeys
        {
            get
            {
                return keys;
            }
            set
            {
                lock (keysLock)
                    keys = value;
            }
        }
        private readonly object keysLock = new object();
        private const string rootUrl = "https://api-us.faceplusplus.com/facepp/v3/";
        private readonly string detectUrl = rootUrl + "detect";
        private readonly string createUrl = rootUrl + "faceset/create";
        private readonly string addUrl = rootUrl + "faceset/addface";
        private readonly string searchUrl = rootUrl + "search";
        private readonly string removeUrl = rootUrl + "faceset/removeface";
        private readonly string getDetailUrl = rootUrl + "faceset/getdetail";
        private readonly string deleteFacesetUrl = rootUrl + "faceset/delete";
        private ApiKeySet keys;

        private readonly HttpClientWrapper httpClientWrapper;

        public FaceApiCalls(HttpClientWrapper httpClientWrapper)
        {
            this.httpClientWrapper = httpClientWrapper;
            keys = new ApiKeySet(Keys.faceApiKey, Keys.faceApiSecret, Keys.facesetToken);
        }

        public FaceApiCalls(HttpClientWrapper httpClientWrapper, ApiKeySet keySet)
        {
            this.httpClientWrapper = httpClientWrapper;
            keys = keySet;
        }
        
        /// <summary>
        /// Calls frame analyze API
        /// </summary>
        /// <param name="image">Image in byte array format</param>
        /// <returns>FrameAnalysisJSON</returns>
        public async Task<FrameAnalysisJSON> AnalyzeFrame(byte[] image)
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent) = keys.GetHttpContentWithoutFaceset();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                }
                formData.Add(new StringContent(Convert.ToBase64String(image)), "image_base64");

                return DeserializeResponse<FrameAnalysisJSON>(await httpClientWrapper.Post(detectUrl, formData));
            }
        }

        /// <summary>
        /// Creates a new faceset
        /// </summary>
        /// <param name="facesetName">Name of the faceset</param>
        /// <returns>CreateFacesetJSON</returns>
        public async Task<CreateFacesetJSON> CreateNewFaceset(string facesetName)
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent) = keys.GetHttpContentWithoutFaceset();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                }
                formData.Add(new StringContent(facesetName), "display_name");

                return DeserializeResponse<CreateFacesetJSON>(await httpClientWrapper.Post(createUrl, formData));
            }
        }

        /// <summary>
        /// Adds face to the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Face token</param>
        /// <returns>AddFaceJSON</returns>
        public async Task<AddFaceJSON> AddFaceToFaceset(string faceToken)
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent, facesetTokenContent) = keys.GetHttpContent();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                }
                formData.Add(new StringContent(faceToken), "face_tokens");

                return DeserializeResponse<AddFaceJSON>(await httpClientWrapper.Post(addUrl, formData));
            }
        }

        /// <summary>
        /// Removes face from the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Facetoken</param>
        /// <returns>RemoveFaceJSON</returns>
        public async Task<RemoveFaceJSON> RemoveFaceFromFaceset(string faceToken)
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent, facesetTokenContent) = keys.GetHttpContent();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                }
                formData.Add(new StringContent(faceToken), "face_tokens");

                return DeserializeResponse<RemoveFaceJSON>(await httpClientWrapper.Post(removeUrl, formData));
            }
        }

        /// <summary>
        /// Searches for the face in the stored faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>FoundFacesJSON</returns>
        public async Task<FoundFacesJSON> SearchFaceInFaceset(string faceToken)
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent, facesetTokenContent) = keys.GetHttpContent();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                }
                formData.Add(new StringContent(faceToken), "face_token");

                return DeserializeResponse<FoundFacesJSON>(await httpClientWrapper.Post(searchUrl, formData));
            }
        }

        /// <summary>
        /// Returns details of the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>FacesetDetailsJSON</returns>
        public async Task<FacesetDetailsJSON> GetFacesetDetail()
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent, facesetTokenContent) = keys.GetHttpContent();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                }

                return DeserializeResponse<FacesetDetailsJSON>(await httpClientWrapper.Post(getDetailUrl, formData));
            }
        }

        /// <summary>
        /// Deletes the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>DeleteFacesetJSON</returns>
        public async Task<DeleteFacesetJSON> DeleteFaceset()
        {
            using (var formData = new MultipartFormDataContent())
            {
                lock (keysLock)
                {
                    var (keyContent, secretContent, facesetTokenContent) = keys.GetHttpContent();
                    formData.Add(keyContent, "api_key");
                    formData.Add(secretContent, "api_secret");
                    formData.Add(facesetTokenContent, "faceset_token");
                };

                return DeserializeResponse<DeleteFacesetJSON>(await httpClientWrapper.Post(deleteFacesetUrl, formData));
            }
        }

        private T DeserializeResponse<T>(string response) where T : IApiResponseJSON
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("Invalid response received from API");
                return default(T);
            }

        }
    }
}
