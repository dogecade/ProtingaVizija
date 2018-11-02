namespace FaceAnalysis
{
    public enum LikelinessConfidence
    {
        LowProbability = 0,
        NormalProbability = 1, //confidence threshold at the 0.1% error rate
        HighProbability = 2, //confidence threshold at the 0.01% error rate
        VeryHighProbability = 3 //confidence threshold at the 0.001% error rate
    }
}
