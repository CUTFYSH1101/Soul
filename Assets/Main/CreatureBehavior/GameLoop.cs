using Main.Util;
using Main.Entity.Creature;
using Main.EventLib.Sub;
using Main.EventLib.Sub.BattleSystem;
using Main.Game.Collision;
using Main.Input;
using Test;
using UnityEngine;

namespace Main.CreatureBehavior
{
    public class GameLoop : MonoBehaviour
    {
        public Transform player, monster;

        private GUILog _gui;

        private Creature _demoPlayer2;
        public void Awake()
        {
            CreatureSystem.Instance.Init();
            _demoPlayer2 = BattleInterface.FindCreature(player);
            CollisionManager.ThreeLevelGroundSetting(); // 三層地板

            _gui = new GUILog();
        }
        private void Update()
        {
            if (Input.Input.GetButtonDown(HotkeySet.DebugMode))
                DebugMode.IsOpen = !DebugMode.IsOpen;
            if (Input.Input.GetButtonDown(HotkeySet.QuitGame))
                Application.Quit();
            CreatureSystem.Instance.Update();
        }

        private void OnGUI()
        {
            var subject = _demoPlayer2;
            _gui.ShowTextOnScene(
                $"測試：\n" +
                $"{subject.CreatureAttr.MindState.ToString()}\n" +
                $"{subject.CreatureAttr.CurrentSkill.ToString()}\n" +
                $"grounded: {(subject.CreatureAttr.Grounded ? "Yes" : "No")}\n" +
                $"CanNotControl: {subject.CreatureAttr.CanNotControlled}\n" +
                $"debuff: {subject.CreatureAttr.CurrentDeBuffs.EnumArrayToString()}\n" +
                $"ThreadSystem: {subject.ThreadSystem}");
        }
    }
}