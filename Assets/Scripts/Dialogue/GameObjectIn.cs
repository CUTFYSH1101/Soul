using UnityEngine;
using DP = Dialogue.DialogueProcessing;
using DR = Dialogue.DialogueRecorder;
using DE = Dialogue.DialogueElement;
using DC = Dialogue.DialogueCenter;
using DS = Dialogue.Dialogues;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable RedundantDefaultMemberInitializer

namespace Dialogue {
    public class GameObjectIn : MonoBehaviour {
        private DC _center;
        public GameObject canvas;
        public bool processingB = false;
        public bool isFirst = true;
        public bool processingC = false;
        public bool isOpen = false;
        private void Start () {
            _center = new DialogueCenter (canvas);
            DS.SetProcessing ();
            _center.GetData ().GetProcessing ().LinkProcessing (DS.ProcessingA);
        }
        private void Update () {
            if (isOpen) {
                NextSentences ();
            }
            //TODO: Can't restore.
            if (Input.GetKeyDown (KeyCode.K)) {
                _center.GetData ().GetRecorder ().Show ();
                #region Unused

                /*var s = _center.GetData ().GetRecorder ().GetStack ().Pop ();
                Debug.Log ($"{s.GetSpeaker ()}:({s.GetOption ()}){s.GetLore ()}");*/
                /*Can't Collect
                var t = _center.GetData ().GetRecorder ().GetStack ();
                Debug.Log ($"{t.Pop ().GetSpeaker ()}:({t.Pop ().GetOption ()}){t.Pop ().GetLore ()}");*/
                /*Stop at How are you
                var tmpS = new Stack<DE> (_center.GetData ().GetRecorder ().GetStack ());
                Debug.Log ($"{tmpS.Pop ().GetSpeaker ()}:({tmpS.Pop ().GetOption ()}){tmpS.Pop ().GetLore ()}");*/

                #endregion
            }
        }
        public void NextSentences () {
            if (isFirst) {
                _center.GetGui ().BindDataToGui ();
                isFirst = false;
            }
            if (Input.GetKeyDown (KeyCode.F)) {
                if (_center.GetData ().GetProcessing ().GetQueue ().Count == 0 && processingC) {
                    HideBox ();
                }
                if ( _center.GetData ().GetProcessing ().GetQueue ().Count == 0 && processingB) {
                    ToProcessingC ();
                    _center.GetGui ().BindDataToGui ();
                }
                _center.GetGui ().BindDataToGui ();
            }
        }
        public void ToProcessingB1 () {
            _center.GetData ().GetProcessing ().LinkProcessing (DS.ProcessingB1);
            _center.GetGui ().BindDataToGui ();
            processingB = true;
        }
        public void ToProcessingB2 () {
            _center.GetData ().GetProcessing ().LinkProcessing (DS.ProcessingB2);
            _center.GetGui ().BindDataToGui ();
            processingB = true;
        }
        public void ToProcessingC () {
            _center.GetData ().GetProcessing ().LinkProcessing (DS.ProcessingC);
            _center.GetGui ().BindDataToGui ();
            processingC = true;
        }

        public void CallBox () {
            _center.GetGui ().CallDiaBox ();
            isOpen = true;
        }
        public void HideBox () {
            _center.GetGui ().HideDiaBox ();
            isOpen = false;
        }
    }
}
