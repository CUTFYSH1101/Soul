using System;
using Main.Util;
using UnityEngine;
using Convert = System.Convert;
using Math = System.Math;
using MyRigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity.Controller
{
    public class Rigidbody2D : MonoBehaviour
    {
        public Vector2 Velocity
        {
            get => instance.velocity;
            private set
            {
                activeX.Value = value.x; // 更改虛擬速度
                instance.velocity = value; // 減少SpeedX取值延遲時間
            }
        }

        private float SpeedX => activeX.Value + passiveX.Value + guideX.Value;
        public UnityEngine.Rigidbody2D instance;
        public new Collider2D collider2D;

        private Vector activeX, passiveX;
        private GuildVec guideX;
        /// 包含衝刺、奔跑、攻擊...等主動行為
        public void SetActiveX(float value) => activeX.Value = value;
        /// 包含衝刺、奔跑、攻擊...等主動行為
        public float GetActiveX() => activeX.Value;
        /// 受到對方攻擊...等被動行為
        public void SetPassiveX(float value) => passiveX.Value = value;
        /// 受到對方攻擊...等被動行為
        public float GetPassiveX() => passiveX.Value;
        /// navigation
        public void SetGuideX(float value) => guideX.Value = value;
        /// navigation
        public float GetGuideX() => guideX.Value;

        private void Awake()
        {
            instance = this.GetOrAddComponent<UnityEngine.Rigidbody2D>();
            collider2D = this.GetOrAddComponent<Collider2D>();
            var drag = instance.drag;
            activeX = new Vector(this, collider2D, drag);
            passiveX = new Vector(this, collider2D, drag);
            guideX = new GuildVec(this, collider2D, drag,
                () =>
                {
                    // 停止移動
                    Velocity = Vector2.zero;
                    activeX.Value = 0;
                    passiveX.Value = 0;
                });
        }

        private void FixedUpdate()
        {
            activeX.Update();
            passiveX.Update();
            guideX.Update();
            instance.velocity = new Vector2(SpeedX, instance.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            activeX.OnCollisionEnter(other);
            passiveX.OnCollisionEnter(other);
            guideX.OnCollisionEnter(other);
            instance.velocity = new Vector2(SpeedX, instance.velocity.y);
        }

        public void AddForce_OnActive(float x, float y, ForceMode2D mode2D = ForceMode2D.Force)
            => AddForce_OnActive(new Vector2(x, y), mode2D);

        public void AddForce_OnActive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force)
            => this.AddForce(ref activeX.Value, force, mode2D);
        
        public void AddForce_OnPassive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force)
            => this.AddForce(ref passiveX.Value, force, mode2D);       

        // 移動方向
        public Vector2 GuildDir => guideX.Direction;

        // 搭配guildDir
        public void MoveTo(Vector2 target, float moveSpeed = 5, float jumpForce = 50)
            => guideX.MoveTo(target, moveSpeed, jumpForce);

        public void Jump(float jumpForce)
            => guideX.Jump(jumpForce);

        private class Vector
        {
            private readonly float dragMultiplier;
            protected readonly Rigidbody2D Rb;
            // private readonly Collider2D collider2D;
            private readonly Physics2D.BundleData bundle;

            public float Value;
            // public void Stop() => Value = 0;


            public Vector(Rigidbody2D rb, Collider2D collider2D, float drag)
            {
                // Debug.Log("前"+this.GetType().SkillName+" "+dragMultiplier+" "+drag);

                dragMultiplier = (1 - drag * Time.fixedDeltaTime);
                if (dragMultiplier > 1)
                {
                    dragMultiplier = 1;
                }

                this.Rb = rb;
                // this.collider2D = collider2D;
                bundle = new Physics2D.BundleData(rb, collider2D);
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
                if (0 < dragMultiplier && dragMultiplier < 1)
                    Value *= dragMultiplier;
                // 模擬摩擦
                Physics2D.FrictionSimulate(bundle, ref Value);
            }

            public void OnCollisionEnter(Collision2D other)
                => Physics2D.CollisionSimulate(bundle, ref Value, other);
        }

        private class GuildVec : Vector
        {
            public Vector2 Direction;
            private bool isJumping;
            private bool hasStopMoving;

            public GuildVec(Rigidbody2D rb, Collider2D collider2D, float drag, Action stopMoving) : 
                base(rb, collider2D, drag)
            {
                this.stopMoving = stopMoving;
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
                if (!isJumping)
                {
                    isJumping = true;
                    Rb.AddForce_OnActive(0, jumpForce);
                }

                if (isJumping && Math.Abs(Rb.Velocity.y) < .001)
                {
                    isJumping = false;
                }
            }

            private void StopMoving()
            {
                if (hasStopMoving)
                    return;
                hasStopMoving = true;
                stopMoving?.Invoke();
            }

            private readonly Action stopMoving;
        }

        public override string ToString()
        {
            return $"{GetType().Name}\n" +
                   $"總速度1=\t{Velocity}\n" +
                   $"總速度2=\t{new Vector2(SpeedX,Velocity.y)}\n" +
                   $"主動速度X=\t{activeX.Value}\n" +
                   $"被動速度X=\t{passiveX.Value}\n" +
                   $"引導速度X=\t{guideX.Value}";
        }
    }

    public static class Physics2D
    {
        public class BundleData
        {
            private readonly MyRigidbody2D rb;
            private readonly Collider2D collider2D;
            public Vector2 Velocity => rb.instance.velocity;
            public Vector2 Position => rb.transform.position;
            public float GravityScale => rb.instance.gravityScale;
            public bool UseGravity => GravityScale != 0;
            public PhysicsMaterial2D SharedMaterial => rb.instance.sharedMaterial;
            public float Bounciness => SharedMaterial != null ? SharedMaterial.bounciness : 0.0f;
            public float Friction => SharedMaterial != null ? SharedMaterial.friction : 0.4f;

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
                subSpeedX += magnitude * Time.fixedDeltaTime;
            else if (subSpeedX < 0)
                subSpeedX -= magnitude * Time.fixedDeltaTime;
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