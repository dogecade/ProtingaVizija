namespace FaceAnalysis
{
    public class DeleteFacesetJSON : IApiResponseJSON
    {
        public string faceset_token { get; set; }
        public string request_id { get; set; }
        public int time_used { get; set; }
        public string outer_id { get; set; }
    }
}