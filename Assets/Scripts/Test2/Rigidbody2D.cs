using System;
using Main.Util;
using Test2;
using Test2.Timers;
using UnityEngine;

namespace Test22
{
    public class Rigidbody2D : MonoBehaviour
    {
        public UnityEngine.Rigidbody2D Instance { get; private set; }

        private void Awake()
        {
            Instance = transform.GetOrAddComponent<UnityEngine.Rigidbody2D>();
        }

        class TimeLine : AbstractSkill
        {
            public TimeLine(MonoBehaviour mono, float cdTime = 0.1f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) : base(mono, cdTime, mode)
            {
            }

            protected override void Enter()
            {
            }

            protected override void Update()
            {
            }

            protected override void Exit()
            {
            }
        }
    }
}