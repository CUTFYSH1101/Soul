using System;
using Extension.Entity.Controller;
using JetBrains.Annotations;
using Main.Entity.Controller;
using Main.Util;
using Test2.Timers;
using UnityEngine;
using static Test2.Timers.Stopwatch;

/// 所有的cause一定來自unity
namespace Test2.Causes
{
    /// 所有的cause一定來自unity
    public interface ICause
    {
        bool Cause();
        void Reset();
    }

    /// <summary>
    /// IsTimeUp
    /// </summary>
    public class CDCause : ICause
    {
        private readonly CDTimer cdTimer;

        public CDCause(float timer, Mode mode = Mode.LocalGame)
        {
            cdTimer = new CDTimer(timer, mode);
        }

        public bool Cause()
        {
            var temp = cdTimer.IsTimeUp;
            return temp;
        }

        public void Reset()
        {
            if (cdTimer.IsTimeUp) cdTimer.Reset();
        }
    }

    /// <summary>
    /// 是否碰撞。仰賴Unity Vector
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractCollision<T> : ICause
    {
        protected T target;
        protected T subject;
        protected abstract Vector2 TargetPos();
        protected abstract Vector2 SubjectPos();
        protected float radius = 0.1f;
        protected int? layerMask;

        public bool Cause()
        {
            return SubCause();
        }

        public void Reset()
        {
        }

        protected abstract bool SubCause();
    }

    /// <summary>
    /// 是否碰撞。仰賴Unity Vector和Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentCollision<T> : AbstractCollision<T> where T : Component
    {
        protected override Vector2 TargetPos() => target.transform.position;

        protected override Vector2 SubjectPos() => subject.transform.position;

        /// 自定義碰撞事件
        protected readonly Method method;

        /// 自定義碰撞事件
        protected enum Method
        {
            SingleLayer,
            MultipleLayer
        }

        /// 針對對象群
        public ComponentCollision(T subject, float radius = 0.1f, [CanBeNull] int? layerMask = null)
        {
            method = Method.MultipleLayer;
            this.subject = subject;
            this.radius = radius;
            this.layerMask = layerMask;
        }

        /// 針對單一物件
        public ComponentCollision(T subject, T target, float radius = 0.1f, [CanBeNull] int? layerMask = null)
        {
            method = Method.SingleLayer;
            this.target = target;
            this.subject = subject;
            this.radius = radius;
            this.layerMask = layerMask;
        }

        protected override bool SubCause()
        {
            bool result = false;
            switch (method)
            {
                case Method.MultipleLayer:
                    result = subject.transform
                        .CircleCast(SubjectPos(), radius, layerMask);
                    break;
                case Method.SingleLayer:
                    result = subject.transform
                        .CircleCast(obj => obj.transform == target.transform, SubjectPos(), radius, layerMask);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
    }

    public class DBClick : ICause
    {
        private Action ONSingle
        {
            get
            {
                SubReset();
                return onSingle;
            }
        }

        private Action ONDouble
        {
            get
            {
                SubReset();
                return onDouble;
            }
        }

        private readonly Action onSingle, onDouble;
        private readonly CDTimer singleTimer;
        private readonly Input input;

        public enum State
        {
            None,
            Single,
            Double
        }

        private State state = State.None;

        public DBClick(string key, float duration)
        {
            input = new Input(key);
            this.singleTimer = new CDTimer(duration, Mode.RealWorld);
            onSingle = () => "single event".LogLine();
            onDouble = () => "double event".LogLine();
        }

        /// 初始化狀態。還沒有任何點擊
        private bool HasNoClick()
            => state == State.None && !input.GetButtonDown();

        /// Start。當第一次點擊。優先度高
        private bool IsFirstClick()
            => state == State.None && input.GetButtonDown();

        /// 當第一次點擊後～第二次點擊前 || 第一次點擊後~執行OnSingleEvent
        private bool IsInSingle()
            => state == State.Single && !singleTimer.IsTimeUp;

        /// 當第二次點擊。注意優先度中
        private bool IsDouble()
            => IsInSingle() && input.GetButtonDown();

        /// 當玩家在時間結束之前沒有再次點擊，瞬間觸發。注意仰賴外部呼叫。優先度低
        private bool IsSingle()
            => state == State.Single && singleTimer.IsTimeUp;

        private void SubReset() => state = State.None;


        public bool Cause()
        {
            // 1. 還沒點擊還沒計時return false
            // 2. 按下第一次，
            // 3. 第一次時間到期，執行single ev
            // 4. 第一次時間未到期，按下第二次，執行double eve
            if (HasNoClick())
            {
                return false;
            }

            // 如果dbClick條件滿足，會優先執行
            if (IsDouble())
            {
                state = State.Double;
                ONDouble?.Invoke();
                return true;
            }


            if (IsSingle())
            {
                ONSingle?.Invoke();
                return false;
            }

            // 第一次按下，設定下一次事件
            if (IsFirstClick())
            {
                state = State.Single;
                singleTimer.Reset();
            }

            return false;
        }

        // 在玩家觸發IsSingle以後，依然會持續一段時間，直到Reset
        public void Reset()
        {
        }
    }

    public class SingleClick : ICause
    {
        private readonly DBClick dbClick;
        private readonly CDTimer singleTimer;
        private DBClick.State state = DBClick.State.None;
        private readonly Input input;

        /// 優先級0
        private bool IsFirstClick()
            => state == DBClick.State.None && input.GetButtonDown();

        private bool IsInSingle()
            => state == DBClick.State.Single && !singleTimer.IsTimeUp;

        /// 優先級1
        private bool IsDouble()
            => IsInSingle() && input.GetButtonDown();

        /// 優先級2
        private bool IsSingle()
            => state == DBClick.State.Single && singleTimer.IsTimeUp;

        public SingleClick(string key, float limitTime)
        {
            input = new Input(key);
            singleTimer = new CDTimer(limitTime, Mode.RealWorld);
        }

        public bool Cause()
        {
            if (IsFirstClick())
            {
                state = DBClick.State.Single;
                singleTimer.Reset();
            }

            if (IsDouble())
            {
                /*state = DBClick.State.Double;
                // ...*/
                SubReset();
            }

            var temp = IsSingle();
            if (IsSingle())
            {
                SubReset();
            }

            return temp;
        }

        public void Reset()
        {
        }

        private void SubReset() => state = DBClick.State.None;
    }
}