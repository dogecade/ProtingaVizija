using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;

namespace StreamingBackend
{
    public static class MJPEGStreamManager
    {
        private static readonly ConcurrentDictionary<ProcessableVideoSource, MJPEGServer> streamServers = new ConcurrentDictionary<ProcessableVideoSource, MJPEGServer>();
        private static readonly FaceProcessor processor = new FaceProcessor();
        static void Main(string[] args)
        {
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        //TODO: this assumes that it's an MJPEG stream, it could be a JPEG stream as well
        public static string AddStream(string sourceUrl)
        {
            return AddStream(new ProcessableVideoSource(new MJPEGStream(sourceUrl)));
        }

        public static string AddStream(ProcessableVideoSource source)
        {
            var server = new MJPEGServer(source, start: true);
            streamServers[source] = server;
            processor.AddSource(source);
            return "http://localhost:" + server.Port.ToString();
        }
        
        public static void RemoveStream(string sourceUrl)
        {
            foreach(var source in streamServers.Keys)
                if (streamServers[source].Url.Split('/')[2] == sourceUrl.Split('/')[2])
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

        public static IEnumerable<string> GetStreamUrls()
        {
            return streamServers.Values.Select(server => "http://localhost:" + server.Port.ToString());
        }
    }
}
