using System;
using System.Collections.Generic;

namespace FaceAnalysis
{
    public struct FacesetDetailsJSON : IApiResponseJSON
    {
        public string Faceset_token { get; set; }
        public string Tags { get; set; }
        public int Time_used { get; set; }
        public string User_data { get; set; }
        public string Display_name { get; set; }
        public IList<string> Face_tokens { get; set; }
        public int Face_count { get; set; }
        public string Request_id { get; set; }
        public string Outer_id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is FacesetDetailsJSON))
            {
                return false;
            }

            var jSON = (FacesetDetailsJSON)obj;
            return Faceset_token == jSON.Faceset_token &&
                   Tags == jSON.Tags &&
                   Time_used == jSON.Time_used &&
                   User_data == jSON.User_data &&
                   Display_name == jSON.Display_name &&
                   EqualityComparer<IList<string>>.Default.Equals(Face_tokens, jSON.Face_tokens) &&
                   Face_count == jSON.Face_count &&
                   Request_id == jSON.Request_id &&
                   Outer_id == jSON.Outer_id;
        }

        public override int GetHashCode()
        {
            var hashCode = -327838629;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Faceset_token);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tags);
            hashCode = hashCode * -1521134295 + Time_used.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(User_data);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Display_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<string>>.Default.GetHashCode(Face_tokens);
            hashCode = hashCode * -1521134295 + Face_count.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Request_id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Outer_id);
            return hashCode;
        }

        public static bool operator ==(FacesetDetailsJSON lhs, FacesetDetailsJSON rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(FacesetDetailsJSON lhs, FacesetDetailsJSON rhs)
        {
            return !(lhs == rhs);
        }
    }
}
