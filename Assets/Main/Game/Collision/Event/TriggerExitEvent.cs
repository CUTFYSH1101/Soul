using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;

namespace Main.Game.Collision.Event
{
    public class TriggerExitEvent : IComponent, IPhysicEvent
    {
        [NotNull] private readonly ITriggerStayEvent _triggerStay;
        private Collider2D[] _oldOthersStay; // 紀錄上一幀的事件
        private readonly Action<Collider2D> _onExit;
        public bool IsTrigger { get; private set; }
        public Collider2D[] Others { get; private set; }

        public TriggerExitEvent(Component component, Action<Collider2D> onExit)
        {
            _triggerStay = new TriggerStayEvent(component);
            _onExit = onExit;
        }

        public TriggerExitEvent(ITriggerStayEvent stayEvent, Action<Collider2D> onExit)
        {
            _triggerStay = stayEvent;
            _onExit = onExit;
        }

        public EnumComponentTag Tag => EnumComponentTag.PhysicsCollisionSystem;

        public void Update() =>
            UpdateDataAndInvokeAction(IsTriggerExit(), _onExit);

        private bool IsTriggerExit()
        {
            /*
            var isNew = false;
            // 當新資料沒有，而舊資料有
            if (_oldOthersStay != null && _triggerStay.Others == null)
                isNew = true;

            else if (_oldOthersStay != null && _triggerStay.Others != null)
                // 當資料不對等就代表有新的撞擊事件
                isNew = _oldOthersStay.Length != _triggerStay.Others.Length;
                */
            var newArray = _triggerStay.Others;
            var isNew = _oldOthersStay != null && newArray == null || _oldOthersStay != null &&
                _oldOthersStay.Length != newArray.Length;
            // 取得布林值
            return _triggerStay.IsTrigger && isNew;
        }

        private void UpdateDataAndInvokeAction(bool isTriggerEnter, Action<Collider2D> onExit)
        {
            var others = new Queue<Collider2D>();
            var oldArray = _oldOthersStay;
            var newArray = _triggerStay.Others;
            if (isTriggerEnter) // 此時，old.length >= current.length
            {
                var newList = newArray?.ToList();
                foreach (var value in oldArray)
                    if (newList == null || !newList.Any() || !newList.Contains(value))
                        others.Enqueue(value);
                // 當isTriggerExit，依序觸發事件
                if (onExit != null)
                    foreach (var other in others)
                        onExit.Invoke(other);
            }

            // 更新資料
            _oldOthersStay = newArray;
            IsTrigger = isTriggerEnter;
            Others = isTriggerEnter ? others.ToArray() : null;
        }
    }
}