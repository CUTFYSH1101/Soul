using System;
using System.Collections;
using Main.Util;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Main.Util.Timers;

namespace Event
{
    /// 偵測是否觸發double click事件
    public class DoubleInput : Timers.RepeatMethod
    {
        private readonly Timer duration = new Timer(.5f);

        // 觸發事件與暴露方法
        public string Key { get; set; }
        private bool GetButtonDown() => Input.GetButtonDown(Key);
        public Action SingleEvent { get; }

        public Action DoubleEvent { get; }

        // 狀態判斷
        public bool InSingleClick { get; private set; }
        public bool DoubleClick { get; private set; }

        public DoubleInput(string key, Action singleEvent, Action doubleEvent, MonoBehaviour mono) : base(mono)
        {
            this.Key = key;
            this.SingleEvent = singleEvent;
            this.DoubleEvent = doubleEvent;
            CallUpdate(true);// 該程式會一直不斷的執行直到遊戲物件的active為false
        }

        protected override void Enter() { }

        protected override void Exit() { }

        protected override IEnumerator Update()
        {
            while (true)
            {
                DoubleClick = false;

                if (!duration.IsTimeUp)
                {
                    duration.Update();
                }

                if (InSingleClick && !duration.IsTimeUp)
                {
                    if (GetButtonDown())
                    {
                        DoubleClick = true;
                        DoubleEvent?.Invoke();
                        InSingleClick = false;
                    }
                }

                if (GetButtonDown())
                {
                    SingleEvent?.Invoke();
                    if (!InSingleClick)
                    {
                        InSingleClick = true;
                        duration.Reset();
                    }
                }

                if (duration.IsTimeUp)
                {
                    InSingleClick = false;
                }

                yield return new Update();
            }
        }

        public override string ToString()
        {
            var message =
                $"------------------------\n" +
                $"{GetType().Name}\n" +
                $"計時單位： {duration.GetIsNotNullToString()}\n" +
                $"呼叫迭代器單位：{base.ToString()}" +
                $"雙擊： {DoubleClick}\n" +
                $"------------------------\n";
            return message;
        }
    }
}