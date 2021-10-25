using System;
using JetBrains.Annotations;
using Main.Game.Collision;
using Main.CreatureBehavior.Behavior;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.Game;
using Main.Res.CharactersRes.Animations.Scripts;
using Main.Res.Script.Audio;
using UnityEngine;


namespace Main.Entity.Creature
{
    [Serializable]
    public class Creature : Entity
    {
        [SerializeField] private CreatureAttr creatureAttr;

        [NotNull]
        public CreatureAttr CreatureAttr
        {
            get => creatureAttr;
            protected set => creatureAttr = value;
        }

        public void SetMindState(EnumMindState newMindState) =>
            creatureAttr.MindState = newMindState;

        [NotNull] public Transform Transform { get; }

        /// 獲取絕對位置
        public Vector2 AbsolutePosition => Transform.position;

        [NotNull] public ProxyUnityRb2D Rigidbody2D { get; }
        [NotNull] public CreatureAnimInterface AnimInterface { get; protected set; }
        public bool IsKilled() => AnimInterface.IsTag("Die");
        [CanBeNull] public DictAudioPlayer AudioPlayer { get; protected set; } = null;
        [CanBeNull] public IBehavior Behavior => (IBehavior)FindByTag(EnumDataTag.Behavior);

        [CanBeNull]
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

        [NotNull] private readonly FlipChecker _flipChecker;
        public bool IsFacingRight => _flipChecker.IsFacingRight;

        /// 該角色的攻擊速度
        public float AttackSpeed
        {
            get => creatureAttr.AttackSpeed;
            set
            {
                creatureAttr.AttackSpeed = value;
                AnimInterface.SetAtkSpeed(value);
            }
        }

        [NotNull] public CreatureThreadSystem ThreadSystem { get; private set; }

        protected internal Creature(Transform transform, CreatureAttr creatureAttr,
            [CanBeNull] DictAudioPlayer audioPlayer = null)
        {
            CreatureAttr = creatureAttr;
            var animator = transform.GetOrLogComponent<Animator>();
            AnimInterface = new CreatureAnimInterface(animator);
            Transform = transform;
            Rigidbody2D = transform.GetOrAddComponent<ProxyUnityRb2D>();
            AudioPlayer = audioPlayer;

            Append(new GroundChecker(this)); // 地板更新事件
            _flipChecker = (FlipChecker)Append(new FlipChecker(transform)); // 轉向更新
            ThreadSystem = (CreatureThreadSystem)Append(new CreatureThreadSystem()); // 角色事件
        }

        public override void Update()
        {
            if (IsKilled() || GamePause.IsGamePause) return;
            base.Update();
            AnimInterface.Knockback(creatureAttr.DuringDeBuff); // 當在debuff中，更改顯示狀態
        }

        // ======
        // 類別
        // ======
        private class GroundChecker : IComponent
        {
            private readonly CreatureAnimInterface _moveAnim;
            private readonly GroundCollision _groundCollision;
            private readonly CreatureAttr _creatureAttr;
            public bool Switch { get; set; } = true;

            public GroundChecker(Creature creature)
            {
                _groundCollision = new GroundCollision(creature.Transform);
                _moveAnim = creature.AnimInterface;
                _creatureAttr = creature.CreatureAttr;
            }

            public bool GetGrounded() => _groundCollision.Grounded();

            public EnumComponentTag Tag => EnumComponentTag.GroundChecker;

            public void Update()
            {
                _creatureAttr.Grounded = GetGrounded(); // 更新角色跳躍狀態
                _moveAnim.JumpUpdate(Switch // 更新角色動畫
                    ? _groundCollision.VerticalDir
                    : 0);
            }

            private class GroundCollision
            {
                private readonly ProxyUnityRb2D _rigidbody2D;
                private readonly Transform _bottomObject;

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
                public bool Grounded() => _rigidbody2D.IsGrounded();

                /// 根據collider最底下的點座標進行判斷
                public GroundCollision([NotNull] Transform container)
                {
                    _rigidbody2D = container.GetOrAddComponent<ProxyUnityRb2D>(); // 因此必定有rb2d

                    // GroundChecker
                    _bottomObject = container.GetFirstComponentInChildren<Transform>("GroundChecker");
                }
            }
        }

        [Serializable]
        private class FlipChecker : IComponent
        {
            public bool IsFacingRight { get; private set; }
            private ProxyUnityRb2D _rigidbody2D;

            public FlipChecker(Transform transform, bool isFacingRight = false)
            {
                _rigidbody2D = transform.GetOrAddComponent<ProxyUnityRb2D>();
                IsFacingRight = isFacingRight;
            }

            /// 轉向
            public void Flip()
            {
                IsFacingRight = !IsFacingRight;
                _rigidbody2D.transform.localScale =
                    Vector3.Scale(_rigidbody2D.transform.localScale, new Vector3(-1, 1, 1));
            }

            public EnumComponentTag Tag => EnumComponentTag.FlipChecker;

            public void Update()
            {
                if (_rigidbody2D.ActiveX + _rigidbody2D.GuideX > 0.1f && !IsFacingRight)
                    Flip(); //往左翻

                if (_rigidbody2D.ActiveX + _rigidbody2D.GuideX < -0.1f && IsFacingRight)
                    Flip(); //往右翻
            }
        }
    }
}