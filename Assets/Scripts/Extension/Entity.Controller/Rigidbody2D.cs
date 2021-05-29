using System;
using Main.Common;
using Main.Util;
using UnityEngine;

/*
 * 繼承/使用順序：
 * CustomRigidbody2D > CreatureController > PlayerController
 * 如有异型種，請覆寫CreatureController(不能動，不能擊退)
 */
namespace Main.Entity.Controller
{
    public class Rigidbody2D : MonoBehaviour
    {
        private float VelocityX => moveX + relateVelocityX + moveToX;
        private float moveX;
        private float relateVelocityX;
        private float moveToX;

        public void SetMoveX(float value) => moveX = value;
        public float GetMoveX() => moveX;
        public void StopForceX() => relateVelocityX = 0;

        /// 取得Rigidbody元件
        public UnityEngine.Rigidbody2D Instance { get; private set; }

        private void Awake()
        {
            Instance = transform.GetOrAddComponent<UnityEngine.Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            float drag = 1.0f - Instance.drag * Time.fixedDeltaTime;
            if (drag < 0.0f) drag = 0.0f;
            relateVelocityX *= drag;
            moveX *= drag;

            /*var drag = Instance.drag * time.fixedDeltaTime;
            if (relateVelocityX != Values.Zero)
                relateVelocityX.Less(drag * relateVelocityX);
            if (moveX != Values.Zero)
                moveX.Less(drag * moveX);*/
            Instance.velocity = new Vector2(VelocityX, Instance.velocity.y);
        }

        /// 給予一個力，使其前進
        public void AddForce(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force)
        {
            if (mode2D == ForceMode2D.Impulse)
            {
                relateVelocityX += force.x / Instance.mass;
                Instance.AddForce(Vector2.up * force.y, mode2D);
            }
            else if (mode2D == ForceMode2D.Force)
            {
                // v = f*t/m
                relateVelocityX += force.x / Instance.mass * Time.fixedDeltaTime;
                Instance.AddForce(Vector2.up * force.y, mode2D);
            }
        }

        /// 給予一個速度，相當自定義AddForce但無視質量
        public void AddVelocity(Vector2 append)
        {
            // => _rigidbody2D.velocity += append;
            relateVelocityX += append.x;
            Instance.velocity += Vector2.up * append.y;
        }

        /// 位移到目標點。用Rigidbody2D.MovePosition
        public void MoveTo(Transform target, float moveSpeed = 5)
        {
            // 計算距離
            var distance = target.position - transform.position;
            // 當到達位置則停止移動
            if (distance.sqrMagnitude < Values.Min) return;
            // 開始移動
            var next = transform.position + distance.normalized * moveSpeed * Time.deltaTime;
            movingRight = distance.x > 0;
            Instance.MovePosition(next);
        }

        /// 位移到目標點。用transform.position
        public void MoveTo(float targetTransformX, float moveSpeed = 5)
        {
            // 計算距離
            var distance = targetTransformX - transform.position.x;
            // 當到達位置則停止移動
            if (Math.Abs(distance) < Values.Min) return;
            // 開始移動
            var append = Math.Sign(distance) * moveSpeed * Time.deltaTime;
            movingRight = append > 0;
            transform.position += new Vector3(append, 0, 0);
        }

        /// 搭配moveTo判斷角色位置往左或往右移動
        public bool movingRight;
    }
}
/*
 * TODO:周一debug完畢之後更改
 * 一共三種控制子分流：moveX,moveTo,addForce
 * 
 * moveTo無法與任一共存，當發生衝突時，採用以下解決方法：
 * 當使用moveTo,則moveX歸零(無法使用角色控制)
 * 當moveX再次有變動(代表玩家操控介入)，則moveTo停止
 * 當使用addForce,則moveTo停止
 * 能夠使用moveTo的時機在addForce之後
 * 
 * addForce與moveX可以共存(代表玩家可以抵禦著攻擊前進、抵禦暴風雪前進)
*/