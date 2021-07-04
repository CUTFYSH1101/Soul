using JetBrains.Annotations;
using Main.Game.Collision;
using Main.Util;
using UnityEngine;
using Math = System.Math;
using Rb2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity
{
    public class IEntity
    {
        protected string name;
        protected Rb2D rigidbody2D;
        protected Transform transform;
        protected Animator animator;

        public string Name => name;
        protected GroundChecker groundChecker;
        public bool Grounded => inited && groundChecker.Grounded();
        public int VerticalDir => inited ? groundChecker.VerticalDir : 0;

        public Rb2D Rigidbody2D => rigidbody2D;
        public Transform Transform => transform;
        public Animator Animator => animator;
        public Vector2 Position => rigidbody2D.transform.position;
        
        private bool inited;

        public IEntity Instance(string name, Transform transform)
        {
            // Type type = Type.None;
            this.name = name;
            this.transform = transform;
            this.rigidbody2D = transform.GetComponent<Rb2D>();
            this.animator = transform.GetComponent<Animator>();
            groundChecker = new GroundChecker(transform);
            inited = true;
            return this;
        }

        public IEntity Instance(Transform transform)
        {
            // Type type = Type.AutoInsert;
            this.name = transform.name;
            this.transform = transform;
            this.rigidbody2D = transform.GetOrAddComponent<Rb2D>();
            this.animator = transform.GetOrAddComponent<Animator>();
            groundChecker = new GroundChecker(transform);
            inited = true;
            return this;
        }

        public class GroundChecker
        {
            private readonly Rb2D rigidbody2D;
            private readonly Transform bottomObject;

            public int VerticalDir
            {
                get
                {
                    int repo;
                    if (rigidbody2D == null)
                        return 0;

                    if (!Grounded())
                    {
                        var speed = rigidbody2D.instance.velocity.y;
                        // 當靜止在空中時，設為下降中，避免空中變回idle
                        if (speed == 0)
                            speed = -1;
                        repo = Math.Sign(speed);
                    }
                    else
                        repo = 0;

                    return repo;
                }
            }

            public bool Grounded() => bottomObject.IsGrounded();

            /// 根據collider最底下的點座標進行判斷
            public GroundChecker([NotNull] Transform container)
            {
                rigidbody2D = container.GetOrAddComponent<Rb2D>();// 因此必定有rb2d
                
                // GroundChecker
                bottomObject = container.GetFirstComponent<Transform>("GroundChecker");// todo check
                // 垂直正下方
            }
        }
    }
}