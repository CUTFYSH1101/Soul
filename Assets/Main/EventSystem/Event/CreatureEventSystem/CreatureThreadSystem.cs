using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Entity;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.Util;

namespace Main.EventSystem.Event.CreatureEventSystem
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
        public CreatureThreadSystem()
        {
        }

        // InProgress
        // 觀察正在執行的事件，每一個事件必須自帶interrupt方法，允許外界查詢並->{停止該事件, remove from this}
        // 觀察正在執行的事件，當事件結束時，執行remove from this
        private readonly List<ICreatureEvent> _inProgressEventList = new List<ICreatureEvent>();

        // 已經篩選好要執行的新的事件，之後foreach->{Invoke, push/Enqueue to InProgress event list, pop/Dequeue from this}
        private readonly Queue<ICreatureEvent> _waitToStartEventList = new Queue<ICreatureEvent>();

        public int Id { get; }

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
            if (_inProgressEventList.Any(creatureEvent => creatureEvent.Finished()))
                foreach (var creatureEvent in
                    _inProgressEventList.Filter(creatureEvent => creatureEvent.Finished()))
                {
                    _inProgressEventList.Remove(creatureEvent);
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
                    thread.Order > ing.Order))// 值越大，執行緒越低
                    continue;
                
                // 略過執行緒相同，且tag相同，不執行重複程式
                if (AnyInProgress(ing =>
                    thread.Order == ing.Order && 
                    thread.Tag != EnumCreatureEventTag.DeBuff && 
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

        private bool AnyInProgress([NotNull] Func<ICreatureEvent, bool> filter) =>
            _inProgressEventList.All(filter);
        
        private ICreatureEvent[] GetLowerInProgressEvents(ICreatureEvent target) =>
            _inProgressEventList.Filter(ing => target.Order < ing.Order);


        // 不包含觸發本身，純事件
        /// <summary>
        /// class IEventA : ICreatureEvent{
        ///     Invoke(){
        ///         Invoke(SubIEvent.this);
        ///         SubIEvent.Switch = false;
        ///     }
        /// }
        /// class Knockback : IEventA{
        ///     Invoke(Vector2 direction, float force){
        ///         ......
        ///         ......
        ///         if base.InvokeEvent == null
        ///             base.InvokeEvent = this; // delegate
        ///         base.Invoke();
        ///     }
        /// }
        /// </summary>
        /// <param name="newEvent"></param>
        public void AppendThread(ICreatureEvent newEvent) => _waitToStartEventList.Enqueue(newEvent);

        public override string ToString() =>
            $"{GetType().Name}" +
            $"\n是否有排隊中的事件？ {_waitToStartEventList.Any()} {_waitToStartEventList.Count}" +
            $"\n是否有正在執行中的事件們？{_inProgressEventList.Any()}" +
            $"\n該事件們是否執行完畢？{_inProgressEventList.Select(e => $"{e.Order} {e.Finished()}").ToArray().ArrayToString()}";
    }
}