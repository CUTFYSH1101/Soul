using Main.EventLib.Main.EventSystem.Main;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem;
using Main.Game;
using TMPro;
using UnityEngine;

namespace Main.EventLib.Sub.UIEvent.Combo
{
    /// <summary>
    /// 觸發時：
    /// - 計數+1
    /// - 播放字體彈出特效
    /// - 開始計時（0.5s內如果不觸發第二次則淡出）
    /// </summary>
    public class ComboUIEvent : UIEvent
    {
        private static Transform _compUI;
        private static UIText _compText;
        private static int _count = 1; // 計數器
        private static CdCondition _timer;

        public ComboUIEvent(string hierarchyPath)
        {
            this.Build();
            EventAttr = new EventAttr(0, 999);
            if (_compUI != null)
                return;
            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compText = new UIText(_compUI == null ? null : _compUI.GetFirstComponentInChildren<TMP_Text>("Text"));
            FilterIn = () => !IsEmpty();
            ToInterrupt = () => _timer.OrCause();
            _timer = new CdCondition(2);
            _compUI.gameObject.SetActive(false);
        }

        private bool IsEmpty() =>
            _compText == null;

        private static void SetCombo(float count)
        {
            _compText.Text = count.ToString();
            _compText.FontSize = count > 10 ? MAXSize : OriginSize;
            _compText.Alpha = 1;
        }

        // [40,64,80] [5,8,10]
        private const float OriginSize = 28;
        private const float MAXSize = OriginSize * 1.25f;
        private const float MINSize = OriginSize * 0.625f;

        private static void FadeOut()
        {
            if (_compText.FontSize > MINSize)
            {
                _compText.FontSize -= global::Main.Util.Time.DeltaTime;
                _compText.Alpha -= global::Main.Util.Time.DeltaTime;
            }
        }
        public void Trigger()
        {
            if (State == EnumState.Free)
            {
                _timer.Reset();
                Director.CreateEvent();
            }
            else
            {
                _timer.Reset();
                _count++;
                SetCombo(_count);
            }
        }

        public override void Enter()
        {
            _compUI.gameObject.SetActive(true);
            SetCombo(_count);
        }

        public override void Update()
        {
            if (_timer.Time <= _timer.Lag * 0.75f)
                FadeOut();
        }

        public override void Exit()
        {
            _compUI.gameObject.SetActive(false);
            _count = 1;
        }
    }
}