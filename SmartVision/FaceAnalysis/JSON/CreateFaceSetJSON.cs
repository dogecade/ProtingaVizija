using System;
using System.Collections.Generic;

namespace FaceAnalysis
{
    public class CreateFaceSetJSON : IApiResponseJSON
    {
        public string faceset_token { get; set; }
        public int time_used { get; set; }
        public int face_count { get; set; }
        public int face_added { get; set; }
        public string request_id { get; set; }
        public string outer_id { get; set; }
        public IList<object> failure_detail { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            CreateFaceSetJSON objectToCompare = (CreateFaceSetJSON) obj;

            return (faceset_token.Equals(objectToCompare.faceset_token)) &&
                   (time_used == objectToCompare.time_used) &&
                   (face_count == objectToCompare.face_count) &&
                   (face_added == objectToCompare.face_added) &&
                   (request_id == objectToCompare.request_id) &&
                   (outer_id == objectToCompare.outer_id) &&
                   (failure_detail.Count == objectToCompare.failure_detail.Count);
        }
    }
}
