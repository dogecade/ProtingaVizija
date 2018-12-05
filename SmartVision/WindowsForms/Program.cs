﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsForms.FormControl;
using Helpers;
using FaceAnalysis;

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
            var fs = new FaceApiCalls(new HttpClientWrapper());
            var x = fs.CreateNewFaceset("feisetas").Result.Faceset_token;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormFaceDetection());
        }
    }
}
