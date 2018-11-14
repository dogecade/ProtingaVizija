using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace FaceAnalysis
{
    internal static class BlockFactory
    {
        //TODO: make generic, output dictionary.
        public static IPropagatorBlock<SourceBitmapPair, SourceBitmapPair[]> CreateDistinctConditionalBatchBlock(Func<bool> batchCondition)
        {
            var dictLock = new object();

            var dictionary = new Dictionary<ProcessableVideoSource, Bitmap>();

            var source = new BufferBlock<SourceBitmapPair[]>();

            var target = new ActionBlock<SourceBitmapPair>(async item =>
            {
                lock(dictLock)
                    if (dictionary.ContainsKey(item.Source))
                        dictionary[item.Source].Dispose();
                dictionary[item.Source] = item.Bitmap;
                if (batchCondition())
                {
                    SourceBitmapPair[] array;
                    lock(dictLock)
                    {
                        array = dictionary.Select(pair => new SourceBitmapPair(pair.Key, HelperMethods.ProcessImage(pair.Value))).ToArray();
                        dictionary.Clear();
                    }    
                    await source.SendAsync(array);

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
