using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DE = Dialogue.DialogueElement;
namespace Dialogue {
    public class DialogueRecorder {
        private readonly Stack<DE> _stack = new Stack<DE> ();
        public Stack<DE> GetStack () => _stack;
        // ReSharper disable Unity.PerformanceAnalysis
        public DE RecordElement (DE throwIn) {
            _stack.Push (throwIn);
            return throwIn;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public DE LastElement () {
            var tmpStack = new Stack<DE> (_stack);
            Debug.Log ($"{tmpStack.Peek ().GetSpeaker ()} : ({tmpStack.Peek ().GetOption ()}) {tmpStack.Peek ().GetLore ()}");
            return tmpStack.Pop ();
        }
        public DialogueRecorder ClearStack () {
            _stack.Clear ();
            return this;
        }
        public void Show () {
            var tmpS = new Stack<DE> (_stack);
            var s = "";
            tmpS.ToList ().ForEach (e => s+=e.GetSpeaker ()+" : ("+e.GetOption ()+") "+e.GetLore ()+"\n");
            Debug.Log (s);
        }
    }
}
