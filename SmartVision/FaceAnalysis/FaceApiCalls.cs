using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
            get => keys;
            set 
            {
                keysLock.EnterWriteLock();
                keys = value;
                keysLock.ExitWriteLock();
            }
        }
        private readonly ReaderWriterLockSlim keysLock = new ReaderWriterLockSlim();
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
            var (keyPair, secretPair) = keys.GetKeyValuePairsWithoutFaceset();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                new KeyValuePair<string, string>("image_base64", Convert.ToBase64String(image))
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<FrameAnalysisJSON>(await httpClientWrapper.Post(detectUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
        }

        /// <summary>
        /// Creates a new faceset
        /// </summary>
        /// <param name="facesetName">Name of the faceset</param>
        /// <returns>CreateFacesetJSON</returns>
        public async Task<CreateFacesetJSON> CreateNewFaceset(string facesetName)
        {
            var (keyPair, secretPair) = keys.GetKeyValuePairsWithoutFaceset();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                new KeyValuePair<string, string>("display_name", facesetName)
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<CreateFacesetJSON>(await httpClientWrapper.Post(createUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
        }

        /// <summary>
        /// Adds face to the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Face token</param>
        /// <returns>AddFaceJSON</returns>
        public async Task<AddFaceJSON> AddFaceToFaceset(string faceToken)
        {
            var (keyPair, secretPair, facesetPair) = keys.GetKeyValuePairs();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                facesetPair,
                new KeyValuePair<string, string>("face_tokens", faceToken)
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<AddFaceJSON>(await httpClientWrapper.Post(addUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
        }

        /// <summary>
        /// Removes face from the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <param name="faceToken">Facetoken</param>
        /// <returns>RemoveFaceJSON</returns>
        public async Task<RemoveFaceJSON> RemoveFaceFromFaceset(string faceToken)
        {
            var (keyPair, secretPair, facesetPair) = keys.GetKeyValuePairs();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                facesetPair,
                new KeyValuePair<string, string>("face_tokens", faceToken)
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<RemoveFaceJSON>(await httpClientWrapper.Post(removeUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
        }

        /// <summary>
        /// Searches for the face in the stored faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>FoundFacesJSON</returns>
        public async Task<FoundFacesJSON> SearchFaceInFaceset(string faceToken)
        {
            var (keyPair, secretPair, facesetPair) = keys.GetKeyValuePairs();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                facesetPair,
                new KeyValuePair<string, string>("face_token", faceToken)
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<FoundFacesJSON>(await httpClientWrapper.Post(searchUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
        }

        /// <summary>
        /// Returns details of the faceset
        /// </summary>
        /// <param name="facesetToken">Faceset token</param>
        /// <returns>FacesetDetailsJSON</returns>
        public async Task<FacesetDetailsJSON> GetFacesetDetail()
        {
            var (keyPair, secretPair, facesetPair) = keys.GetKeyValuePairs();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                facesetPair
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<FacesetDetailsJSON>(await httpClientWrapper.Post(getDetailUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
        }

        /// <summary>
        /// Deletes the faceset
        /// </summary>
        /// <returns>DeleteFacesetJSON</returns>
        public async Task<DeleteFacesetJSON> DeleteFaceset()
        {
            var (keyPair, secretPair, facesetPair) = keys.GetKeyValuePairs();
            keysLock.EnterReadLock();
            var pairs = new KeyValuePair<string, string>[]
            {
                keyPair,
                secretPair,
                facesetPair
            };
            keysLock.ExitReadLock();

            var encodedPairs = pairs.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));

            return DeserializeResponse<DeleteFacesetJSON>(await httpClientWrapper.Post(deleteFacesetUrl, new StringContent(string.Join("&", encodedPairs), Encoding.UTF8, "application/x-www-form-urlencoded")));
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
