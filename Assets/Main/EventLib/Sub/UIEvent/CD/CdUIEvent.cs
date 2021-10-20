using Main.EventLib.Main.EventSystem.Main;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Math = System.Math;

namespace Main.EventLib.Sub.UIEvent.CD
{
    public class CdUIEvent : UIEvent
    {
        private float _timer;
        private readonly Transform _compUI;

        private readonly Image _compCover;

        //TextMeshProUGUI,TMP_Text
        private readonly TMP_Text _compSeconds;

        public CdUIEvent(string hierarchyPath, float cdDuration)
        {
            this.Build();
            EventAttr = new EventAttr(0, cdDuration);

            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compCover = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Cover");
            _compSeconds = _compUI == null ? null : _compUI.GetFirstComponentInChildren<TMP_Text>("Seconds");

            FilterIn = () => !IsEmpty();
            // Debug.Log(cdDuration);
            if (IsEmpty()) return;
            _compCover.enabled = false; // 一開始不顯示
            _compSeconds.enabled = false;
            ToInterrupt = () => false;
        }

        private bool IsEmpty() =>
            _compCover == null || _compSeconds == null;


        public void UpdateDuration(float newTime)
        {
            if (State == EnumState.Free)
                EventAttr.MaxDuration = newTime;
        }

        public void Invoke()
        {
            if (IsEmpty()) return;
            if (EventAttr.MaxDuration == 0)
            {
                _compSeconds.text = "∞"; // miss 顯示不出來
                return;
            }

            Director.CreateEvent();
        }
        public override UIEvent AppendToCdTime(IEvent @event)
        {
            @event.PreWork += () =>
            {
                if (State == EnumState.Free)
                    this.SetDuration(@event.EventAttr.CdTime);
                Invoke();
            };
            return this;
        }

        public override void Enter()
        {
            _compCover.enabled = true;
            _compSeconds.enabled = true;
            _timer = EventAttr.MaxDuration;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
            _compSeconds.text = _timer.ToString();
        }

        public override void Update()
        {
            _timer -= Util.Time.DeltaTime;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
            _compSeconds.text = Math.Ceiling(_timer).ToString();
        }

        public override void Exit()
        {
            _compCover.enabled = false;
            _compSeconds.enabled = false; // 結束後再次關閉顯示
        }
    }
}