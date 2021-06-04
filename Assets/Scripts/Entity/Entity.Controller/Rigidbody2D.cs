using System;
using Main.Util;
using UnityEngine;
using Convert = System.Convert;
using MyRigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity.Controller
{
    public class Rigidbody2D : MonoBehaviour
    {
        public Vector2 velocity
        {
            get => instance.velocity;
            set
            {
                _activeX.value = value.x; // 更改虛擬速度
                instance.velocity = value; // 減少SpeedX取值延遲時間
            }
        }

        private float SpeedX => _activeX.value + _passiveX.value + _guideX.value;
        public UnityEngine.Rigidbody2D instance;
        public Collider2D collider2D;

        private Vector _activeX, _passiveX;
        private GuildVec _guideX;
        /// 包含衝刺、奔跑、攻擊...等主動行為
        public void SetActiveX(float value) => _activeX.value = value;
        /// 包含衝刺、奔跑、攻擊...等主動行為
        public float GetActiveX() => _activeX.value;
        /// 受到對方攻擊...等被動行為
        public float GetPassiveX() => _passiveX.value;
        public float GetGuideX() => _guideX.value;

        private void Awake()
        {
            instance = this.GetOrAddComponent<UnityEngine.Rigidbody2D>();
            collider2D = this.GetOrAddComponent<Collider2D>();
            gameObject.AddComponent<Collider2D>();
            var drag = instance.drag;
            _activeX = new Vector(this, collider2D, drag);
            _passiveX = new Vector(this, collider2D, drag);
            _guideX = new GuildVec(this, collider2D, drag,
                () =>
                {
                    // 停止移動
                    velocity = Vector2.zero;
                    _activeX.value = 0;
                    _passiveX.value = 0;
                });
        }

        private void FixedUpdate()
        {
            _activeX.Update();
            _passiveX.Update();
            _guideX.Update();
            instance.velocity = new Vector2(SpeedX, instance.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _activeX.OnCollisionEnter(other);
            _passiveX.OnCollisionEnter(other);
            _guideX.OnCollisionEnter(other);
            instance.velocity = new Vector2(SpeedX, instance.velocity.y);
        }

        public void AddForce_OnActive(float x, float y, ForceMode2D mode2D = ForceMode2D.Force)
            => AddForce_OnActive(new Vector2(x, y), mode2D);

        public void AddForce_OnActive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force)
            => this.AddForce(ref _activeX.value, force, mode2D);
        
        public void AddForce_OnPassive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force)
            => this.AddForce(ref _passiveX.value, force, mode2D);       

        // 移動方向
        public Vector2 guildDir => _guideX.direction;

        // 搭配guildDir
        public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50)
            => _guideX.MoveTo(target, moveSpeed, jumpForce);

        public void Jump(float jumpForce)
            => _guideX.Jump(jumpForce);

        private class Vector
        {
            private readonly float _dragMultiplier;
            protected readonly Rigidbody2D _rb;
            protected readonly Collider2D _collider2D;
            protected readonly Physic2D.BundleData bundle;

            public float value;
            public void Stop() => value = 0;


            public Vector(Rigidbody2D rb, Collider2D collider2D, float drag)
            {
                _dragMultiplier = (1 - drag * Time.fixedTime);
                if (_dragMultiplier > 1)
                {
                    _dragMultiplier = 1;
                }

                this._rb = rb;
                _collider2D = collider2D;
                bundle = new Physic2D.BundleData(rb, collider2D);
            }

            public void Update()
            {
                if (Math.Abs(value) < .001f)
                {
                    value = 0;
                    return;
                }

                // 模擬阻力
                if (0 < _dragMultiplier && _dragMultiplier < 1)
                    value *= _dragMultiplier;
                // 模擬摩擦
                Physic2D.FrictionSimulate(bundle, ref value);
            }

            public void OnCollisionEnter(Collision2D other)
                => Physic2D.CollisionSimulate(bundle, ref value, other);
        }

        private class GuildVec : Vector
        {
            public Vector2 direction;
            private bool isJumping;
            private bool hasStopMoving;

            public GuildVec(Rigidbody2D rb, Collider2D collider2D, float drag, Action stopMoving) : base(rb, collider2D,
                drag)
            {
                this._stopMoving = stopMoving;
            }

            public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50)
            {
                // 停止移動
                StopMoving();

                var difference = target - (Vector2) _rb.transform.position;
                var dir = difference.normalized;
                // 當到達位置則停止移動
                if (difference.magnitude <= .15f)
                    return;
                // 當目標在平台上，試圖往上跳
                if (Math.Abs(difference.x) <= .15f)
                    if (difference.y >= .15f)
                        Jump(jumpForce);
                // 開始移動
                value = dir.x * moveSpeed;
                // 主動移動方向
                direction = dir;
            }

            public void Jump(float jumpForce)
            {
                if (!isJumping)
                {
                    isJumping = true;
                    _rb.AddForce_OnActive(0, jumpForce);
                }

                if (isJumping && Math.Abs(_rb.velocity.y) < .001)
                {
                    isJumping = false;
                }
            }

            private void StopMoving()
            {
                if (hasStopMoving)
                    return;
                hasStopMoving = true;
                _stopMoving?.Invoke();
            }

            private readonly Action _stopMoving;
        }
    }

    public static class Physic2D
    {
        public class BundleData
        {
            private readonly MyRigidbody2D rb;
            private readonly Collider2D collider2D;
            public Vector2 velocity => rb.instance.velocity;
            public Vector2 position => rb.transform.position;
            public float gravityScale => rb.instance.gravityScale;
            public bool useGravity => gravityScale != 0;
            public PhysicsMaterial2D sharedMaterial => rb.instance.sharedMaterial;
            public float bounciness => sharedMaterial != null ? sharedMaterial.bounciness : 0.0f;
            public float friction => sharedMaterial != null ? sharedMaterial.friction : 0.4f;

            public BundleData(Rigidbody2D rb, Collider2D collider2D)
            {
                this.rb = rb;
                this.collider2D = collider2D;
            }
        }

        public static void AddForce(this MyRigidbody2D rb, ref float subSpeedX, Vector2 force, ForceMode2D mode2D)
        {
            AddForceX(rb, ref subSpeedX, force.x, mode2D); // x 軸方向
            rb.instance.AddForce(new Vector2(0, force.y), mode2D); // y 軸方向
        }

        public static void AddForceX(this MyRigidbody2D rb, ref float subSpeedX, float force,
            ForceMode2D mode2D)
        {
            var mass = rb.instance.mass;
            switch (mode2D)
            {
                case ForceMode2D.Force:
                    // 加速度a = force / mass
                    // v = v0 + a * t, t = 0.02, a = force / mass
                    subSpeedX += force / mass * Time.fixedDeltaTime;
                    break;
                case ForceMode2D.Impulse:
                    // 速度v = force / mass
                    // v = v0 + a * t, "t = 1", a = force / mass, "t 可以省略"
                    subSpeedX += force / mass;
                    break;
                default:
                    Debug.LogError("超出範圍");
                    break;
            }
        }

        // 還有一點誤差
        public static void FrictionSimulate(BundleData bundleData, ref float subSpeedX)
        {
            // 確認是否擁有重力因素
            if (!bundleData.useGravity)
                return;

            // 避免開跟號錯誤
            if (Math.Abs(subSpeedX) < .001f)
            {
                subSpeedX = 0;
                return;
            }

            // 確認是否擁有物理材質球
            var friction = bundleData.friction;

            // 公式
            float magnitude = -Physics2D.gravity.magnitude * bundleData.gravityScale *
                              friction *
                              Convert.ToSingle(Math.Sqrt(Math.Abs(subSpeedX))) * 0.5f;
            if (subSpeedX > 0)
                subSpeedX += magnitude * Time.fixedDeltaTime;
            else if (subSpeedX < 0)
                subSpeedX -= magnitude * Time.fixedDeltaTime;
        }

        public static void CollisionSimulate(BundleData bundleData, ref float subSpeedX,
            Collision2D other)
        {
            // 確認材質球
            var bounciness = bundleData.bounciness;

            // 不考慮彈性碰撞、質量差
            // 末速度 = - 彈性係數 * 初速度 * Cos（速度與法向力夾角）
            var normal = (Vector2) other.transform.position - bundleData.position;
            var angle = Vector2.Angle(normal, bundleData.velocity);
            subSpeedX *= bounciness * Convert.ToSingle(Math.Sin(angle));
        }
    }
}