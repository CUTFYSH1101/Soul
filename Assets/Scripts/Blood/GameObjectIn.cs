using UnityEngine;
using BE = Blood.BloodElements;
using DB = UnityEngine.Debug;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Blood {
    public class GameObjectIn : MonoBehaviour{
        public int randomQueueCount;
        public Vector2 specificQueuePos = new Vector2 (0,0);
        private BloodCenter _center;
        private void Start () {
            _center = new BloodCenter ();
            _center.GetData ().GetHandler ().GenerateRandomQueue (randomQueueCount);
            _center.GetGui ().GenerateBar (specificQueuePos).InsertDisplay ();
        }
        private void Update () {
            if (_center.GetData ().DetectKeyToDequeue ()) {
                _center.GetGui ().CancelDisplay ().GenerateBar (specificQueuePos).InsertDisplay ();
            }
        }
    }
}