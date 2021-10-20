using System;
using Main.Game;
using Main.Game.Collision;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Main.AnimCinemachine
{
    public class CharacterMovement2D : MonoClass
    {
        public bool checkGroundForJump = true;
        public Transform groundChecker;
        public int moveSpeed = 5;
        public int jumpForce = 5;
        public int rotateForce = 50;
        private (Animator anim, Rigidbody2D rigbody) _obj;
        private float _speedX;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Move = Animator.StringToHash("Move");

        private void Start()
        {
            _obj.anim = this.GetOrAddComponent<Animator>();
            _obj.rigbody = this.GetOrAddComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (UnityInput.GetButton("Horizontal") && IsGrounded())
            {
                _speedX = UnityInput.GetAxisRaw("Horizontal") * moveSpeed;
                // _obj.rigbody.AddTorque(-UnityInput.GetAxisRaw("Horizontal") * rotateForce);
                _obj.anim.SetInteger(Speed,Math.Sign(UnityInput.GetAxisRaw("Horizontal")));
                _obj.anim.SetBool(Move,true);
            }
            else if (IsGrounded())
            {
                _speedX = 0;
                _obj.anim.SetInteger(Speed,0);
                _obj.anim.SetBool(Move,false);
            }

            if (UnityInput.GetKeyDown(KeyCode.Space) && IsGrounded())
                _obj.rigbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);


            _obj.rigbody.velocity = new Vector2(_speedX, _obj.rigbody.velocity.y);
        }

        private bool IsGrounded() =>
            !checkGroundForJump || groundChecker.IsGrounded();
    }
}