﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsForms
{
    class FaceDetection
    {
        private static CascadeClassifier cascade = new CascadeClassifier("..\\..\\XML\\haarcascade_frontalface_default.xml"); // Used for face detection
        private string x = System.Reflection.Assembly.GetExecutingAssembly().Location;
        /// <summary>
        /// Detects a face in a frame and draws a rectangle around it
        /// </summary>
        /// <param name="imageFrame">Frame to analyze</param>
        public static void FaceDetectionFromFrame(Image<Bgr, byte> imageFrame)
        {
            if (imageFrame != null)
            {
                var grayFrame = imageFrame.Convert<Gray, Byte>();

                var faces = cascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty); // The actual face detection happens here
                
                foreach (var face in faces)
                {
                    imageFrame.Draw(face, new Bgr(Color.Blue), 3); // The detected face(s) is(are) highlighted here using a box that is drawn around it/them
                }
            }
        }

        public static void DisableFaceDetection()
        {
            try
            {
                cascade.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
