using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using AForge.Video;

namespace FaceAnalysis
{
    public class ProcessableVideoSource
    {
        private ConcurrentQueue<Rectangle> faceRectangles;
        public event NewFrameEventHandler NewFrame;
        public IVideoSource Stream { get; }
        public Guid Id { get; }

        public ProcessableVideoSource(IVideoSource videoStream)
        {
            Stream = videoStream;
            Id = Guid.NewGuid();
        }
         
        public void Start()
        {
            Stream.Start();
            Stream.NewFrame += UpdateFrame;
        }

        public void Stop()
        {
            Stream.Stop();
            Stream.NewFrame -= UpdateFrame;
        }

        public void UpdateFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bitmap;
            lock (sender)
                bitmap = new Bitmap(e.Frame);
            bitmap = HelperMethods.ProcessImage(bitmap);
            if (faceRectangles != null)
                using (Graphics g = Graphics.FromImage(bitmap))
                using (Pen pen = new Pen(new SolidBrush(Color.Red), 1))
                    foreach (Rectangle face in faceRectangles)
                        g.DrawRectangle(pen, face);

            NewFrame?.Invoke(this, new NewFrameEventArgs(bitmap));
            bitmap.Dispose();
        }

        public void UpdateRectangles(object sender, FrameProcessedEventArgs e)
        {
            if(e.RectangleDictionary.ContainsKey(this))
                faceRectangles = new ConcurrentQueue<Rectangle>(e.RectangleDictionary[this]);
        }

        public override bool Equals(object obj)
        {
            return obj is ProcessableVideoSource && ((ProcessableVideoSource)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(ProcessableVideoSource lhs, ProcessableVideoSource rhs)
        {
            if (lhs is null)
                return rhs is null;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ProcessableVideoSource lhs, ProcessableVideoSource rhs)
        {
            return lhs == rhs;
        }

    }
}
