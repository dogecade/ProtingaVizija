using System;
using System.Collections.Generic;

namespace WindowsForms.FaceAnalysis
{
    public struct FacesetDetailsJSON
    {
        public string faceset_token { get; set; }
        public string tags { get; set; }
        public int time_used { get; set; }
        public string user_data { get; set; }
        public string display_name { get; set; }
        public IList<string> face_tokens { get; set; }
        public int face_count { get; set; }
        public string request_id { get; set; }
        public string outer_id { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            FacesetDetailsJSON objectToCompare = (FacesetDetailsJSON) obj;

            return (faceset_token.Equals(objectToCompare.faceset_token)) &&
                   (tags.Equals(objectToCompare.tags)) &&
                   (user_data.Equals(objectToCompare.user_data)) &&
                   (display_name.Equals(objectToCompare.display_name)) &&
                   (outer_id.Equals(objectToCompare.outer_id)) &&
                   (request_id == objectToCompare.request_id) &&
                   (time_used == objectToCompare.time_used) &&
                   (face_count == objectToCompare.face_count) &&
                   (face_tokens.Count == objectToCompare.face_tokens.Count);
        }
    }
}
