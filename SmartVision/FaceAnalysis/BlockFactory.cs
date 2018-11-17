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
        public static IPropagatorBlock<Tuple<KeyType, ValueType>, Dictionary<KeyType, ValueType>> CreateConditionalDictionaryBlock<KeyType, ValueType>(Func<bool> batchCondition)
        {
            var dictLock = new object();

            var dictionary = new Dictionary<KeyType, ValueType>();

            var source = new BufferBlock<Dictionary<KeyType, ValueType>>();

            var target = new ActionBlock<Tuple<KeyType, ValueType>>(async item =>
            {
                lock(dictLock)
                    if (dictionary.TryGetValue(item.Item1, out ValueType value) && value is IDisposable disposableValue)
                        disposableValue.Dispose();
                dictionary[item.Item1] = item.Item2;
                if (batchCondition())
                {
                    var oldDictionary = dictionary;
                    lock (dictLock)
                        dictionary = new Dictionary<KeyType, ValueType>();
                    await source.SendAsync(oldDictionary);

                }
            });

            target.Completion.ContinueWith(async delegate
            {
                if (dictionary.Any())
                {
                    var oldDictionary = dictionary;
                    lock (dictLock)
                        dictionary = new Dictionary<KeyType, ValueType>();
                    await source.SendAsync(oldDictionary);
                }
                source.Complete();
            });

            return DataflowBlock.Encapsulate(target, source);
        }
    }
}
