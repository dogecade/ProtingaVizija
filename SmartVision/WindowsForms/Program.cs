using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsForms.FaceAnalysis;
using WindowsForms.FormControl;
using FaceAnalysis;
using Newtonsoft.Json;

namespace WindowsForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            HttpClientWrapper wrapper = new HttpClientWrapper();
            FaceApiCalls calls = new FaceApiCalls(wrapper);
            Bitmap b = new Bitmap(@"C:\dev\ProtingaVizija\SmartVision\IntegrationTests\TestPictures\1.jpg");
            var result = JsonConvert.DeserializeObject<FrameAnalysisJSON>(calls.AnalyzeImage(HelperMethods.ImageToByte(b)).Result);
            var x = calls.AddFaceToFaceset("57ca7288bb4649fe82f304f0eef6f125", result.faces[0].face_token).Result;
            var y = calls.RemoveFaceFromFaceset("57ca7288bb4649fe82f304f0eef6f125", result.faces[0].face_token).Result;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormFaceDetection());
        }
    }
}
