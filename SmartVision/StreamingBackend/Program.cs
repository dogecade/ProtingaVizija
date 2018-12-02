using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;
using Objects.CameraProperties;

namespace StreamingBackend
{
    public static class MJPEGStreamManager
    {
        private static readonly ConcurrentDictionary<ProcessableVideoSource, MJPEGServer> streamServers = new ConcurrentDictionary<ProcessableVideoSource, MJPEGServer>();
        private static FaceProcessor processor;
        static void Main(string[] args)
        {
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public static void Test() => Debug.WriteLine("opapapa");

        //TODO: this assumes that it's an MJPEG stream, it could be a JPEG stream as well
        public static async Task<(string url, string id)> AddStream(string sourceUrl, CameraProperties properties = null)
        {
            return await AddStream(new ProcessableVideoSource(new MJPEGStream(sourceUrl)), properties);
        }

        //TODO: this is hacky as hell, should be fixed with proper CameraProperties things
        public static async Task<(string url, string id)> AddStream(ProcessableVideoSource source, CameraProperties properties = null)
        {
            bool processorNull = processor == null;
            processor = processor == null ? new FaceProcessor(properties) : processor;
            var server = new MJPEGServer(source, start: true);
            streamServers[source] = server;
            processor.AddSource(source);
            if (processorNull)
                await processor.Start();
            return (server.Url, source.Id.ToString());
            
        }
        
        public static void RemoveStream(string sourceId)
        {
            foreach(var source in streamServers.Keys)
                if (source.Id.ToString() == sourceId)
                {
                    RemoveStream(source);
                    return;
                }

        }

        public static void RemoveStream(ProcessableVideoSource source)
        {
            streamServers.TryRemove(source, out var server);
            server.Stop();
            processor.RemoveSource(source);
        }

        public static IEnumerable<(string url, string id)> GetStreams()
        {
            return streamServers.Select(pair => (pair.Value.Url, pair.Key.Id.ToString()));
        }
    }
}
