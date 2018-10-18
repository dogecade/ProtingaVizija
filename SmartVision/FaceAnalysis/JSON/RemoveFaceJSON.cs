using System;
using System.Collections.Generic;

namespace FaceAnalysis
{
    public class RemoveFaceJSON
    {
        public string faceset_token { get; set; }
        public int face_removed { get; set; }
        public int time_used { get; set; }
        public int face_count { get; set; }
        public string request_id { get; set; }
        public string outer_id { get; set; }
        public IList<object> failure_detail { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            RemoveFaceJSON objectToCompare = (RemoveFaceJSON)obj;

            return (faceset_token.Equals(objectToCompare.faceset_token)) &&
                   (time_used == objectToCompare.time_used) &&
                   (face_count == objectToCompare.face_count) &&
                   (face_removed == objectToCompare.face_removed) &&
                   (request_id.Equals(objectToCompare.request_id)) &&
                   (outer_id.Equals(objectToCompare.outer_id)) &&
                   (failure_detail.Count == objectToCompare.failure_detail.Count);
        }
    }
}