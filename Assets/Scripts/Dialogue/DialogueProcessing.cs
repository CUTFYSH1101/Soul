using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DE = Dialogue.DialogueElement;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Dialogue {
    public class DialogueProcessing {
        private readonly Queue<DE> _queue = new Queue<DialogueElement> ();
        public Queue<DE> GetQueue () => _queue;
        public DialogueProcessing AddElementToQueue (params DE[] processingIn) {
            processingIn.ToList ().ForEach (e => _queue.Enqueue (e));
            return this;
        }
        public DialogueElement NextElement () {
            if (_queue.Count == 0) return _queue.Peek ();
            Debug.Log ($"{_queue.Peek ().GetSpeaker ()} : ({_queue.Peek ().GetOption ()}) {_queue.Peek ().GetLore ()}");
            return _queue.Dequeue ();
        }
        public DialogueProcessing LinkProcessing (DialogueProcessing newDp) {
            if (_queue.Count != 0) return this;
            Debug.Log ($"Link Processing!");
            newDp._queue.ToList ().ForEach (e => AddElementToQueue (e));
            return this;
        }
        public DialogueProcessing ClearQueue () {
            Debug.Log ("Clean Queue");
            do {
                _queue.Dequeue ();
            } while (_queue.Count > 0);
            _queue.Clear ();
            return this;
        }
    }
}
