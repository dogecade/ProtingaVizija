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
        public static FaceProcessor Processor { get; } = new FaceProcessor();
        static void Main(string[] args)
        {
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public static async Task<(string url, string id)> AddStreamAsync(IVideoSource source)
        {
            return await AddStreamAsync(new ProcessableVideoSource(source));
        }

        public static async Task<(string url, string id)> AddStreamAsync(ProcessableVideoSource source)
        {
            var server = new MJPEGServer(source, start: true);
            streamServers[source] = server;
            await Processor.AddSourceAsync(source);
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
            Processor.RemoveSource(source);
        }

        public static IEnumerable<(string url, string id)> GetStreams()
        {
            return streamServers.Select(pair => (pair.Value.Url, pair.Key.Id.ToString()));
        }
    }
}
