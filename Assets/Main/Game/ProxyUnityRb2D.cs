using System;
using UnityEngine;

namespace Main.Game
{
    public class ProxyUnityRb2D : MonoBehaviour
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
        public bool Enable { get; set; } = true;
        public bool SwitchActiveX { get; set; } = true;

        /// 模擬動摩擦力與空氣阻力
        public bool SwitchPhysicReduceSimulateX { get; set; } = true;

        public bool FasterByTime { get; set; } = false;

        public float Drag
        {
            get => instance.drag;
            set
            {
                instance.drag = value;
                _activeX.Drag = value;
                _passiveX.Drag = value;
                _guideX.Drag = value;
            }
        }

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
            if (!Enable) return;

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
                1 + Util.Time.DeltaTime;
        }

        private float _coeffFasterMultiplier;

        private void FixedUpdate()
        {
            if (!Enable) return;
            
            if (SwitchPhysicReduceSimulateX)
            {
                _activeX.Update();
                _passiveX.Update();
                _guideX.Update();
            }

            Physics2D.DragSimulate(instance.drag, ref _activeX.Value);
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

        // 搭配guildDir，不能與activeX同時使用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="moveSpeed">移動速度</param>
        /// <param name="jumpForce">跳躍力</param>
        /// <param name="tryJumpIfAboveDifferenceHeight">座標X軸到達目標位置後，如果座標Y軸相差該數值，會跳起，試圖站上平台</param>
        public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50,
            float tryJumpIfAboveDifferenceHeight = 0.15f)
            => _guideX.MoveTo(target, moveSpeed, jumpForce, tryJumpIfAboveDifferenceHeight);

        public void Jump(float jumpForce)
            => _guideX.Jump(jumpForce);

        private class Vector
        {
            private float _dragMultiplier;

            public float Drag
            {
                get => (_dragMultiplier - 1) / Util.Time.DeltaTime;
                set
                {
                    _dragMultiplier = (1 - value * Util.Time.DeltaTime);
                    if (_dragMultiplier > 1) _dragMultiplier = 1;
                }
            }

            protected readonly ProxyUnityRb2D Rb;

            // private readonly Collider2D collider2D;
            private readonly Physics2D.PhysicsDataInterface _physics;

            public float Value;
            // public void Stop() => Value = 0;


            public Vector(ProxyUnityRb2D rb, Collider2D collider2D, float drag)
            {
                // Debug.Log("前"+this.GetType().SkillName+" "+dragMultiplier+" "+drag);

                Drag = drag;
                /*
                _dragMultiplier = (1 - drag * Time.DeltaTime);
                if (_dragMultiplier > 1) _dragMultiplier = 1;
                */

                Rb = rb;
                // this.collider2D = collider2D;
                _physics = new Physics2D.PhysicsDataInterface(rb, collider2D);
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
                Physics2D.FrictionSimulate(_physics, ref Value);
            }

            public void OnCollisionEnter(Collision2D other)
                => Physics2D.CollisionSimulate(_physics, ref Value, other);
        }

        private class GuildVec : Vector
        {
            public Vector2 Direction;
            private bool _isJumping;
            private bool _hasStopMoving;

            public GuildVec(ProxyUnityRb2D rb, Collider2D collider2D, float drag, Action stopMoving) :
                base(rb, collider2D, drag)
            {
                _stopMoving = stopMoving;
            }

            public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50,
                float tryJumpIfAboveDifferenceHeight = 0.15f)
            {
                // 停止移動
                StopMoving();

                var difference = target - (Vector2) Rb.transform.position;
                var dir = new Vector2(Math.Sign(difference.x), Math.Sign(difference.y));
                // var dir = difference.normalized;
                // 當到達位置則停止移動
                if (difference.magnitude <= .15f)
                    return;
                // 當目標在平台上，試圖往上跳
                if (Math.Abs(difference.x) <= .15f)
                    if (difference.y >= tryJumpIfAboveDifferenceHeight)
                        Jump(jumpForce); // todo 當目標在下方，試圖往平台下移動
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
        public class PhysicsDataInterface
        {
            private readonly ProxyUnityRb2D _rb;
            private readonly Collider2D _collider2D;
            public Vector2 Velocity => _rb.instance.velocity;
            public Vector2 Position => _rb.transform.position;
            public float GravityScale => _rb.instance.gravityScale;
            public bool UseGravity => GravityScale != 0;
            public float LinearDrag => _rb.instance.drag;
            public PhysicsMaterial2D SharedMaterial => _rb.instance.sharedMaterial;
            public float Bounciness => SharedMaterial != null ? SharedMaterial.bounciness : 0.0f;
            public float Friction => SharedMaterial != null ? SharedMaterial.friction : 0.4f;

            public PhysicsDataInterface(ProxyUnityRb2D rb, Collider2D collider2D)
            {
                _rb = rb;
                _collider2D = collider2D;
            }
        }

        public static void AddForce(this ProxyUnityRb2D rb, ref float subSpeedX, Vector2 force, ForceMode2D mode2D)
        {
            AddForceX(rb, ref subSpeedX, force.x, mode2D); // x 軸方向
            rb.instance.AddForce(new Vector2(0, force.y), mode2D); // y 軸方向
        }

        public static void AddForceX(this ProxyUnityRb2D rb, ref float subSpeedX, float force,
            ForceMode2D mode2D)
        {
            var mass = rb.instance.mass;
            switch (mode2D)
            {
                case ForceMode2D.Force:
                    // 加速度a = force / mass
                    // v = v0 + a * t, t = 0.02, a = force / mass
                    subSpeedX += force / mass * Util.Time.DeltaTime;
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
        /// <summary>
        /// 讀取物理資料中的空氣阻力與動摩擦力，並修改到subSpeedX上
        /// </summary>
        /// <param name="physicsData"></param>
        /// <param name="subSpeedX"></param>
        public static void FrictionSimulate(PhysicsDataInterface physicsData, ref float subSpeedX)
        {
            // 確認是否擁有重力因素
            if (!physicsData.UseGravity)
                return;

            // 避免開跟號錯誤
            if (Math.Abs(subSpeedX) < .001f)
            {
                subSpeedX = 0;
                return;
            }

            // 確認是否擁有物理材質球
            var friction = physicsData.Friction;

            // 公式
            float magnitude = -UnityEngine.Physics2D.gravity.magnitude * physicsData.GravityScale *
                              friction *
                              Convert.ToSingle(Math.Sqrt(Math.Abs(subSpeedX))) * 0.5f;

            // 避免錯誤
            if (Math.Abs(subSpeedX) + magnitude * Util.Time.DeltaTime < 0)
            {
                subSpeedX = 0;
                return;
            }

            if (subSpeedX > 0)
                subSpeedX += magnitude * Util.Time.DeltaTime;
            else if (subSpeedX < 0)
                subSpeedX -= magnitude * Util.Time.DeltaTime;
        }

        public static void CollisionSimulate(PhysicsDataInterface physicsData, ref float subSpeedX,
            Collision2D other)
        {
            // 確認材質球
            var bounciness = physicsData.Bounciness;

            // 不考慮彈性碰撞、質量差
            // 末速度 = - 彈性係數 * 初速度 * Cos（速度與法向力夾角）
            var normal = (Vector2) other.transform.position - physicsData.Position;
            var angle = Vector2.Angle(normal, physicsData.Velocity);
            subSpeedX *= bounciness * Convert.ToSingle(Math.Sin(angle));
        }

        private static double LN(double value) => Math.Log(value, Math.E);

        private static double GetCommonRatioByLinearDrag(double drag)
        {
            /*
            drag非同一個公式，而是用相近值
            <=0
            ratio = 1
            <=10
            ratio = -0.0047x + 0.9992
            <=100
            ratio = -0.0032x + 0.9704
            <=1000
            ratio = -0.214ln(x) + 1.6283
            <=5000
            ratio = 98.576x^(-0.92)
            <=10000
            ratio = 152.3x^(-0.972)
            >10000
            ratio = -2E-06x + 0.0388
            */
            double ratio; // 最大不超過1，最小不等於0
            if (drag <= 0) ratio = 1;
            else if (drag <= 10) ratio = -0.0047 * drag + 0.9992;
            else if (drag <= 100) ratio = -0.0032 * drag + 0.9704;
            else if (drag <= 1000) ratio = -0.214 * LN(drag) + 1.6283;
            else if (drag <= 5000) ratio = 98.576 * Math.Pow(drag, -0.92);
            else if (drag <= 10000) ratio = 152.3 * Math.Pow(drag, -0.972);
            else ratio = -0.000002 * drag + 0.0388;
            return ratio;
        }

        private static float GetCommonRatioByLinearDrag(float drag)
        {
            return (float) GetCommonRatioByLinearDrag((double) drag);
        }

        public static void DragSimulate(float drag, ref float subSpeedX)
        {
            if (Math.Abs(subSpeedX) < .001f)
            {
                subSpeedX = 0;
                return;
            }

            var ratio = GetCommonRatioByLinearDrag(drag);
            // subSpeedX *= ratio * Time.DeltaTime;
            subSpeedX *= ratio;
        }
        // todo 待完成
        /*
        public class TimeScaleSimulateEvent
        {
            private float _timeScale = 1;
            public Func<float> OriginSubSpeedX { get; set; }

            public float TimeScale
            {
                get => _timeScale;
                set
                {
                    _timeScale = value;
                    if (_timeScale > 1) _timeScale = 1;
                    else if (_timeScale < 0) _timeScale = 0;
                }
            }

            public float Invoke(float timeScale = 1)
            {
                // 確認timeScale數值
                if (timeScale > 1) timeScale = 1;
                else if (timeScale < 0) timeScale = 0;

                return OriginSubSpeedX() * timeScale;
            }
            /*public float Invoke(float timeScale = 1)
            {
                // 確認timeScale數值
                if (timeScale > 1) timeScale = 1;
                else if (timeScale < 0) timeScale = 0;

                return OriginSubSpeedX() * timeScale;
            }#1#
        }
    */
    }
}
/*
△t = 0.005，單位(s)，t(n+1)-tn = 0.005 (s)
速率v，單位(m/s)，當速率 <= 0.001 m/s，自動歸0
觀察結論
1.已知每欄前後幀速度公比固定，令公比為a，且0<a<=1
2.已知公比a只與空氣阻力drag相關，不考慮初速度、重力等其他因素
3.公比a，0<a<=1，當drag為1，則a=1，drag越大，a越小，極限0但不等於0

假設a = 0.96，v0 = 10 m/s
則v0 = 10, v1 = 9.6, v2 = 9.216, v3 = 8.84736, 以此類推
假設n = 0.96，v0 = 10 (m/s)，t0 = 0, t1 = 0.05, t2 = 0.1, t3 = 0.15...
*/
/*
drag非同一個公式，而是用相近值
<=0
ratio = 1
>10
ratio = -0.0047x + 0.9992
10~100
ratio = -0.0032x + 0.9704
100~1000
ratio = -0.214ln(x) + 1.6283
1000~5000
ratio = 98.576x-0.92
5000~10000
ratio = 152.3x-0.972
>10000
ratio = -2E-06x + 0.0388
*/