using FaceAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
namespace UnitTests
{
    [TestClass]
    public class FaceSetTests
    {
        [TestMethod]
        public void FaceSetMock_AnalyzeFrame_Succeeds()
        {
            string expectedAnalysisString =
                "{\"image_id\": \"MtRxWCniGhYNjFZrYbCnjQ==\", \"request_id\": \"1539602847,247870b7-df20-469d-923f-8ecd2e19322c\", \"time_used\": 225, \"faces\": [{\"face_rectangle\": {\"width\": 166, \"top\": 181, \"left\": 215, \"height\": 166}, \"face_token\": \"72db29773b14f18a2a82b4281df2c2f4\"}]}";

            var expectedAnalysisObject = JsonConvert.DeserializeObject<FrameAnalysisJSON>(expectedAnalysisString);

            var mock = new Mock<IHttpClientWrapper>();

            mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
                .Returns(expectedAnalysisString);

            var actualFacesetObject = JsonConvert.DeserializeObject<FrameAnalysisJSON>(mock.Object.Post(null, null).Result);

            Assert.IsTrue(expectedAnalysisObject.Equals(actualFacesetObject), "Mock call to frame analyze api was not succesfull");
        }

        [TestMethod]
        public void FaceSetMock_CreateFaceSet_Succeeds()
        {
            string expectedFacesetString =
                "{\"faceset_token\": \"6192dcd46df652215295f41be478565c\", \"time_used\": 137, \"face_count\": 0, \"face_added\": 0, \"request_id\": \"1539602878,1d610383-d92a-4222-8868-1095c7e99654\", \"outer_id\": \"\", \"failure_detail\": []}";

            var expectedFacesetObject = JsonConvert.DeserializeObject<CreateFaceSetJSON>(expectedFacesetString);

            var mock = new Mock<IHttpClientWrapper>();

            mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
                .Returns(expectedFacesetString);

            var actualFacesetObject = JsonConvert.DeserializeObject<CreateFaceSetJSON>(mock.Object.Post(null, null).Result);

            Assert.IsTrue(expectedFacesetObject.Equals(actualFacesetObject), "Mock call to faceset create api was not succesfull");
        }

        [TestMethod]
        public void FaceSetMock_GetFacesetDetails_Succeeds()
        {
            string expectedDetailsString =
                "{\"faceset_token\": \"ebe47a655e523eb2dda3655222e276a6\", \"tags\": \"\", \"time_used\": 99, \"user_data\": \"\", \"display_name\": \"newset\", \"face_tokens\": [\"d800cb1da95a278a487d15a4604c3388\"], \"face_count\": 1, \"request_id\": \"1539613552,d394fe5c-68bd-438f-b1f2-ae9d0de4798d\", \"outer_id\": \"\"}";

            var expectedDetailsObject = JsonConvert.DeserializeObject<FacesetDetailsJSON>(expectedDetailsString);

            var mock = new Mock<IHttpClientWrapper>();

            mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
                .Returns(expectedDetailsString);

            var actualDetailsObject = JsonConvert.DeserializeObject<FacesetDetailsJSON>(mock.Object.Post(null, null).Result);

            Assert.IsTrue(expectedDetailsObject.Equals(actualDetailsObject), "Mock call to faceset details api was not succesfull");
        }

        [TestMethod]
        public void FaceSetMock_AddFaceToFaceset_Succeeds()
        {
            string expectedAddString =
                "{\"faceset_token\": \"6192dcd46df652215295f41be478565c\", \"time_used\": 664, \"face_count\": 1, \"face_added\": 1, \"request_id\": \"1539602904,1954bd8b-eae9-4abf-9809-ff89fe17a1b2\", \"outer_id\": \"\", \"failure_detail\": []}";

            var expectedAddObject = JsonConvert.DeserializeObject<AddFaceJSON>(expectedAddString);

            var mock = new Mock<IHttpClientWrapper>();

            mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
                .Returns(expectedAddString);

            var actualAddObject = JsonConvert.DeserializeObject<AddFaceJSON>(mock.Object.Post(null, null).Result);

            Assert.IsTrue(expectedAddObject.Equals(actualAddObject), "Mock call to add to faceset api was not succesfull");
        }

        [TestMethod]
        public void FaceSetMock_RemoveFaceFromFaceset_Succeeds()
        {
            string expectedRemoveString =
                "{\"faceset_token\": \"e8ef78938a6268823c6202931f96afa9\", \"face_removed\": 1, \"time_used\": 181, \"face_count\": 0, \"request_id\": \"1539602962,6ac7d66a-a6d4-4980-b83f-508fe5339d39\", \"outer_id\": \"\", \"failure_detail\": []}";

            var expectedRemoveObject = JsonConvert.DeserializeObject<RemoveFaceJSON>(expectedRemoveString);

            var mock = new Mock<IHttpClientWrapper>();

            mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
                .Returns(expectedRemoveString);

            var actualRemoveObject = JsonConvert.DeserializeObject<RemoveFaceJSON>(mock.Object.Post(null, null).Result);

            Assert.IsTrue(expectedRemoveObject.Equals(actualRemoveObject), "Mock call to face remove api was not succesfull");
        }

        [TestMethod]
        public void FaceSetMock_SearchFaceInFaceset_Succeeds()
        {
            string expectedSearchString =
                "{\"request_id\": \"1539613554,ff92162a-c7ab-43b2-8f63-b6928ee8aa2f\", \"time_used\": 462, \"thresholds\": {\"1e-3\": 62.327, \"1e-5\": 73.975, \"1e-4\": 69.101}, \"results\": [{\"confidence\": 97.105, \"user_id\": \"\", \"face_token\": \"d800cb1da95a278a487d15a4604c3388\"}]}";

            var expectedSearchObject = JsonConvert.DeserializeObject<FoundFacesJSON>(expectedSearchString);

            var mock = new Mock<IHttpClientWrapper>();

            mock.Setup(m => m.Post(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()).Result)
                .Returns(expectedSearchString);

            var actualSearchObject = JsonConvert.DeserializeObject<FoundFacesJSON>(mock.Object.Post(null, null).Result);

            Assert.IsTrue(expectedSearchObject.Equals(actualSearchObject), "Mock call to search api was not succesfull");
        }
    }
}
