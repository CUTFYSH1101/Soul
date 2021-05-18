using System;
using System.Linq;
using Main.Util;
using UnityEngine;

namespace Main.Entity.Controller
{
    public class IdleAIState : IAIState
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Update()
        {
            aiStrategy.IdleUpdate();
        }
    }
}