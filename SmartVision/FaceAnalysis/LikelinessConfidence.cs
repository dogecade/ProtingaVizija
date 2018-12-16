namespace FaceAnalysis
{
    public enum LikelinessConfidence
    {
        LowProbability = 0,
        NormalProbability = 1, //confidence threshold at the 0.1% error rate
        HighProbability = 2, //confidence threshold at the 0.01% error rate
        VeryHighProbability = 3 //confidence threshold at the 0.001% error rate
    }

    public static class LikelinessLevelExtensions
    {
        public static string ToPrettyString(this LikelinessConfidence confidence)
        {
            switch (confidence)
            {
                case LikelinessConfidence.LowProbability:
                    return "low";
                case LikelinessConfidence.NormalProbability:
                    return "normal";
                case LikelinessConfidence.HighProbability:
                    return "high";
                case LikelinessConfidence.VeryHighProbability:
                    return "very high";
                default:
                    return confidence.ToString();
            }
        }
    }

}