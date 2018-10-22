﻿using FaceAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class FaceSetTests
    {
        private static readonly FaceApiCalls faceApiCalls = new FaceApiCalls(new HttpClientWrapper());

        [TestMethod]
        public void Faceset_AnalyzeFrame_Succeeds()
        {
            // Analyze image. Verify
            var analysisResult = AnalyzeFrameAndVerify(Resources.TestImage1, 1);
        }

        [TestMethod]
        public void Faceset_CreateAndRemoveFaceset_Succeeds()
        {
            // Create a new faceset and verify that it was created successfully
            var createResult = CreateFacesetAndVerify(Guid.NewGuid().ToString());

            // Remove faceset and verify it was removed successfully
            var removeResult = DeleteFacesetAndVerify(createResult.Faceset_token);
        }

        [TestMethod]
        public void Faceset_AddSingleFaceToFaceset_Succeeds()
        {
            // Create a new faceset and verify that it was created successfully
            var createResult = CreateFacesetAndVerify(Guid.NewGuid().ToString());

            // Analyze image. Verify
            var analysisResult = AnalyzeFrameAndVerify(Resources.TestImage1, 1);

            // Add image from faceset. Verify
            var addResult = AddImageToFacesetAndVerify(createResult.Faceset_token, analysisResult.Faces[0].Face_token, 1);

            // Try to delete faceset. Should throw an exception as it is not empty (will return 'null')
            Assert.IsNull(faceApiCalls.DeleteFaceset(createResult.Faceset_token).Result, "Should be null");

            // Make sure that there is only one face in the faceset
            var detailsResult = GetFacesetDetailsAndVerify(createResult.Faceset_token, 1, new List<string>() { analysisResult.Faces[0].Face_token });

            // Remove face from the faceset. Verify
            var removeFaceResults = RemoveFaceFromFacesetAndVerify(createResult.Faceset_token, analysisResult.Faces[0].Face_token, 0);

            // Remove faceset and verify it was removed successfully
            var removeResult = DeleteFacesetAndVerify(createResult.Faceset_token);
        }

        private CreateFacesetJSON CreateFacesetAndVerify(string facesetname)
        {
            var result = faceApiCalls.CreateNewFaceset(facesetname).Result;
            Assert.IsNotNull(result, "Invalid API response");
            Assert.AreNotEqual("", result.Faceset_token, "Faceset was not created");
            Assert.AreEqual(0, result.Face_added, "No faces actually were added");
            Assert.AreEqual(0, result.Face_count, "There should be no faces in the newly created faceset");
            Assert.AreEqual(0, result.Failure_detail.Count, "There should be no failures while creating a faceset");

            return result;
        }

        private DeleteFacesetJSON DeleteFacesetAndVerify(string facesetToken)
        {
            var result = faceApiCalls.DeleteFaceset(facesetToken).Result;
            Assert.IsNotNull(result, "Invalid API response");
            Assert.AreEqual(facesetToken, result.Faceset_token, "Faceset was not removed");

            return result;
        }

        private FrameAnalysisJSON AnalyzeFrameAndVerify(Bitmap bitmap, int expectedFacesCount)
        {
            bitmap = HelperMethods.ProcessImage(bitmap);
            var result = faceApiCalls.AnalyzeFrame(HelperMethods.ImageToByte(bitmap)).Result;
            Assert.IsNotNull(result, "Invalid API response");
            Assert.AreEqual(expectedFacesCount, result.Faces.Count, "There should be only one face in the picture. Actually found:{0}", result.Faces.Count);

            return result;
        }

        private AddFaceJSON AddImageToFacesetAndVerify(string facesetToken, string faceToken, int expectedFacesetFaceCount)
        {
            var result = faceApiCalls.AddFaceToFaceset(facesetToken, faceToken).Result;
            Assert.AreEqual(facesetToken, result.Faceset_token, "Face was added to the wrong faceset");
            Assert.AreEqual(1, result.Face_added, "There should be exactly 1 face added");
            Assert.AreEqual(expectedFacesetFaceCount, result.Face_count, "There should be exactly {0} face in the faceset", expectedFacesetFaceCount);
            Assert.AreEqual(0, result.Failure_detail.Count, "There should be no failures");

            return result;
        }

        private FacesetDetailsJSON GetFacesetDetailsAndVerify(string facesetToken, int expectedFacesetFaceCount, List<string> expectedFaceTokens)
        {
            var result = faceApiCalls.GetFacesetDetail(facesetToken).Result;
            Assert.AreEqual(facesetToken, result.Faceset_token, "Retrieved details of the wrong faceset");
            Assert.AreEqual(expectedFacesetFaceCount, result.Face_count, "There should be exactly {0} face in the faceset", expectedFacesetFaceCount);

            foreach (var expectedFaceToken in expectedFaceTokens)
            {
                Assert.IsTrue(result.Face_tokens.Any(x => x.Contains(expectedFaceToken)));
            }

            return result;
        }

        private RemoveFaceJSON RemoveFaceFromFacesetAndVerify(string facesetToken, string faceToken, int expectedFacesetFaceCount)
        {
            var result = faceApiCalls.RemoveFaceFromFaceset(facesetToken, faceToken).Result;
            Assert.AreEqual(facesetToken, result.Faceset_token, "Removed face from the wrong faceset");
            Assert.AreEqual(1, result.Face_removed, "There should be exactly 1 faces removed from the faceset");
            Assert.AreEqual(expectedFacesetFaceCount, result.Face_count, "There should be exactly {0} faces left in the faceset",expectedFacesetFaceCount);
            Assert.AreEqual(0, result.Failure_detail.Count, "There should be no failures");

            return result;
        }
    }
}
