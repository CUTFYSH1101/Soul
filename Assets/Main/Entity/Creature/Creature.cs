using System;
using JetBrains.Annotations;
using Main.AnimAndAudioSystem.Anims.Scripts;
using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.CreatureAndBehavior.Behavior;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.Game;
using Main.Game.Collision;
using UnityEngine;


namespace Main.Entity.Creature
{
    [Serializable]
    public class Creature : Entity
    {
        [SerializeField] private CreatureAttr creatureAttr;

        public CreatureAttr CreatureAttr
        {
            get => creatureAttr;
            protected set => creatureAttr = value;
        }

        public void SetMindState(EnumMindState newMindState) =>
            creatureAttr.MindState = newMindState;

        public Transform Transform { get; }

        /// 獲取絕對位置
        public Vector2 AbsolutePosition => Transform.position;

        public UnityRb2D Rigidbody2D { get; }
        public CreatureAnimManager AnimManager { get; protected set; }
        public bool IsKilled() => AnimManager.IsTag("Die");
        public DictionaryAudioPlayer AudioPlayer { get; protected set; }
        public IBehavior Behavior => (IBehavior) FindDataByTag(EnumDataTag.Behavior);
        public SkillAttr CurrentSkill
        {
            get
            {
                try
                {
                    return Behavior?.FindSkillAttrByTag(creatureAttr.CurrentSkill);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private readonly FlipChecker _flipChecker;
        public bool IsFacingRight => _flipChecker.IsFacingRight;

        /// 該角色的攻擊速度
        public float AttackSpeed
        {
            get => creatureAttr.AttackSpeed;
            set
            {
                creatureAttr.AttackSpeed = value;
                AnimManager.SetAttackSpeed(value);
            }
        }

        public CreatureThreadSystem ThreadSystem { get; private set; }

        protected internal Creature(Transform transform, CreatureAttr creatureAttr,
            [CanBeNull] DictionaryAudioPlayer audioPlayer = null)
        {
            CreatureAttr = creatureAttr;
            var animator = transform.GetOrLogComponent<Animator>();
            AnimManager = new CreatureAnimManager(animator);
            Transform = transform;
            Rigidbody2D = transform.GetOrAddComponent<UnityRb2D>();
            AudioPlayer = audioPlayer;

            AppendComponent(new GroundChecker(this)); // 地板更新事件
            _flipChecker = (FlipChecker) AppendComponent(new FlipChecker(transform)); // 轉向更新
            ThreadSystem = (CreatureThreadSystem) AppendComponent(new CreatureThreadSystem()); // 角色事件
        }

        public override void Update()
        {
            base.Update();
            AnimManager.Knockback(creatureAttr.DuringDeBuff); // 當在debuff中，更改顯示狀態
        }

        // ======
        // 類別
        // ======
        private class GroundChecker : IComponent
        {
            private readonly CreatureAnimManager _moveAnim;
            private readonly GroundCollision _groundCollision;
            private readonly CreatureAttr _creatureAttr;
            public bool Switch { get; set; } = true;

            public GroundChecker(Creature creature)
            {
                _groundCollision = new GroundCollision(creature.Transform);
                _moveAnim = creature.AnimManager;
                _creatureAttr = creature.CreatureAttr;
            }

            public bool GetGrounded() => _groundCollision.Grounded();

            public int Id { get; }

            public void Update()
            {
                _creatureAttr.Grounded = GetGrounded(); // 更新角色跳躍狀態
                _moveAnim.JumpUpdate(Switch // 更新角色動畫
                    ? _groundCollision.VerticalDir
                    : 0);
            }

            private class GroundCollision
            {
                private readonly UnityRb2D _rigidbody2D;
                private readonly Transform _bottomObject;
                private readonly CollisionManager.TouchTheGroundEvent _groundEvent;

                public int VerticalDir
                {
                    get
                    {
                        if (_rigidbody2D == null)
                            return 0;

                        if (Grounded())
                            return 0;

                        var speed = _rigidbody2D.instance.velocity.y;
                        // 當靜止在空中時，設為下降中，避免空中變回idle
                        if (speed == 0)
                            speed = -1;
                        return Math.Sign(speed);
                    }
                }

                // public bool Grounded() => bottomObject.IsGrounded();
                public bool Grounded() => _groundEvent.IsTriggerStay;

                /// 根據collider最底下的點座標進行判斷
                public GroundCollision([NotNull] Transform container)
                {
                    _rigidbody2D = container.GetOrAddComponent<UnityRb2D>(); // 因此必定有rb2d

                    // GroundChecker
                    _bottomObject = container.GetFirstComponentInChildren<Transform>("GroundChecker");

                    // 垂直正下方
                    _groundEvent = new CollisionManager.TouchTheGroundEvent(container);
                }
            }
        }

        [Serializable]
        private class FlipChecker : IComponent
        {
            public bool IsFacingRight { get; private set; }
            private UnityRb2D _rigidbody2D;

            public FlipChecker(Transform transform, bool isFacingRight = false)
            {
                _rigidbody2D = transform.GetOrAddComponent<UnityRb2D>();
                IsFacingRight = isFacingRight;
            }

            /// 翻面
            public void Flip()
            {
                IsFacingRight = !IsFacingRight;
                _rigidbody2D.transform.localScale =
                    Vector3.Scale(_rigidbody2D.transform.localScale, new Vector3(-1, 1, 1));
            }

            public int Id { get; }

            public void Update()
            {
                if (_rigidbody2D.ActiveX + _rigidbody2D.GuideX > 0.1f && !IsFacingRight)
                    Flip(); //往左翻

                if (_rigidbody2D.ActiveX + _rigidbody2D.GuideX < -0.1f && IsFacingRight)
                    Flip(); //往右翻
            }
        }
    }

    /*/// 控制角色行為，支援Anim+Attr+場景互動
    public class AbstractCreatureBehavior
    {
    }*/
}