using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DB = UnityEngine.Debug;
using BE = Blood.BloodElement;
using BEs = Blood.BloodElements;

namespace Blood {
    public class BloodHandler {
        //For Queue
        private readonly Queue<BE> _queueElement = new Queue<BE> ();
        public Queue<BE> GetQueueElement () => _queueElement;
        // ReSharper disable MemberCanBePrivate.Global
        public BloodHandler AddElementToQueue (params BE[] elementIn) {
            // ReSharper restore MemberCanBePrivate.Global
            //Let Array to List that use ForEach Function, Action Delegate and Lambda to Add Multiple Elements.
            elementIn.ToList ().ForEach (elements => _queueElement.Enqueue (elements));
            return this;
        }
        public BloodHandler GenerateRandomQueue (int countIn) {
            for (var c = 1; c <= countIn; c++) {
                Thread.Sleep (500);
                AddElementToQueue (ArrayPick<BE>.GetRandomFromArray (BEs.GetAllElement ()));
            }
            _logQueueList (_queueElement);
            return this;
        }
        public BloodHandler GenerateSpecificQueue (params BE[] elementIn) {
            AddElementToQueue (elementIn);
            _logQueueList (_queueElement);
            return this;
        }
        private void _logQueueList (Queue<BE> q) {
            var tmp = new Queue<BE> (q);
            var tmpCount = tmp.Count;
            string result = "BloodHandler: \n";
            for (var c = 1; c <= tmpCount; c++) {
                if (c == tmpCount) result += $"{c} : {tmp.Dequeue ().GetElementName ()} ==> Fin";
                else  result += $"{c} : {tmp.Dequeue ().GetElementName ()} ==> ";
            }
            if (tmpCount == 0) result = "Fin!";
            DB.Log (result);    
        }
        public void ShowQueueLog () {
            _logQueueList (_queueElement);
        }
    }
}
