using System;
using System.Collections.Generic;

namespace FaceAnalysis
{
    public class CreateFacesetJSON : IApiResponseJSON
    {
        public string Faceset_token { get; set; }
        public int Time_used { get; set; }
        public int Face_count { get; set; }
        public int Face_added { get; set; }
        public string Request_id { get; set; }
        public string Outer_id { get; set; }
        public IList<object> Failure_detail { get; set; }

        public override bool Equals(object obj)
        {
            var jSON = obj as CreateFacesetJSON;
            return jSON != null &&
                   Faceset_token == jSON.Faceset_token &&
                   Time_used == jSON.Time_used &&
                   Face_count == jSON.Face_count &&
                   Face_added == jSON.Face_added &&
                   Request_id == jSON.Request_id &&
                   Outer_id == jSON.Outer_id &&
                   EqualityComparer<IList<object>>.Default.Equals(Failure_detail, jSON.Failure_detail);
        }

        public override int GetHashCode()
        {
            var hashCode = 1756639260;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Faceset_token);
            hashCode = hashCode * -1521134295 + Time_used.GetHashCode();
            hashCode = hashCode * -1521134295 + Face_count.GetHashCode();
            hashCode = hashCode * -1521134295 + Face_added.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Request_id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Outer_id);
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<object>>.Default.GetHashCode(Failure_detail);
            return hashCode;
        }

        public static bool operator ==(CreateFacesetJSON lhs, CreateFacesetJSON rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CreateFacesetJSON lhs, CreateFacesetJSON rhs)
        {
            return !(lhs == rhs);
        }

    }
}
