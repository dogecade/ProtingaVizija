using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;
using Microsoft.Extensions.Hosting;

namespace AdminWeb
{
    public interface IFaceProcessingService : IHostedService, IDisposable
    {
        IEnumerable<(string url, string id)> GetStreams();
        void RemoveStream(string sourceId);
        Task<(string url, string id)> AddStreamAsync(IVideoSource source);
        FaceProcessor Processor { get; }

    }
}
