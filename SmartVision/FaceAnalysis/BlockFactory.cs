using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace FaceAnalysis
{
    internal static class BlockFactory
    {
        public static IPropagatorBlock<T, T> CreateDisposingBroadcastBlock<T>(Func<T, T> cloningFunction)
        {
            T lastItem = default(T);
            object itemLock = new object();

            var source = new BroadcastBlock<T>(delegate
            {
                lock (itemLock)
                    return cloningFunction(lastItem);
            });

            var target = new ActionBlock<T>(async item =>
            {
                lock(itemLock)
                {
                    if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
                        ((IDisposable)lastItem)?.Dispose();
                    lastItem = item;
                }
                await source.SendAsync(item);
            });

            target.Completion.ContinueWith(delegate
            {
                source.Complete();
            });

            return DataflowBlock.Encapsulate(target, source);
        }

        public static IPropagatorBlock<SourceBitmapPair, SourceBitmapPair[]> CreateDistinctConditionalBatchBlock(Func<bool> batchCondition)
        {
            var dictionary = new Dictionary<ProcessableVideoSource, Bitmap>();

            var source = new BufferBlock<SourceBitmapPair[]>();

            var target = new ActionBlock<SourceBitmapPair>(async item =>
            {
                dictionary[item.Source] = item.Bitmap;
                if (batchCondition())
                {
                    await source.SendAsync(dictionary.Select(pair => new SourceBitmapPair(pair.Key, pair.Value)).ToArray());
                    dictionary.Clear();
                }
            });

            target.Completion.ContinueWith(async delegate
            {
                if (dictionary.Any())
                    await source.SendAsync(dictionary.Select(pair => new SourceBitmapPair(pair.Key, pair.Value)).ToArray());
                source.Complete();
            });

            return DataflowBlock.Encapsulate(target, source);
        }
    }
}
