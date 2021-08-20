using Main.EventSystem.Cause;
using Main.EventSystem.Event.Attribute;
using Main.Game;
using TMPro;
using UnityEngine;

namespace Main.EventSystem.Event.UIEvent.Combo
{
    public class ComboUIEvent : UIEvent
    {
        private static Transform _compUI;
        private static UIText _compText;
        private static int _count = 1; // 計數器
        private static CdCause _timer;

        public ComboUIEvent(string hierarchyPath) :
            base(new EventAttr(0, 999))
        {
            if (_compUI != null)
                return;
            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compText = new UIText(_compUI == null ? null : _compUI.GetFirstComponentInChildren<TMP_Text>("Text"));
            CauseEnter = new FuncCause(() => !IsEmpty());
            CauseExit = new FuncCause(() => _timer.OrCause());
            _timer = new CdCause(2);
            _compUI.gameObject.SetActive(false);
        }

        private bool IsEmpty() =>
            _compText == null;


        private void SetCombo(float count)
        {
            _compText.Text = count.ToString();
            _compText.FontSize = count > 10 ? MAXSize : OriginSize;
            _compText.Alpha = 1;
        }

        // [40,64,80] [5,8,10]
        private const float OriginSize = 64;
        private const float MAXSize = OriginSize * 1.25f;
        private const float MINSize = OriginSize * 0.625f;

        private void FadeOut()
        {
            if (_compText.FontSize > MINSize)
            {
                _compText.FontSize -= Main.Util.Time.DeltaTime;
                _compText.Alpha -= Main.Util.Time.DeltaTime;
            }
        }

        private class UIText
        {
            private readonly TMP_Text _compText;

            public float Alpha
            {
                get => _compText.color.a;
                set
                {
                    var color = _compText.color;
                    color.a = value;
                    _compText.color = color;
                }
            }

            public string Text
            {
                get => _compText.text;
                set => _compText.text = value;
            }

            public float FontSize
            {
                get => _compText.fontSize;
                set => _compText.fontSize = value;
            }

            public UIText(TMP_Text compText) => _compText = compText;
        }

        public new void Invoke()
        {
            if (Finished)
            {
                _timer.Reset();
                base.Invoke();
            }
            else
            {
                _timer.Reset();
                _count++;
                SetCombo(_count);
            }
        }

        protected override void Enter()
        {
            _compUI.gameObject.SetActive(true);
            SetCombo(_count);
        }
        protected override void Update()
        {
            // todo
            /*
            if (Input.anyKeyDown)
            {
                _timer.Reset();
                _count++;
                SetCombo(_count);
            }
            */

            if (_timer.Time <= _timer.Lag * 0.75f)
                FadeOut();
        }

        protected override void Exit()
        {
            _compUI.gameObject.SetActive(false);
            _count = 1;
        }
    }
}