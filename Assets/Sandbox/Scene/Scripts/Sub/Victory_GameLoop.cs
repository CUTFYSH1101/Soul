using System;
using Main.EventLib;
using Main.EventLib.Sub;
using Main.EventLib.Sub.BattleSystem;
using Main.Game.Collision;
using Main.Input;
using Main.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Input = Main.Input.Input;

namespace Test.Scene.Scripts.Sub
{
    public class Victory_GameLoop : MonoBehaviour
    {
        private static Vector2[] _posArr;
        private static bool _flag;
        private void OnEnable()
        {
            CreatureSystem.Init();
            CollisionManager.ThreeLevelGroundSetting(); // 三層地板
            if (_flag)
            {
                var creatureArr = CreatureSystem.Creatures;
                for (var i = 0; i < _posArr.Length; i++) creatureArr[i].Transform.position = _posArr[i];
                _flag = false;
            }
        }

        private void OnDisable()
        {
            CreatureSystem.Destroy();
        }

        private void Update()
        {
            if (Input.GetButtonDown(HotkeySet.DebugMode))
                DebugMode.IsOpen = !DebugMode.IsOpen;
            if (Input.GetButtonDown(HotkeySet.QuitGame))
                Application.Quit();
            if (Input.GetButtonDown(HotkeySet.Reset))
            {
                _flag = true;
                _posArr = CreatureSystem.Creatures.Get(creature => creature.AbsolutePosition);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            CreatureSystem.Instance.Update();
        }
    }
}