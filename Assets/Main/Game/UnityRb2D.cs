using System;
using UnityEngine;
using Time = Main.Util.Time;

namespace Main.Game
{
    public class UnityRb2D : MonoBehaviour
    {
        public Vector2 Velocity
        {
            get => instance.velocity;
            set
            {
                _activeX.Value = value.x; // 更改虛擬速度
                _passiveX.Value = 0;
                _guideX.Value = 0;
                instance.velocity = value; // 減少SpeedX取值延遲時間
            }
        }

        private float SpeedX => _activeX.Value + _passiveX.Value + _guideX.Value;
        public Rigidbody2D instance;
        public new Collider2D collider2D;

        private Vector _activeX, _passiveX;
        private GuildVec _guideX;
        public bool SwitchActiveX { get; set; } = true;

        /// 模擬動摩擦力與空氣阻力
        public bool SwitchPhysicReduceSimulateX { get; set; } = true;

        public bool FasterByTime { get; set; } = false;

        /// 包含衝刺、奔跑、攻擊...等主動行為
        public float ActiveX
        {
            get => _activeX.Value;
            set => _activeX.Value = value;
        }

        /// 受到對方攻擊...等被動行為
        public float PassiveX
        {
            get => _passiveX.Value;
            set => _passiveX.Value = value;
        }

        /// navigation
        public float GuideX
        {
            get => _guideX.Value;
            set => _guideX.Value = value;
        }

        public void ResetAll()
        {
            ResetX();
            ResetY();
            instance.gravityScale = 1;
        }

        public void ResetX()
        {
            _activeX.Value = 0;
            _passiveX.Value = 0;
            _guideX.Value = 0;
        }

        public void ResetY()
        {
            instance.velocity = new Vector2(instance.velocity.x, 0);
        }

        public void UseGravity(bool @switch)
        {
            instance.gravityScale = @switch ? 1 : 0;
        }

        public bool IsMoving => instance.velocity.x != 0 && instance.velocity.y != 0;

        private void Awake()
        {
            instance = this.GetOrAddComponent<Rigidbody2D>();
            collider2D = this.GetOrAddComponent<CapsuleCollider2D>();
            var drag = instance.drag;
            _activeX = new Vector(this, collider2D, drag);
            _passiveX = new Vector(this, collider2D, drag);
            _guideX = new GuildVec(this, collider2D, drag,
                () =>
                {
                    // 停止移動
                    Velocity = Vector2.zero;
                    _activeX.Value = 0;
                    // passiveX.Value = 0; 在暴風雪中行進，可添加一個非常微弱的addForce，並係數乘以Time.DeltaTime
                });

            _coeffFasterMultiplier =
                1 + Time.DeltaTime;
        }

        private float _coeffFasterMultiplier;

        private void FixedUpdate()
        {
            if (SwitchPhysicReduceSimulateX)
            {
                _activeX.Update();
                _passiveX.Update();
                _guideX.Update();
            }

            if (!SwitchActiveX) _activeX.Value = 0;
            instance.velocity = new Vector2(SpeedX, instance.velocity.y);
            if (FasterByTime)
                Velocity *= _coeffFasterMultiplier;
        }
        // instance.velocity += instance.velocity.normalized * Time.DeltaTime;
        // Velocity *= (1 + Time.DeltaTime);

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
            => this.AddForce(ref _activeX.Value, force, mode2D);

        public void AddForce_OnPassive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force)
            => this.AddForce(ref _passiveX.Value, force, mode2D);

        // 移動方向
        public Vector2 GuildDir => _guideX.Direction;

        // 搭配guildDir，不能
        public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50)
            => _guideX.MoveTo(target, moveSpeed, jumpForce);

        public void Jump(float jumpForce)
            => _guideX.Jump(jumpForce);

        private class Vector
        {
            private readonly float _dragMultiplier;

            protected readonly UnityRb2D Rb;

            // private readonly Collider2D collider2D;
            private readonly Physics2D.BundleData _bundle;

            public float Value;
            // public void Stop() => Value = 0;


            public Vector(UnityRb2D rb, Collider2D collider2D, float drag)
            {
                // Debug.Log("前"+this.GetType().SkillName+" "+dragMultiplier+" "+drag);

                _dragMultiplier = (1 - drag * Time.DeltaTime);
                if (_dragMultiplier > 1)
                {
                    _dragMultiplier = 1;
                }

                Rb = rb;
                // this.collider2D = collider2D;
                _bundle = new Physics2D.BundleData(rb, collider2D);
                // Debug.Log("後"+this.GetType().SkillName+" "+dragMultiplier+" "+drag);
            }

            public void Update()
            {
                if (Math.Abs(Value) < .001f)
                {
                    Value = 0;
                    return;
                }

                // 模擬阻力
                if (0 < _dragMultiplier && _dragMultiplier < 1)
                    Value *= _dragMultiplier;
                // 模擬摩擦
                Physics2D.FrictionSimulate(_bundle, ref Value);
            }

            public void OnCollisionEnter(Collision2D other)
                => Physics2D.CollisionSimulate(_bundle, ref Value, other);
        }

        private class GuildVec : Vector
        {
            public Vector2 Direction;
            private bool _isJumping;
            private bool _hasStopMoving;

            public GuildVec(UnityRb2D rb, Collider2D collider2D, float drag, Action stopMoving) :
                base(rb, collider2D, drag)
            {
                _stopMoving = stopMoving;
            }

            public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50)
            {
                // 停止移動
                StopMoving();

                var difference = target - (Vector2) Rb.transform.position;
                var dir = difference.normalized;
                // 當到達位置則停止移動
                if (difference.magnitude <= .15f)
                    return;
                // 當目標在平台上，試圖往上跳
                if (Math.Abs(difference.x) <= .15f)
                    if (difference.y >= .15f)
                        Jump(jumpForce);
                // 開始移動
                Value = dir.x * moveSpeed;
                // 主動移動方向
                Direction = dir;
            }

            public void Jump(float jumpForce)
            {
                if (!_isJumping)
                {
                    _isJumping = true;
                    Rb.AddForce_OnActive(0, jumpForce, ForceMode2D.Impulse);
                }

                if (_isJumping && Math.Abs(Rb.Velocity.y) < .001)
                {
                    _isJumping = false;
                }
            }

            private void StopMoving()
            {
                if (_hasStopMoving)
                    return;
                _hasStopMoving = true;
                _stopMoving?.Invoke();
            }

            private readonly Action _stopMoving;
        }

        public override string ToString()
        {
            return $"{GetType().Name}\n" +
                   $"總速度1=\t{Velocity}\n" +
                   $"總速度2=\t{new Vector2(SpeedX, Velocity.y)}\n" +
                   $"主動速度X=\t{_activeX.Value}\n" +
                   $"被動速度X=\t{_passiveX.Value}\n" +
                   $"引導速度X=\t{_guideX.Value}";
        }
    }

    public static class Physics2D
    {
        public class BundleData
        {
            private readonly UnityRb2D _rb;
            private readonly Collider2D _collider2D;
            public Vector2 Velocity => _rb.instance.velocity;
            public Vector2 Position => _rb.transform.position;
            public float GravityScale => _rb.instance.gravityScale;
            public bool UseGravity => GravityScale != 0;
            public PhysicsMaterial2D SharedMaterial => _rb.instance.sharedMaterial;
            public float Bounciness => SharedMaterial != null ? SharedMaterial.bounciness : 0.0f;
            public float Friction => SharedMaterial != null ? SharedMaterial.friction : 0.4f;

            public BundleData(UnityRb2D rb, Collider2D collider2D)
            {
                _rb = rb;
                _collider2D = collider2D;
            }
        }

        public static void AddForce(this UnityRb2D rb, ref float subSpeedX, Vector2 force, ForceMode2D mode2D)
        {
            AddForceX(rb, ref subSpeedX, force.x, mode2D); // x 軸方向
            rb.instance.AddForce(new Vector2(0, force.y), mode2D); // y 軸方向
        }

        public static void AddForceX(this UnityRb2D rb, ref float subSpeedX, float force,
            ForceMode2D mode2D)
        {
            var mass = rb.instance.mass;
            switch (mode2D)
            {
                case ForceMode2D.Force:
                    // 加速度a = force / mass
                    // v = v0 + a * t, t = 0.02, a = force / mass
                    subSpeedX += force / mass * Time.DeltaTime;
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
            if (!bundleData.UseGravity)
                return;

            // 避免開跟號錯誤
            if (Math.Abs(subSpeedX) < .001f)
            {
                subSpeedX = 0;
                return;
            }

            // 確認是否擁有物理材質球
            var friction = bundleData.Friction;

            // 公式
            float magnitude = -UnityEngine.Physics2D.gravity.magnitude * bundleData.GravityScale *
                              friction *
                              Convert.ToSingle(Math.Sqrt(Math.Abs(subSpeedX))) * 0.5f;
            if (subSpeedX > 0)
                subSpeedX += magnitude * Time.DeltaTime;
            else if (subSpeedX < 0)
                subSpeedX -= magnitude * Time.DeltaTime;
        }

        public static void CollisionSimulate(BundleData bundleData, ref float subSpeedX,
            Collision2D other)
        {
            // 確認材質球
            var bounciness = bundleData.Bounciness;

            // 不考慮彈性碰撞、質量差
            // 末速度 = - 彈性係數 * 初速度 * Cos（速度與法向力夾角）
            var normal = (Vector2) other.transform.position - bundleData.Position;
            var angle = Vector2.Angle(normal, bundleData.Velocity);
            subSpeedX *= bounciness * Convert.ToSingle(Math.Sin(angle));
        }
    }
}