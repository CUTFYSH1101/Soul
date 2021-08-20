using System;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Util;
using Main.Game;
using Main.Game.Collision;
using Main.Util;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Physics2D = UnityEngine.Physics2D;
using Time = Main.Util.Time;
using UnityInput = UnityEngine.Input;

namespace Main.EventSystem.Event.UIEvent.QTE
{
    /*
     * subject.invoke,
     * if onTrigger && isQTEEvent
     * QTEEvent.Invoke
     */
    public class QteUIEvent : UIEvent
    {
        private Sprite _origin;
        private readonly QteImageList _imageList;
        private readonly Transform _compUI;
        private readonly Image _compCover;
        private readonly Image _compBackground;
        private QteImageList.CompImage icon;
        public bool Miss { get; set; }
        private float _timer;

        /// 1.自帶傷害事件 或
        /// 2.由呼叫的Skill判斷，移除此項
        public QteUIEvent(string hierarchyPath)
        {
            _imageList = UnityRes.GetQteImageList();
            _compUI = UnityTool.GetComponentByPath<Transform>(hierarchyPath);
            _compBackground = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Background");
            _compCover = _compUI == null ? null : _compUI.GetFirstComponentInChildren<Image>("Cover");
            CauseEnter = new FuncCause(() => !IsEmpty());
            CauseExit = new FuncCause(() => Miss); // if keydown or duration.isTimeUp

            if (_compUI == null)
                return;
            _compUI.gameObject.SetActive(false);
        }

        private bool IsEmpty() =>
            _compBackground == null || _compCover == null;

        public void Invoke(EnumQteSymbol icon)
        {
            Miss = false;
            this.icon = _imageList.Get(icon);
            base.Invoke();
        }

        public float Duration
        {
            get => EventAttr.MaxDuration;
            set => EventAttr.MaxDuration = value;
        }

        protected override void Enter()
        {
            // 顯示UI
            _compUI.gameObject.SetActive(true);
            _origin = _compBackground.sprite;
            _compBackground.sprite = icon.image;
            _compBackground.rectTransform.sizeDelta = icon.size;
            _timer = EventAttr.MaxDuration;
            _compCover.fillAmount = 1;
        }

        protected override void Update()
        {
            _timer -= Time.DeltaTime;
            _compCover.fillAmount = _timer / EventAttr.MaxDuration;
        }

        protected override void Exit()
        {
            // 關閉UI
            _compUI.gameObject.SetActive(false);
            _compBackground.sprite = _origin;
        }
    }
}