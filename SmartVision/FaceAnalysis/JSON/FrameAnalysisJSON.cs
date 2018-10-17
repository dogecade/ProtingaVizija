using System;
using System.Collections.Generic;
using System.Drawing;

namespace FaceAnalysis
{
    public class FrameAnalysisJSON
    {
        public string image_id { get; set; }
        public string request_id { get; set; }
        public int time_used { get; set; }
        public IList<Face> faces { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            FrameAnalysisJSON objectToCompare = (FrameAnalysisJSON)obj;

            return (image_id.Equals(objectToCompare.image_id)) &&
                   (request_id.Equals(objectToCompare.request_id)) &&
                   (time_used == objectToCompare.time_used) &&
                   (faces.Count == objectToCompare.faces.Count);
        }
    }

    public class FaceRectangle
    {
        public int Width { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Height { get; set; }

        public static implicit operator Rectangle(FaceRectangle r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }
    }

    public struct Face
    {
        public FaceRectangle face_rectangle { get; set; }
        public string face_token { get; set; }
    }
}