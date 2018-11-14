using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WindowsForms.FormControl;
using FaceAnalysis;
using Helpers;
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
            //FaceApiCalls f = new FaceApiCalls(new HttpClientWrapper());
            //var x = f.AddFaceToFaceset(Constants.Keys.facesetToken,
            //    f.AnalyzeFrame(HelperMethods.ImageToByte(
            //            new Bitmap("C:\\Users\\Deividas\\Desktop\\12239583_950337065031684_8601253280351957690_n.jpg")))
            //        .Result.Faces[0].Face_token).Result;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormFaceDetection());
        }
    }
}
