﻿using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace WindowsForms
{
    class FaceRecognition
    {
        private const string url = "https://api-us.faceplusplus.com/facepp/v3/detect";
        private const string attributes = "gender,age"; // Returns gender and age attributes
        private const string landmark = "2"; // Returns 106 face landmarks

        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// Analyzes the image with face api
        /// </summary>
        /// <param name="bitmap">Image to analyze</param>
        /// <returns>Properties of the faces spotted in image</returns>
        public static string AnalyzeImage(Bitmap bitmap)
        {
            byte[] image = ImageToByte(bitmap);
            string analyzedFace;
            try
            {
                analyzedFace = CallApi(image).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return analyzedFace;
        }
        /// <summary>
        /// Makes an API call
        /// </summary>
        /// <param name="image">Image converted to byte array</param>
        /// Author: Deividas Brazenas
        /// <returns>Analyzed face json</returns>
        private static async Task<string> CallApi(byte[] image)
        {
            try
            {
                HttpContent keyContent = new StringContent(Keys.apiKey);
                HttpContent secretContent = new StringContent(Keys.apiSecret);
                HttpContent imageContent = new StringContent(Convert.ToBase64String(image));
                HttpContent landmarkContent = new StringContent(landmark);
                HttpContent attributesContent = new StringContent(attributes);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keyContent,"api_key" );
                    formData.Add(secretContent,"api_secret");
                    formData.Add(imageContent,"image_base64");
                    formData.Add(landmarkContent,"return_landmark");
                    formData.Add(attributesContent,"return_attributes");

                    var response = await client.PostAsync(url, formData);

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

        private static byte[] ImageToByte(Bitmap img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}