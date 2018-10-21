using System.Collections.Generic;

namespace FaceAnalysis
{
    public struct DeleteFacesetJSON : IApiResponseJSON
    {
        public string faceset_token { get; set; }
        public string Request_id { get; set; }
        public int Time_used { get; set; }
        public string outer_id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DeleteFacesetJSON))
            {
                return false;
            }

            var jSON = (DeleteFacesetJSON)obj;
            return faceset_token == jSON.faceset_token &&
                   Request_id == jSON.Request_id &&
                   Time_used == jSON.Time_used &&
                   outer_id == jSON.outer_id;
        }

        public override int GetHashCode()
        {
            var hashCode = -1781013288;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(faceset_token);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Request_id);
            hashCode = hashCode * -1521134295 + Time_used.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(outer_id);
            return hashCode;
        }

        public static bool operator ==(DeleteFacesetJSON lhs, DeleteFacesetJSON rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(DeleteFacesetJSON lhs, DeleteFacesetJSON rhs)
        {
            return !(lhs == rhs);
        }
    }
}