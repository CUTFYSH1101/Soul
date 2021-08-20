using JetBrains.Annotations;
using Main.EventSystem.Event.Attribute;
using Main.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Math = System.Math;
using Time = Main.Util.Time;

namespace Main.EventSystem.Event.UIEvent.CD
{
    public class CdEvent : UIEvent
    {
        private float _timer;
        private readonly Transform _compUI;

        private readonly Image _compCover;

        //TextMeshProUGUI,TMP_Text
        private readonly TMP_Text _compSeconds;

        public CdEvent(string hierarchyPath, float cdDuration) :
            base(new EventAttr(0, cdDuration))
        {
            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compCover = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Cover");
            _compSeconds = _compUI == null ? null : _compUI.GetFirstComponentInChildren<TMP_Text>("Seconds");

            CauseEnter = new FuncCause(() => !IsEmpty());

            if (IsEmpty()) return;
            _compCover.enabled = false; // 一開始不顯示
            _compSeconds.enabled = false;
            CauseExit = new FuncCause(() => false);
        }

        private bool IsEmpty() =>
            _compCover == null || _compSeconds == null;


        public void UpdateDuration(float newTime)
        {
            if (State == EnumState.Free)
                EventAttr.MaxDuration = newTime;
        }

        public new void Invoke()
        {
            if (IsEmpty()) return;
            if (EventAttr.MaxDuration == 0)
            {
                _compSeconds.text = "∞"; // miss 顯示不出來
                return;
            }

            base.Invoke();
        }
        public override UIEvent AppendToCdTime([NotNull] AbstractEvent @event)
        {
            @event.PreWork += () =>
            {
                if (State == EnumState.Free)
                    EventAttr.MaxDuration = @event.CdTime;
                Invoke();
            };
            return this;
        }

        protected override void Enter()
        {
            _compCover.enabled = true;
            _compSeconds.enabled = true;
            _timer = EventAttr.MaxDuration;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
            _compSeconds.text = _timer.ToString();
        }

        protected override void Update()
        {
            _timer -= Time.DeltaTime;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
            _compSeconds.text = Math.Ceiling(_timer).ToString();
        }

        protected override void Exit()
        {
            _compCover.enabled = false;
            _compSeconds.enabled = false; // 結束後再次關閉顯示
        }
    }
}