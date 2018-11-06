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

        public ProcessableVideoSource(IVideoSource videoStream)
        {
            Stream = videoStream;
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
            if (faceRectangles != null)
                using (Graphics g = Graphics.FromImage(bitmap))
                using (Pen pen = new Pen(new SolidBrush(Color.Red), 1))
                    foreach (Rectangle face in faceRectangles)
                        g.DrawRectangle(pen, face);

            NewFrame?.Invoke(this, new NewFrameEventArgs(bitmap));
        }

        public void UpdateRectangles(object sender, FrameProcessedEventArgs e)
        {
            faceRectangles = new ConcurrentQueue<Rectangle>(e.FaceRectangles) ?? faceRectangles;
        }
    }
}
