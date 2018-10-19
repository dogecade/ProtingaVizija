using FaceAnalysis;
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
            var removeResult = DeleteFacesetAndVerify(createResult.faceset_token);
        }

        [TestMethod]
        public void Faceset_AddSingleFaceToFaceset_Succeeds()
        {
            // Create a new faceset and verify that it was created successfully
            var createResult = CreateFacesetAndVerify(Guid.NewGuid().ToString());

            // Analyze image. Verify
            var analysisResult = AnalyzeFrameAndVerify(Resources.TestImage1, 1);

            // Add image from faceset. Verify
            var addResult = AddImageToFacesetAndVerify(createResult.faceset_token, analysisResult.faces[0].face_token, 1);

            // Try to delete faceset. Should throw an exception as it is not empty (will return 'null')
            Assert.IsNull(faceApiCalls.DeleteFaceset(createResult.faceset_token).Result, "Should be null");

            // Make sure that there is only one face in the faceset
            var detailsResult = GetFacesetDetailsAndVerify(createResult.faceset_token, 1, new List<string>() { analysisResult.faces[0].face_token });

            // Remove face from the faceset. Verify
            var removeFaceResults = RemoveFaceFromFacesetAndVerify(createResult.faceset_token, analysisResult.faces[0].face_token, 0);

            // Remove faceset and verify it was removed successfully
            var removeResult = DeleteFacesetAndVerify(createResult.faceset_token);
        }

        private CreateFaceSetJSON CreateFacesetAndVerify(string facesetname)
        {
            var result = faceApiCalls.CreateNewFaceset(facesetname).Result;
            Assert.AreNotEqual("", result.faceset_token, "Faceset was not created");
            Assert.AreEqual(0, result.face_added, "No faces actually were added");
            Assert.AreEqual(0, result.face_count, "There should be no faces in the newly created faceset");
            Assert.AreEqual(0, result.failure_detail.Count, "There should be no failures while creating a faceset");

            return result;
        }

        private DeleteFacesetJSON DeleteFacesetAndVerify(string facesetToken)
        {
            var result = faceApiCalls.DeleteFaceset(facesetToken).Result;
            Assert.AreEqual(facesetToken, result.faceset_token, "Faceset was not removed");

            return result;
        }

        private FrameAnalysisJSON AnalyzeFrameAndVerify(Bitmap bitmap, int expectedFacesCount)
        {
            bitmap = HelperMethods.ProcessImage(bitmap);
            var result = faceApiCalls.AnalyzeFrame(HelperMethods.ImageToByte(bitmap)).Result;
            Assert.AreEqual(expectedFacesCount, result.faces.Count, "There should be only one face in the picture. Actually found:{0}", result.faces.Count);

            return result;
        }

        private AddFaceJSON AddImageToFacesetAndVerify(string facesetToken, string faceToken, int expectedFacesetFaceCount)
        {
            var result = faceApiCalls.AddFaceToFaceset(facesetToken, faceToken).Result;
            Assert.AreEqual(facesetToken, result.faceset_token, "Face was added to the wrong faceset");
            Assert.AreEqual(1, result.face_added, "There should be exactly 1 face added");
            Assert.AreEqual(expectedFacesetFaceCount, result.face_count, "There should be exactly {0} face in the faceset", expectedFacesetFaceCount);
            Assert.AreEqual(0, result.failure_detail.Count, "There should be no failures");

            return result;
        }

        private FacesetDetailsJSON GetFacesetDetailsAndVerify(string facesetToken, int expectedFacesetFaceCount, List<string> expectedFaceTokens)
        {
            var result = faceApiCalls.GetFacesetDetail(facesetToken).Result;
            Assert.AreEqual(facesetToken, result.faceset_token, "Retrieved details of the wrong faceset");
            Assert.AreEqual(expectedFacesetFaceCount, result.face_count, "There should be exactly {0} face in the faceset", expectedFacesetFaceCount);

            foreach (var expectedFaceToken in expectedFaceTokens)
            {
                Assert.IsTrue(result.face_tokens.Any(x => x.Contains(expectedFaceToken)));
            }

            return result;
        }

        private RemoveFaceJSON RemoveFaceFromFacesetAndVerify(string facesetToken, string faceToken, int expectedFacesetFaceCount)
        {
            var result = faceApiCalls.RemoveFaceFromFaceset(facesetToken, faceToken).Result;
            Assert.AreEqual(facesetToken, result.faceset_token, "Removed face from the wrong faceset");
            Assert.AreEqual(1, result.face_removed, "There should be exactly 1 faces removed from the faceset");
            Assert.AreEqual(expectedFacesetFaceCount, result.face_count, "There should be exactly {0} faces left in the faceset",expectedFacesetFaceCount);
            Assert.AreEqual(0, result.failure_detail.Count, "There should be no failures");

            return result;
        }
    }
}
