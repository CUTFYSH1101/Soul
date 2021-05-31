using System;
using Main.Entity.Controller;
using Main.Util;
using Test2.Causes;
using Test2.Timers;
using UnityEngine;

namespace Test2
{
    public class Tester : MonoBehaviour
    {
        private bool anyKeyDown => UnityEngine.Input.anyKeyDown;
        private CDMethod cdMethod1;
        private CDCause cdCause;
        private CDTimer cdTimer;
        private DBClick dbClick;
        private SingleClick singleClick;

        private ICause collision;
        private Sample example;

        private void Awake()
        {
            cdMethod1 = new CDMethod(this, 2);
            cdCause = new CDCause(10);
            // cdSkill.SetExitCause = new CDCause(this, 4);
            cdTimer = new CDTimer(2, Stopwatch.Mode.LocalGame);
            dbClick = new DBClick("Fire1", 1);
            singleClick = new SingleClick("Fire1", 1);

            collision = new ComponentCollision<Transform>(transform, 0.1f,
                LayerMask.NameToLayer("Ground").ToLayerMask());
            example = new Sample(this);
        }

        private void Update()
        {
            if (anyKeyDown)
            {
                // example.Invoke();
            }

            /*if (collision.Cause())
            {
                Debug.Log("VAR");
            }*/

            /*if (dbClick.Cause())
            {
                "雙擊事件".LogLine();
            }

            if (singleClick.Cause())
            {
                "單擊事件".LogLine();
            }

            if (anyKeyDown)
            {
                // Debug.Log(cdTimer.IsTimeUp);
                // if (cdTimer.IsTimeUp)
                // {
                //     cdTimer.Reset();
                // }
                // if (cdCause.Cause())
                // {
                //     cdCause.Reset();
                //     Debug.Log("VAR");
                // }
                // Debug.Log($"dbClick:{dbClick.Cause()}\n" +
                //           $"click:{singleClick.Cause()}");
                // Debug.Log(dbClick.Cause());
            }

            if (anyKeyDown)
            {
                // cdSkill.Invoke();
                // cdMethod1.Invoke();
                // Debug.Log(cdCause?.Cause() == true);
                // cdCause.Reset();
            }*/
        }

    }
}