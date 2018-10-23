using System;
using System.Collections.Generic;
using System.Drawing;

namespace FaceAnalysis
{
    public class FrameAnalysisJSON : IApiResponseJSON
    {
        public string Image_id { get; set; }
        public string Request_id { get; set; }
        public int Time_used { get; set; }
        public IList<Face> Faces { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is FrameAnalysisJSON))
            {
                return false;
            }

            var jSON = (FrameAnalysisJSON)obj;
            return Image_id == jSON.Image_id &&
                   Request_id == jSON.Request_id &&
                   Time_used == jSON.Time_used &&
                   EqualityComparer<IList<Face>>.Default.Equals(Faces, jSON.Faces);
        }

        public override int GetHashCode()
        {
            var hashCode = -561735587;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Image_id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Request_id);
            hashCode = hashCode * -1521134295 + Time_used.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<Face>>.Default.GetHashCode(Faces);
            return hashCode;
        }

        public static bool operator ==(FrameAnalysisJSON lhs, FrameAnalysisJSON rhs)
        {
            if (lhs is null)
                return rhs is null;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(FrameAnalysisJSON lhs, FrameAnalysisJSON rhs)
        {
            return !(lhs == rhs);
        }
    }

    public struct FaceRectangle
    {
        public int Width { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Height { get; set; }

        public static implicit operator Rectangle(FaceRectangle r) => new Rectangle(r.Left, r.Top, r.Width, r.Height);
    }

    public struct Face
    {
        public FaceRectangle Face_rectangle { get; set; }
        public string Face_token { get; set; }
    }
}