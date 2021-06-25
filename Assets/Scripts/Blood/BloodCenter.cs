using System;
using System.Collections.Generic;
using UnityEngine;
using BE = Blood.BloodElement;
using BEs = Blood.BloodElements;
using DB = UnityEngine.Debug;
using Object = UnityEngine.Object;
// ReSharper disable Unity.InefficientPropertyAccess
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Blood {
    public class BloodCenter {
        private readonly Data _centerData;
        private readonly GUI _centerGui;
        public BloodCenter () {
            _centerData = new Data (this);
            _centerGui = new GUI (this);
        }
        //Link different area, just like a mediator.
        public Data GetData () => _centerData;
        public GUI GetGui () => _centerGui;
        public class Data {
            public Data (BloodCenter bc) {
                _center = bc;
            }
            // ReSharper disable once NotAccessedField.Local
            private BloodCenter _center;
            //Get Handler Data
            private readonly BloodHandler _bloodBarData = new BloodHandler ();
            public BloodHandler GetHandler () => _bloodBarData;
            // ReSharper disable Unity.PerformanceAnalysis
            /// <summary>
            ///   <para>偵測是否有對應按鍵的輸入事件，並判斷是否有配對到相應的元素。</para>
            /// </summary>
            public bool DetectKeyToDequeue () {
                if (_bloodBarData.GetQueueElement ().Count == 0) return false;
                if (Input.GetKeyDown (KeyCode.J) && ( _bloodBarData.GetQueueElement ().Peek ().Equals (BEs.GetSquare ()) || _bloodBarData.GetQueueElement ().Peek ().Equals (BEs.GetSquareEx ()) )) {
                    _bloodBarData.GetQueueElement ().Dequeue ();
                    DB.Log ("BloodCenter.Data: \nDequeue Square(EX)");
                    _bloodBarData.ShowQueueLog ();
                    return true;
                }
                if (Input.GetKeyDown (KeyCode.K) && ( _bloodBarData.GetQueueElement ().Peek ().Equals (BEs.GetCircle ()) || _bloodBarData.GetQueueElement ().Peek ().Equals (BEs.GetCircleEx ()) )) {
                    _bloodBarData.GetQueueElement ().Dequeue ();
                    DB.Log ("BloodCenter.Data: \nDequeue Circle(EX)");
                    _bloodBarData.ShowQueueLog ();
                    return true;
                }
                if (Input.GetKeyDown (KeyCode.L) && ( _bloodBarData.GetQueueElement ().Peek ().Equals (BEs.GetCross ()) || _bloodBarData.GetQueueElement ().Peek ().Equals (BEs.GetCrossEx ()) )) {
                    _bloodBarData.GetQueueElement ().Dequeue ();
                    DB.Log ("BloodCenter.Data: \nDequeue Cross(EX)");
                    _bloodBarData.ShowQueueLog ();
                    return true;
                }
                return false;
            }
        }
        public class GUI {
            public GUI (BloodCenter bc) {
                _center = bc;
                _elementsG = new[] {
                    _canvasNode.GetChild (0).GetChild (BEs.GetCircle ().GetElementId ()).gameObject,
                    _canvasNode.GetChild (0).GetChild (BEs.GetSquare ().GetElementId ()).gameObject,
                    _canvasNode.GetChild (0).GetChild (BEs.GetCross ().GetElementId ()).gameObject,
                    _canvasNode.GetChild (0).GetChild (BEs.GetCircleEx ().GetElementId ()).gameObject,
                    _canvasNode.GetChild (0).GetChild (BEs.GetSquareEx ().GetElementId ()).gameObject,
                    _canvasNode.GetChild (0).GetChild (BEs.GetCrossEx ().GetElementId ()).gameObject,
                };
                _bar = _canvasNode.GetChild (1).gameObject;
                _barRect = _bar.GetComponent<RectTransform> ();
            }
            private readonly BloodCenter _center;

            private readonly Transform _canvasNode = GameObject.Find ("Canvas").transform;
            private GameObject[] _elementsG;
            private GameObject _bar;

            private RectTransform _barRect;
            private float _elementSize = 25F;
            private int _maxDisplay = 20;
            private float _barSize;

            public GUI GenerateBar (Vector2 positionIn = default) {
                if (_center._centerData.GetHandler ().GetQueueElement ().Count > 20 ) _barSize = _maxDisplay * _elementSize;
                else _barSize = _center._centerData.GetHandler ().GetQueueElement ().Count * _elementSize;
                _barRect.sizeDelta = new Vector2 (_barSize, _elementSize);
                var originalPos = new Vector2 (_barSize / 2F, -_elementSize / 2F);
                _barRect.anchoredPosition = new Vector2 (originalPos.x + positionIn.x, originalPos.y - positionIn.y);
                DB.Log ($"BloodCenter.Gui: \nBar pos : ({_barRect.anchoredPosition.x},{_barRect.anchoredPosition.y}) ||| Bar size : ({_barRect.sizeDelta.x},{_barRect.sizeDelta.y}) \n" +
                    $"There are {_center._centerData.GetHandler ().GetQueueElement ().Count} elements @ bar");
                return this;
            }
            public GUI InsertDisplay () {
                var tmpQ = new Queue<BloodElement> (_center._centerData.GetHandler ().GetQueueElement ());
                var tmpQc = _center._centerData.GetHandler ().GetQueueElement ().Count;
                var posList = PosArithmetic (IsOdd ());
                for (var c = 0; c < tmpQc; c++) {
                    Object.Instantiate (_elementsG[tmpQ.Dequeue ().GetElementId ()], _bar.transform).GetComponent<RectTransform> ().anchoredPosition = posList[c];
                }
                return this;
            }
            public GUI CancelDisplay () {
                for(int c = 0; c <= _center._centerData.GetHandler ().GetQueueElement ().Count; c++ ) {
                    Object.Destroy (_bar.transform.GetChild (c).gameObject);
                    //? : Count need add 1 to destroy completely.
                }
                return this;
            }
            private bool IsOdd () => _center._centerData.GetHandler ().GetQueueElement ().Count % 2 == 1;
            private List<Vector2> PosArithmetic (bool isOdd) {
                float middleValue;
                int halfCount = _center._centerData.GetHandler ().GetQueueElement ().Count / 2;
                float deltaValue = _elementSize;
                float startValue;
                if (isOdd) {
                    middleValue = 0F;
                    startValue = middleValue - Convert.ToSingle (halfCount) * deltaValue;
                }
                else {
                    middleValue = -12.5F;
                    startValue = middleValue - ( Convert.ToSingle (halfCount) - 1 ) * deltaValue;
                }
                List<Vector2> vectors = new List<Vector2> ();
                for (var c = 1; c <= _center._centerData.GetHandler ().GetQueueElement ().Count; c++) {
                    vectors.Add (new Vector2 (startValue + ( c - 1 ) * deltaValue,0F));
                }
                return vectors;
            }
        }
    }
}