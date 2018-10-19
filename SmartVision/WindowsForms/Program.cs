using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsForms.FormControl;

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
            Debug.WriteLine(new FaceAnalysis.FaceApiCalls(new FaceAnalysis.HttpClientWrapper));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormFaceDetection());
        }
    }
}
