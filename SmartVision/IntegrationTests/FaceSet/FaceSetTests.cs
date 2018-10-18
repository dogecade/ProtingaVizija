using FaceAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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

        //[TestMethod]
        //public void FacesetMock_AnalyzeFrame_Succeeds()
        //{
        //    string expectedAnalysisString =
        //        "{\"image_id\": \"MtRxWCniGhYNjFZrYbCnjQ==\", \"request_id\": \"1539602847,247870b7-df20-469d-923f-8ecd2e19322c\", \"time_used\": 225, \"faces\": [{\"face_rectangle\": {\"width\": 166, \"top\": 181, \"left\": 215, \"height\": 166}, \"face_token\": \"72db29773b14f18a2a82b4281df2c2f4\"}]}";

        //    var expectedAnalysisObject = JsonConvert.DeserializeObject<FrameAnalysisJSON>(expectedAnalysisString);

        //    var mock = new Mock<IHttpClientWrapper>();

        //    mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
        //        .Returns(expectedAnalysisString);

        //    var actualFacesetObject = JsonConvert.DeserializeObject<FrameAnalysisJSON>(mock.Object.Post(null, null).Result);

        //    Assert.IsTrue(expectedAnalysisObject.Equals(actualFacesetObject), "Mock call to frame analyze api was not succesfull");
        //}

        //[TestMethod]
        //public void FacesetMock_CreateFaceSet_Succeeds()
        //{
        //    string expectedFacesetString =
        //        "{\"faceset_token\": \"6192dcd46df652215295f41be478565c\", \"time_used\": 137, \"face_count\": 0, \"face_added\": 0, \"request_id\": \"1539602878,1d610383-d92a-4222-8868-1095c7e99654\", \"outer_id\": \"\", \"failure_detail\": []}";

        //    var expectedFacesetObject = JsonConvert.DeserializeObject<CreateFaceSetJSON>(expectedFacesetString);

        //    var mock = new Mock<IHttpClientWrapper>();

        //    mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
        //        .Returns(expectedFacesetString);

        //    var actualFacesetObject = JsonConvert.DeserializeObject<CreateFaceSetJSON>(mock.Object.Post(null, null).Result);

        //    Assert.IsTrue(expectedFacesetObject.Equals(actualFacesetObject), "Mock call to faceset create api was not succesfull");
        //}

        //[TestMethod]
        //public void FacesetMock_GetFacesetDetails_Succeeds()
        //{
        //    string expectedDetailsString =
        //        "{\"faceset_token\": \"ebe47a655e523eb2dda3655222e276a6\", \"tags\": \"\", \"time_used\": 99, \"user_data\": \"\", \"display_name\": \"newset\", \"face_tokens\": [\"d800cb1da95a278a487d15a4604c3388\"], \"face_count\": 1, \"request_id\": \"1539613552,d394fe5c-68bd-438f-b1f2-ae9d0de4798d\", \"outer_id\": \"\"}";

        //    var expectedDetailsObject = JsonConvert.DeserializeObject<FacesetDetailsJSON>(expectedDetailsString);

        //    var mock = new Mock<IHttpClientWrapper>();

        //    mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
        //        .Returns(expectedDetailsString);

        //    var actualDetailsObject = JsonConvert.DeserializeObject<FacesetDetailsJSON>(mock.Object.Post(null, null).Result);

        //    Assert.IsTrue(expectedDetailsObject.Equals(actualDetailsObject), "Mock call to faceset details api was not succesfull");
        //}

        //[TestMethod]
        //public void FacesetMock_AddFaceToFaceset_Succeeds()
        //{
        //    string expectedAddString =
        //        "{\"faceset_token\": \"6192dcd46df652215295f41be478565c\", \"time_used\": 664, \"face_count\": 1, \"face_added\": 1, \"request_id\": \"1539602904,1954bd8b-eae9-4abf-9809-ff89fe17a1b2\", \"outer_id\": \"\", \"failure_detail\": []}";

        //    var expectedAddObject = JsonConvert.DeserializeObject<AddFaceJSON>(expectedAddString);

        //    var mock = new Mock<IHttpClientWrapper>();

        //    mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
        //        .Returns(expectedAddString);

        //    var actualAddObject = JsonConvert.DeserializeObject<AddFaceJSON>(mock.Object.Post(null, null).Result);

        //    Assert.IsTrue(expectedAddObject.Equals(actualAddObject), "Mock call to add to faceset api was not succesfull");
        //}

        //[TestMethod]
        //public void FacesetMock_RemoveFaceFromFaceset_Succeeds()
        //{
        //    string expectedRemoveString =
        //        "{\"faceset_token\": \"e8ef78938a6268823c6202931f96afa9\", \"face_removed\": 1, \"time_used\": 181, \"face_count\": 0, \"request_id\": \"1539602962,6ac7d66a-a6d4-4980-b83f-508fe5339d39\", \"outer_id\": \"\", \"failure_detail\": []}";

        //    var expectedRemoveObject = JsonConvert.DeserializeObject<RemoveFaceJSON>(expectedRemoveString);

        //    var mock = new Mock<IHttpClientWrapper>();

        //    mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
        //        .Returns(expectedRemoveString);

        //    var actualRemoveObject = JsonConvert.DeserializeObject<RemoveFaceJSON>(mock.Object.Post(null, null).Result);

        //    Assert.IsTrue(expectedRemoveObject.Equals(actualRemoveObject), "Mock call to face remove api was not succesfull");
        //}

        //[TestMethod]
        //public void FacesetMock_SearchFaceInFaceset_Succeeds()
        //{
        //    string expectedSearchString =
        //        "{\"request_id\": \"1539613554,ff92162a-c7ab-43b2-8f63-b6928ee8aa2f\", \"time_used\": 462, \"thresholds\": {\"1e-3\": 62.327, \"1e-5\": 73.975, \"1e-4\": 69.101}, \"results\": [{\"confidence\": 97.105, \"user_id\": \"\", \"face_token\": \"d800cb1da95a278a487d15a4604c3388\"}]}";

        //    var expectedSearchObject = JsonConvert.DeserializeObject<FoundFacesJSON>(expectedSearchString);

        //    var mock = new Mock<IHttpClientWrapper>();

        //    mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
        //        .Returns(expectedSearchString);

        //    var actualSearchObject = JsonConvert.DeserializeObject<FoundFacesJSON>(mock.Object.Post(null, null).Result);

        //    Assert.IsTrue(expectedSearchObject.Equals(actualSearchObject), "Mock call to search api was not succesfull");
        //}

        [TestMethod]
        public void Faceset_AnalyzeFrame_Succeeds()
        {
            // Analyze image. Verify
            var analysisResult = AnalyzeFrameAndVerify(new Bitmap("..\\..\\TestPictures\\1.jpg"), 1);
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
            var analysisResult = AnalyzeFrameAndVerify(new Bitmap("..\\..\\TestPictures\\1.jpg"), 1);

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
            var result = JsonConvert.DeserializeObject<CreateFaceSetJSON>(faceApiCalls.CreateNewFaceset(facesetname).Result);
            Assert.AreNotEqual("", result.faceset_token, "Faceset was not created");
            Assert.AreEqual(0, result.face_added, "No faces actually were added");
            Assert.AreEqual(0, result.face_count, "There should be no faces in the newly created faceset");
            Assert.AreEqual(0, result.failure_detail.Count, "There should be no failures while creating a faceset");

            return result;
        }

        private DeleteFacesetJSON DeleteFacesetAndVerify(string facesetToken)
        {
            var result = JsonConvert.DeserializeObject<DeleteFacesetJSON>(faceApiCalls.DeleteFaceset(facesetToken).Result);
            Assert.AreEqual(facesetToken, result.faceset_token, "Faceset was not removed");

            return result;
        }

        private FrameAnalysisJSON AnalyzeFrameAndVerify(Bitmap bitmap, int expectedFacesCount)
        {
            bitmap = HelperMethods.ProcessImage(bitmap);
            var result = JsonConvert.DeserializeObject<FrameAnalysisJSON>(faceApiCalls.AnalyzeFrame(HelperMethods.ImageToByte(bitmap)).Result);
            Assert.AreEqual(expectedFacesCount, result.faces.Count, "There should be only one face in the picture. Actually found:{0}", result.faces.Count);

            return result;
        }

        private AddFaceJSON AddImageToFacesetAndVerify(string facesetToken, string faceToken, int expectedFacesetFaceCount)
        {
            var result = JsonConvert.DeserializeObject<AddFaceJSON>(faceApiCalls.AddFaceToFaceset(facesetToken, faceToken).Result);
            Assert.AreEqual(facesetToken, result.faceset_token, "Face was added to the wrong faceset");
            Assert.AreEqual(1, result.face_added, "There should be exactly 1 face added");
            Assert.AreEqual(expectedFacesetFaceCount, result.face_count, "There should be exactly {0} face in the faceset", expectedFacesetFaceCount);
            Assert.AreEqual(0, result.failure_detail.Count, "There should be no failures");

            return result;
        }

        private FacesetDetailsJSON GetFacesetDetailsAndVerify(string facesetToken, int expectedFacesetFaceCount, List<string> expectedFaceTokens)
        {
            var result = JsonConvert.DeserializeObject<FacesetDetailsJSON>(faceApiCalls.GetFacesetDetail(facesetToken).Result);
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
            var result = JsonConvert.DeserializeObject<RemoveFaceJSON>(faceApiCalls.RemoveFaceFromFaceset(facesetToken, faceToken).Result);
            Assert.AreEqual(facesetToken, result.faceset_token, "Removed face from the wrong faceset");
            Assert.AreEqual(1, result.face_removed, "There should be exactly 1 faces removed from the faceset");
            Assert.AreEqual(expectedFacesetFaceCount, result.face_count, "There should be exactly {0} faces left in the faceset",expectedFacesetFaceCount);
            Assert.AreEqual(0, result.failure_detail.Count, "There should be no failures");

            return result;
        }
    }
}
