using System.Collections;
using UnityEngine;

namespace Main.Util
{
    public class Timers
    {
        /// 計時器方法
        public abstract class RepeatMethod
        {
            private readonly MonoBehaviour mono;
            private IEnumerator updateMethod;

            protected RepeatMethod(Component component)
            {
                mono = component.GetOrAddComponent<MonoClass>();
            }

            protected void CallUpdate(bool @switch)
            {
                if (@switch)
                {
                    updateMethod = Update();
                    mono.StartCoroutine(updateMethod);
                }
                else
                {
                    mono.StopCoroutine(updateMethod);
                }
            }

            protected virtual void Invoke()
            {
                Enter();
                CallUpdate(true);
            }

            protected abstract void Enter();

            // 注意不要使用mono.StopCoroutine(Update());因可能會停掉其他類別的同名方法
            protected abstract void Exit();
            protected abstract IEnumerator Update();

            public override string ToString()
            {
                var message =
                    $"------------------------\n" +
                    $"{GetType().Name}\n" +
                    $"呼叫迭代器： {mono.GetIsNotNullToString()}\n" +
                    $"更新方法： {updateMethod.GetIsNotNullToString()}\n" +
                    $"------------------------\n";
                return message;
            }
        }

        /// 計時器方法
        public class Timer
        {
            public float Lag { get; set; } = .1f;
            private float time;
            public void Reset() => time = Lag;
            public void Update() => time -= Time.deltaTime;

            public bool IsTimeUp => time <= 0;

            // 初始化計時器
            // public Timer() => Reset();
            public Timer() {}

            // 初始化計時器
            public Timer(float lag)
            {
                Lag = lag;
                // Reset();
            }
        }
    }
}