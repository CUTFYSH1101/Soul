using System;
using Main.Event;
using Main.Util;
using UnityEngine;

namespace Test2
{
    public class Test : MonoBehaviour
    {
        public ICause cause;
        private Skill_1 system;
        private Subject subjectSystem;

        private void Awake()
        {
            system = new Skill_1();
            subjectSystem = new Subject(new Skill_1(), new CdCause(6), "被中斷".LogLine);
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                /*causeEnter = new CdCause(5);
                causeEnter.Reset();*/
                // system = new Skill_1();
                subjectSystem = new Subject(new Skill_1(), new CdCause(2), "被中斷".LogLine);
            }

            // system.Update();
            subjectSystem.Update();
            // Debug.Log(causeEnter?.Cause());
        }

        public class SkillObserver
        {
            private readonly AbstractSkillCompat skillCompat;
            private readonly Node last;
            private readonly Node first; //TODO
            private bool finished;

            public SkillObserver(AbstractSkillCompat skillCompat, ICause causeExit, Action callback)
            {
                this.skillCompat = skillCompat;
                last = new Node(causeExit, null, callback);
            }

            public void Update()
            {
                if (finished)
                    return;

                // 直接跳到最後一步
                if (last.Cause)
                {
                    finished = true;
                    last?.Callback();
                }
                else
                    skillCompat.Update();
            }
        }

        public abstract class Skill_2 : AbstractSkillCompat
        {
            public Node first;
            public Node toAction2;
            public Node toAction3;
            public Node toAction4;
            public Node finish;
            public ICause causeEnter1;//TODO
            public Func<bool> causeEnter;
            
            public abstract void Action1();// 蓄力動畫
            public abstract void Action2();// 攻擊動畫
            public abstract void Action3();// 暈眩動畫
            public abstract void Action4();// 回到一開始

            protected void Init(float cdTime)
            {
                /*first = new Node(new CdCause(cdTime), )
                toAction2 = new Node(EnterAction1)*/
            }

            protected Skill_2()
            {
                
            }
        }

        public class Subject
        {
            private readonly AbstractSkillCompat skillCompat;
            private readonly Node last;
            private readonly Node first; //TODO
            private bool finished;

            public Subject(AbstractSkillCompat skillCompat, ICause causeExit, Action callback)
            {
                this.skillCompat = skillCompat;
                last = new Node(causeExit, null, callback);
            }

            public void Update()
            {
                if (finished)
                    return;

                // 直接跳到最後一步
                if (last.Cause)
                {
                    finished = true;
                    last?.Callback();
                }
                else
                    skillCompat.Update();
            }
        }

        public class Skill_1 : AbstractSkillCompat
        {
            private CdCause cause1, cause2, cause3;

            public Skill_1()
            {
                // 初始化
                cause1 = new CdCause(5);
                cause2 = new CdCause(1);
                cause3 = new CdCause(1);
                // 初始化關卡
                rootHandler =
                    new NodeHandler(new Node(cause1, null, "第一個事件".LogLine)).SetNextHandler(
                        new NodeHandler(new Node(cause2, null, "第二個事件".LogLine)).SetNextHandler(
                            new NodeHandler(new Node(cause3, null, "第三個事件".LogLine))));
                // 指定第一個關卡
                SetHandler(rootHandler);
            }
        }

        public abstract class AbstractSkillCompat
        {
            private NodeHandler nowHandler;
            protected NodeHandler rootHandler;

            public bool Finished => nowHandler.Finished();

            protected void SetHandler(NodeHandler handler)
            {
                handler.Reset();
                nowHandler = handler;
            }

            public virtual void Update()
            {
                if (Finished)
                    return;

                nowHandler.Invoke();

                var newHandler = nowHandler.CheckState();
                // 是否滿足data.Cause，是則切換關卡
                if (nowHandler != newHandler)
                {
                    SetHandler(newHandler);
                }
            }
        }

        public class NodeHandler
        {
            // private NodeHandler rootHandler;
            private NodeHandler nextHandler;
            private Node now;
            private Node next;
            private bool used;
            public Node GETNode => now;

            public NodeHandler(Node now)
            {
                this.now = now;
            }

            public NodeHandler SetNextHandler(NodeHandler theNextHandler)
            {
                nextHandler = theNextHandler;
                next = nextHandler.now;
                return this; // 方便使用
            }

            public NodeHandler CheckState()
            {
                // 如果允許進入下一個狀態觸發 且 不是最後一個時
                if (next != null && now.Cause)
                {
                    return nextHandler?.CheckState();
                }

                return this;

                /*
                if (next.Cause() && next != null)
                {
                    now = next;
                }
                return now;*/
            }

            public bool Finished() => nextHandler == null && used;

            public void Invoke()
            {
                // 使其只在第一幀被呼叫
                if (used)
                    return;
                used = true;

                now?.Callback();
            }

            // 當切換時
            public void Reset()
            {
                now.cause1.Reset(); // 重置CD，開始CD計時
            }
        }

        public class Node
        {
            public ICause cause1;
            public Func<bool> cause;
            public Action callback;
            private bool used;

            public bool Cause
            {
                get
                {
                    if (!used)
                    {
                        used = true;
                        cause1.Reset();
                    }

                    return (cause1 == null || cause1.Cause()) &&
                           (cause == null || cause.Invoke());
                }
            }

            public Action Callback => callback ?? (() => { });

            public Node(ICause cause1, Func<bool> cause, Action callback)
            {
                this.cause1 = cause1;
                this.cause = cause;
                this.callback = callback;
            }
        }
    }
}