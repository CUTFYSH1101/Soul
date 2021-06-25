using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using DP = Dialogue.DialogueProcessing;
using DR = Dialogue.DialogueRecorder;
using DE = Dialogue.DialogueElement;
using DS = Dialogue.Dialogues;
// ReSharper disable RedundantAssignment
// ReSharper disable NotAccessedField.Local
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Dialogue {
    public class DialogueCenter {
        private readonly Data _dataCenter;
        private readonly GUI _guiCenter;
        public DialogueCenter (GameObject canvas) {
            _guiCenter = new GUI (this, canvas);
            _dataCenter = new Data (this);
        }
        public Data GetData () => _dataCenter;
        public GUI GetGui () => _guiCenter;

        public class Data {
            private readonly DP _processing;
            private readonly DR _recorder;
            [UsedImplicitly] private DialogueCenter _center;
            public Data (DialogueCenter dc) {
                _center = dc;
                _processing = new DP ();
                _recorder = new DR ();
            }
            public DP GetProcessing () => _processing;
            public DR GetRecorder () => _recorder;
        }

        public class GUI {
            private DialogueCenter _center;
            private GameObject _canvasNode;
            public GUI (DialogueCenter dc, GameObject canvas) {
                _canvasNode = canvas;
                _center = dc;
                _npcBtn = canvas.transform.GetChild (0).gameObject;
                _diaBox = canvas.transform.GetChild (1).gameObject;
                _nameText = _diaBox.transform.GetChild (0).GetChild (0).gameObject;
                _loreText = _diaBox.transform.GetChild (0).GetChild (1).gameObject;
                _bifurGroup = _diaBox.transform.GetChild (0).GetChild (2).gameObject;
                _nextBtn = _diaBox.transform.GetChild (0).GetChild (3).gameObject;
                _closeBtn = _diaBox.transform.GetChild (1).gameObject;
            }
            private GameObject _npcBtn, _diaBox, _nameText, _loreText, _bifurGroup, _nextBtn, _closeBtn;
            public void BindDataToGui () {
                if (_center._dataCenter.GetProcessing ().GetQueue ().Count <= 0 ) {
                    _center._dataCenter.GetProcessing ().GetQueue ().Clear ();
                    return;
                }
                var data = _center._dataCenter.GetRecorder ().RecordElement (_center._dataCenter.GetProcessing ().NextElement ());
                var tmpData = new Queue<DE> ();
                _nameText.GetComponent<Text> ().text = data.GetSpeaker ();
                _bifurGroup.SetActive (false);
                _loreText.SetActive (true);
                if (data.GetOption ()) {
                    _bifurGroup.SetActive (true);
                    _loreText.SetActive (false);
                    tmpData.Enqueue (data);
                }
                for (var c = 0; data.GetOption (); c++) {
                    _bifurGroup.transform.GetChild (c).gameObject.SetActive (true);
                    _bifurGroup.transform.GetChild (c).gameObject.GetComponent<Text> ().text = tmpData.Dequeue ().GetLore ();
                    if (_center._dataCenter.GetProcessing ().GetQueue ().Count == 0) break;
                    tmpData.Enqueue(_center._dataCenter.GetRecorder ().RecordElement (_center._dataCenter.GetProcessing ().NextElement ()));
                }
                _loreText.GetComponent<Text> ().text = data.GetLore ();
            }
            public bool CallDiaBox () {
                _diaBox.SetActive (true);
                return true;
            }
            public bool HideDiaBox () {
                _diaBox.SetActive (false);
                return false;
            }
        }
    }
}
