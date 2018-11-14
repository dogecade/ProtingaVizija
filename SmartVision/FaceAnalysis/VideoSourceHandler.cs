using System;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using AForge.Video;

namespace FaceAnalysis
{
    public class VideoSourceHandler
    {
        private static FaceProcessor processor;
        public static void EstablishProcessor()
        {
            processor?.Complete();
            processor = new FaceProcessor(Sources);
        }
        public static List<ProcessableVideoSource> Sources { get; set; } = new List<ProcessableVideoSource>();
        public static void AddSource(ProcessableVideoSource source) => Sources.Add(source);
        public static void RemoveSource(ProcessableVideoSource source) => Sources.Remove(source);
    }
}
