using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;

/*
1. stay偵測
2. 探索除了stay以外的新的碰撞物件
3. 收集第一次的碰撞物件
 */
namespace Main.Game.Collision.Event
{
    public class TriggerEnterEvent : IComponent, IPhysicEvent
    {
        [NotNull] private readonly ITriggerStayEvent _triggerStay;
        private Collider2D[] _oldOthersStay; // 紀錄上一幀的事件
        private readonly Action<Collider2D> _onEnter;
        public bool IsTrigger { get; private set; }
        public Collider2D[] Others { get; private set; }

        public TriggerEnterEvent(Component component, Action<Collider2D> onEnter)
        {
            _triggerStay = new TriggerStayEvent(component);
            _onEnter = onEnter;
        }

        public TriggerEnterEvent(ITriggerStayEvent stayEvent, Action<Collider2D> onEnter)
        {
            _triggerStay = stayEvent;
            _onEnter = onEnter;
        }

        public EnumComponentTag Tag => EnumComponentTag.PhysicsCollisionSystem;

        public void Update() => UpdateDataAndInvokeAction(IsTriggerEnter(), _onEnter);

        private bool IsTriggerEnter()
        {
            /*
            var isNew = false;
            // 當沒有舊資料，並添加新的
            if (_oldOthersStay == null && _triggerStay.Others != null)
                isNew = true;

            else if (_oldOthersStay != null && _triggerStay.Others != null)
                // 當兩資料不對等就代表有新的碰撞事件
                isNew = _oldOthersStay.Length != _triggerStay.Others.Length;
                */
            var isNew = _oldOthersStay == null || _oldOthersStay.Length != _triggerStay.Others.Length;
            // 取得布林值
            return _triggerStay.IsTrigger && isNew;
        }

        private void UpdateDataAndInvokeAction(bool isTriggerEnter, Action<Collider2D> onEnter)
        {
            var others = new Queue<Collider2D>();
            var oldArray = _oldOthersStay;
            var newArray = _triggerStay.Others;
            if (isTriggerEnter) // 此時，old.length <= new.length
            {
                var oldList = oldArray?.ToList();
                foreach (var value in newArray) // 通過isTriggerEnter者，newArray不可能為空，因此不必判空
                    if (oldList == null || !oldList.Any() || !oldList.Contains(value))
                        others.Enqueue(value);
                // 當isTriggerEnter，依序觸發事件
                if (onEnter != null)
                    foreach (var other in others)
                        onEnter.Invoke(other);
            }

            // 更新資料
            _oldOthersStay = newArray;
            IsTrigger = isTriggerEnter;
            Others = isTriggerEnter ? others.ToArray() : null;
        }
        // 節省效能
    }
}