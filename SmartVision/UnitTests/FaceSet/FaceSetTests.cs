using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsForms;
using WindowsForms.FaceAnalysis;
using WindowsForms.FaceAnalysis.JSON;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestClass]
    public class FaceSetTests
    {
        private static string _faceSetToken;
        private static Faceset fs;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            string guid = Guid.NewGuid().ToString();
            string faceSetName = "FaceSet-" + guid;
            var newFaceSetJSON = Faceset.CreateNewFaceset(faceSetName).Result;

            // Create a new face set
            var faceSetJson = JsonConvert.DeserializeObject<CreateFaceSetJSON>(newFaceSetJSON);
            _faceSetToken = faceSetJson.faceset_token;

            // Verify it was created successfully
            Assert.AreEqual(0, faceSetJson.face_count, "There should be no faces in the new face set");
            Assert.AreEqual(0, faceSetJson.face_added, "There should be no faces added to the new face set");

            fs = new Faceset(_faceSetToken);
        }

        [TestMethod]
        [Description("Tests, whether the face is added to the face set")]
        [TestProperty("Test Author: ", "Deividas Brazenas")]
        public void FaceSet_AddPictureToFaceSet_Succeeds()
        {
            // Analyze image
            Bitmap bitmap = new Bitmap("..\\..\\TestPictures\\1.jpg");
            string analyzedImageJSON = FaceRecognition.AnalyzeImage(bitmap);
            var analyzedImage = JsonConvert.DeserializeObject<AnalyzedFaces>(analyzedImageJSON);

            // Verify it was analyzed correctly
            Assert.AreEqual(1, analyzedImage.faces.Count, "There should be exactly one face in the analyzed image");

            string faceToken = analyzedImage.faces[0].face_token;

            // Add image to face set
            var addedFaceJSON = fs.AddFace(faceToken).Result;

            // Verify it was added
            var faceSetDetailsJSON = fs.GetDetail().Result;
            var faceSetDetails = JsonConvert.DeserializeObject<FacesetDetails>(faceSetDetailsJSON);

            bool wasAdded = false;

            foreach (var token in faceSetDetails.face_tokens)
            {
                if (token.Equals(faceToken))
                {
                    wasAdded = true;
                }
            }

            Assert.IsTrue(wasAdded, "Face was not added to the face set");
        }

        [TestMethod]
        [Description("Tests, whether the face is removed from the face set")]
        [TestProperty("Test Author: ", "Deividas Brazenas")]
        public void FaceSet_RemovePictureFaceSet_Succeeds()
        {
            // Analyze image
            Bitmap bitmap = new Bitmap("..\\..\\TestPictures\\1.jpg");
            string analyzedImageJSON = FaceRecognition.AnalyzeImage(bitmap);
            var analyzedImage = JsonConvert.DeserializeObject<AnalyzedFaces>(analyzedImageJSON);

            // Verify it was analyzed correctly
            Assert.AreEqual(1, analyzedImage.faces.Count, "There should be exactly one face in the analyzed image");

            string faceToken = analyzedImage.faces[0].face_token;

            // Add image to face set
            var addedFaceJSON = fs.AddFace(faceToken).Result;

            // Verify it was added
            var faceSetDetailsJSON = fs.GetDetail().Result;
            var faceSetDetails = JsonConvert.DeserializeObject<FacesetDetails>(faceSetDetailsJSON);

            bool wasAdded = false;

            foreach (var token in faceSetDetails.face_tokens)
            {
                if (token.Equals(faceToken))
                {
                    wasAdded = true;
                }
            }

            Assert.IsTrue(wasAdded, "Face was not added to the face set");

            // Remove face from the face set
            var removedFaceJSON = fs.RemoveFace(faceToken).Result;

            faceSetDetailsJSON = fs.GetDetail().Result;
            faceSetDetails = JsonConvert.DeserializeObject<FacesetDetails>(faceSetDetailsJSON);

            bool wasRemoved = true;

            foreach (var token in faceSetDetails.face_tokens)
            {
                if (token.Equals(faceToken))
                {
                    wasRemoved = false;
                }
            }

            Assert.IsTrue(wasAdded, "Face was not removed from the face set");

        }
    }
}
