using System;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using AForge.Video;

namespace FaceAnalysis
{
    public class VideoSourceHandler
    {
        public static List<ProcessableVideoSource> Sources { get; } = new List<ProcessableVideoSource>();
        public static void AddSource(ProcessableVideoSource source) => Sources.Add(source);
        public static void RemoveSource(ProcessableVideoSource source) => Sources.Remove(source);
    }
}
