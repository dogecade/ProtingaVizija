using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FaceAnalysis
{

    /// <summary>
    /// Block that stores key/value pairs in a dictionary until batch is triggered.
    /// </summary>
    /// <typeparam name="KeyType">Type of the keys</typeparam>
    /// <typeparam name="ValueType">Type of the values</typeparam>
    public class BatchDictionaryBlock<KeyType, ValueType> : IPropagatorBlock<(KeyType, ValueType), IDictionary<KeyType, ValueType>>,
                                                            IReceivableSourceBlock<IDictionary<KeyType, ValueType>>
    {
        private ConcurrentDictionary<KeyType, ValueType> dictionary = new ConcurrentDictionary<KeyType, ValueType>();

        private IReceivableSourceBlock<IDictionary<KeyType, ValueType>> source;

        private ITargetBlock<(KeyType, ValueType)> target;

        private TaskCompletionSource<bool> signal = null;

        private readonly object signalLock = new object();

        public BatchDictionaryBlock()
        {
            source = new BufferBlock<IDictionary<KeyType, ValueType>>();

            target = new ActionBlock<(KeyType, ValueType)>(item =>
            {
                if (dictionary.TryGetValue(item.Item1, out ValueType value) && value is IDisposable disposableValue)
                    disposableValue.Dispose();
                dictionary[item.Item1] = item.Item2;
                lock (signalLock)
                    if (signal != null && !signal.Task.IsCompleted)
                        signal?.SetResult(true);
            });
            target.Completion.ContinueWith(async delegate
            {
                if (dictionary.Any())
                {
                    await TriggerBatch();
                }
                source.Complete();
            });
        }

        /// <summary>
        /// Awaits until block has value, then triggers batch, pushes dictionary
        /// </summary>
        public async Task TriggerBatch()
        {
            await HasValueAsync();
            var oldDictionary = dictionary;
            dictionary = new ConcurrentDictionary<KeyType, ValueType>();
            if (source is BufferBlock<IDictionary<KeyType, ValueType>>)
                ((BufferBlock<IDictionary<KeyType, ValueType>>)source).Post(oldDictionary);         
        }

        /// <summary>
        /// Await until block has values
        /// </summary>
        /// <returns></returns>
        public async Task HasValueAsync()
        {
            if (dictionary.Count > 0)
                return;
            lock(signalLock)
                signal = new TaskCompletionSource<bool>();
            await signal.Task;
        }

        /// <summary>
        /// Attempts to synchronously receive an item from the source.
        /// </summary>
        /// <param name="filter">Item filter</param>
        /// <param name="item">Received item, if any</param>
        /// <returns></returns>
        public bool TryReceive(Predicate<IDictionary<KeyType, ValueType>> filter, out IDictionary<KeyType, ValueType> item)
        {
            return source.TryReceive(filter, out item);
        }

        /// <summary>
        /// Attempts to remove all available elements from the source into a new 
        /// array that is returned.
        /// </summary>
        /// <param name="items">Array of items, if any</param>
        /// <returns></returns>
        public bool TryReceiveAll(out IList<IDictionary<KeyType, ValueType>> items)
        {
            return source.TryReceiveAll(out items);
        }

        /// <summary>
        /// Links this dataflow block to the provided target.
        /// </summary>
        /// <param name="target">Target block</param>
        /// <param name="linkOptions">Link options</param>
        /// <returns></returns>
        public IDisposable LinkTo(ITargetBlock<IDictionary<KeyType, ValueType>> target, DataflowLinkOptions linkOptions)
        {
            return source.LinkTo(target, linkOptions);
        }

        /// <summary>
        /// Called by a target to reserve a message previously offered by a source 
        /// but not yet consumed by this target.
        /// </summary>
        /// <param name="messageHeader">Header of the message to reserve</param>
        /// <param name="target">Target block</param>
        /// <returns></returns>
        bool ISourceBlock<IDictionary<KeyType, ValueType>>.ReserveMessage(DataflowMessageHeader messageHeader,
           ITargetBlock<IDictionary<KeyType, ValueType>> target)
        {
            return source.ReserveMessage(messageHeader, target);
        }

        /// <summary>
        /// Called by a target to consume a previously offered message from a source.
        /// </summary>
        /// <param name="messageHeader">Header of the message to consume</param>
        /// <param name="target">Target block</param>
        /// <param name="messageConsumed">Whether the message was consumed or not</param>
        /// <returns></returns>
        IDictionary<KeyType, ValueType> ISourceBlock<IDictionary<KeyType, ValueType>>.ConsumeMessage(DataflowMessageHeader messageHeader,
           ITargetBlock<IDictionary<KeyType, ValueType>> target, out bool messageConsumed)
        {
            return source.ConsumeMessage(messageHeader,
               target, out messageConsumed);
        }

        /// <summary>
        /// Called by a target to release a previously reserved message from a source.
        /// </summary>
        /// <param name="messageHeader">Header of the message to release</param>
        /// <param name="target">Target block</param>
        void ISourceBlock<IDictionary<KeyType, ValueType>>.ReleaseReservation(DataflowMessageHeader messageHeader,
           ITargetBlock<IDictionary<KeyType, ValueType>> target)
        {
            source.ReleaseReservation(messageHeader, target);
        }

        /// <summary>
        /// Asynchronously passes a message to the target block, giving the target the 
        /// opportunity to consume the message.
        /// </summary>
        /// <param name="messageHeader">Header of message being passed</param>
        /// <param name="messageValue">Message value</param>
        /// <param name="source">Source value</param>
        /// <param name="consumeToAccept"></param>
        /// <returns></returns>
        DataflowMessageStatus ITargetBlock<(KeyType, ValueType)>.OfferMessage(DataflowMessageHeader messageHeader,
           (KeyType, ValueType) messageValue, ISourceBlock<(KeyType, ValueType)> source, bool consumeToAccept)
        {
            return target.OfferMessage
            (
                messageHeader,
                messageValue,
                source,
                consumeToAccept
            );
        }

        /// <summary>
        /// Gets a Task that represents the completion of this dataflow block.
        /// </summary>
        public Task Completion { get { return source.Completion; } }

        // Signals to this target block that it should not accept any more messages, 
        // nor consume postponed messages. 
        public void Complete()
        {
            target.Complete();
        }

        public void Fault(Exception e)
        {
            target.Fault(e);
        }

    }
}
