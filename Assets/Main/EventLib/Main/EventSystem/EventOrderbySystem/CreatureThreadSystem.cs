using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Util;
using Main.Entity;
using Main.EventLib.Sub.CreatureEvent;

namespace Main.EventLib.Main.EventSystem.EventOrderbySystem
{
    /// <summary>
    /// 跟creature系統互動
    /// </summary>
    /// <code>
    /// Builder build creature,
    /// and append new CreatureEventSystem(creature);
    /// 它會觀察角色是否觸發新的事件，並攔截，
    /// 在系統中，依照優先度、事件是否執行完畢的排序，最後才會執行
    /// </code>
    public class CreatureThreadSystem : IComponent
    {
        /*
         * 一定先傳入要觀測的角色資料
         * 事件執行完畢會離開list
         */

        // InProgress
        // 觀察正在執行的事件，每一個事件必須自帶interrupt方法，允許外界查詢並->{停止該事件, remove from this}
        // 觀察正在執行的事件，當事件結束時，執行remove from this
        private readonly List<IElement> _inProgressEventList = new();

        // 已經篩選好要執行的新的事件，之後foreach->{Invoke, push/Enqueue to InProgress event list, pop/Dequeue from this}
        private readonly Queue<IElement> _waitToStartEventList = new();

        public EnumComponentTag Tag => EnumComponentTag.CreatureThreadSystem;

        public void Update()
        {
            /*
            確認是否有正在執行的事件結束->並離開list
            確認是否有新的事件加入*
            同*，該執行緒大於目前list內事件的->清空list內執行緒低的，並執行新的執行緒，並把該執行緒加入list
            同*，反之，如果執行緒低於上述，則丟棄新的執行緒（不排到隊列裡執行）
            同*，反之，如果執行續等於上述，則加入list，並執行執行續
            
            一旦list內新增單一或多個新事件，會執行
             */
            // 當事件結束，從list中去除
            if (_inProgressEventList.Any(@event => @event.Finished()))
                foreach (var @event in
                    _inProgressEventList.Filter(@event => @event.Finished()))
                {
                    _inProgressEventList.Remove(@event);
                    // creatureEvent.Switch = false; // 已經執行完畢，不必再關閉
                }

            if (!IsInvokeNewEvent) return;
            for (var i = 0; i < _waitToStartEventList.Count; i++)
            {
                var thread = _waitToStartEventList.Dequeue();

                // 如果還沒觸發事件
                if (!_inProgressEventList.Any())
                {
                    _inProgressEventList.Add(thread);
                    thread.Switch = true; // 允許執行
                    thread.InvokeEvent?.Invoke(); // 正式執行該委派
                    continue;
                }

                // 確認新執行緒的順位
                // 如果執行緒低於上述，則丟棄新的執行緒（不排到隊列裡執行）
                if (AnyInProgress(ing =>
                    thread.Order > ing.Order)) // 值越大，執行緒越低
                    continue;

                // 略過執行緒相同，且tag相同，不執行重複程式
                if (AnyInProgress(ing =>
                    thread.Order == ing.Order &&
                    thread.Tag != EnumCreatureEventTag.Debuff &&
                    thread.Tag == ing.Tag))
                    continue;

                // 如果執行續等於上述，則加入list，並執行執行續
                if (AnyInProgress(ing =>
                    thread.Order == ing.Order))
                {
                    _inProgressEventList.Add(thread);
                    thread.Switch = true; // 允許執行
                    thread.InvokeEvent?.Invoke(); // 正式執行該委派
                    continue;
                }


                // 如果該執行緒大於目前list內事件的->清空list內執行緒低的，並執行新的執行緒，並把該執行緒加入list
                var toInterruptEvents = GetLowerInProgressEvents(thread);
                foreach (var interruptEvent in toInterruptEvents)
                {
                    _inProgressEventList.Remove(interruptEvent);
                    interruptEvent.Switch = false;
                }

                _inProgressEventList.Add(thread);
                thread.Switch = true; // 允許執行
                thread.InvokeEvent?.Invoke(); // 正式執行該委派
            }
        }

        private bool IsInvokeNewEvent =>
            _waitToStartEventList.Any();

        private bool AnyInProgress([NotNull] Func<IElement, bool> filter) =>
            _inProgressEventList.All(filter);

        private IElement[] GetLowerInProgressEvents(IElement target) =>
            _inProgressEventList.Filter(ing => target.Order < ing.Order);


        /// 添加事件進入Queue中等待執行
        /// <code>
        /// class normal_event{
        ///     Invoke() => this.CreatureEvent();
        /// }
        /// class orderby_event{
        ///     Invoke(){
        ///         this.Switch = false;
        ///         AppendEventElement(() => this.CreatureEvent());
        ///     }
        /// }
        /// </code>
        public void AppendEventElement(IElement newEvent)
        {
            if (newEvent.Order == EnumOrder.Move && _waitToStartEventList.Contains(newEvent))
                return;// 避免moveEvent重複騷擾事件系統

            _waitToStartEventList.Enqueue(newEvent);
        }

        public override string ToString() =>
            $"{GetType().Name}" +
            $"\n是否有排隊中的事件？ {_waitToStartEventList.Any()} {_waitToStartEventList.Count}" +
            $"\n是否有正在執行中的事件們？{_inProgressEventList.Any()}" +
            $"\n該事件們是否執行完畢？{_inProgressEventList.Select(e => $"{e.Order} {e.Finished()}").ToArray().ArrayToString()}";
    }
}