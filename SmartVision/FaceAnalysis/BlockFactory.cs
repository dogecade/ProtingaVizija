using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace FaceAnalysis
{
    internal static class BlockFactory
    {
        //TODO: make generic, output dictionary.
        public static IPropagatorBlock<Tuple<KeyType, ValueType>, IDictionary<KeyType, ValueType>> CreateConditionalDictionaryBlock<KeyType, ValueType>(Func<bool> batchCondition)
        {

            var dictionary = new ConcurrentDictionary<KeyType, ValueType>();

            var source = new BufferBlock<IDictionary<KeyType, ValueType>>();

            var target = new ActionBlock<Tuple<KeyType, ValueType>>(async item =>
            {
                if (dictionary.TryGetValue(item.Item1, out ValueType value) && value is IDisposable disposableValue)
                    disposableValue.Dispose();
                dictionary[item.Item1] = item.Item2;
                //TODO: would probably be nice to trigger earlier than when latest item arrives.
                if (batchCondition())
                {
                    var oldDictionary = dictionary;
                    dictionary = new ConcurrentDictionary<KeyType, ValueType>();
                    await source.SendAsync(oldDictionary);

                }
            });

            target.Completion.ContinueWith(async delegate
            {
                if (dictionary.Any())
                {
                    var oldDictionary = dictionary;
                    dictionary = new ConcurrentDictionary<KeyType, ValueType>();
                    await source.SendAsync(oldDictionary);
                }
                source.Complete();
            });

            return DataflowBlock.Encapsulate(target, source);
        }
    }
}
