using System;
using System.Collections.Generic;
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

            var target = new ActionBlock<T>(item =>
            {
                lock(itemLock)
                {
                    if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
                        ((IDisposable)lastItem)?.Dispose();
                    lastItem = item;
                }
                source.SendAsync(item);
            });

            target.Completion.ContinueWith(delegate
            {
                source.Complete();
            });

            return DataflowBlock.Encapsulate(target, source);
        }
    }
}
