using Main.Util;
using Main.Input;
using Main.Res.Script.Audio;
using Test;
using UnityEngine;

namespace Main.EventLib
{
    public class GameLoop : MonoBehaviour
    {
        /*
        private ConcreteEventA eventA = new ConcreteEventA(2);
        private ConcreteEventB eventB = new ConcreteEventB();

        private ConcreteEventC eventC = new ConcreteEventC(() => Debug.Log("Before Enter"),
            () => Debug.Log("After Exit"));
            */

        public DictAudioPlayer audioPlayer;
        private GUILog _gui;
        public Transform player;
        private DemoPlayer _demoPlayer1;

        public void Awake()
        {
            _demoPlayer1 = new DemoPlayer(player, audioPlayer);
            _gui = new GUILog();
        }

        private void Update()
        {
            if (Input.Input.GetButtonDown(HotkeySet.QuitGame))
                Application.Quit();

            _demoPlayer1.Update();
        }

        private void OnGUI() =>
            _gui.ShowTextOnScene(
                $"測試：\n" +
                $"{_demoPlayer1.CreatureAttr.MindState.ToString()}\n" +
                $"{_demoPlayer1.CreatureAttr.CurrentSkill.ToString()}\n" +
                $"grounded: {(_demoPlayer1.CreatureAttr.Grounded ? "Yes" : "No")}\n" +
                $"CanNotControl: {_demoPlayer1.CreatureAttr.CanNotControlled}\n" +
                $"debuff: {_demoPlayer1.CreatureAttr.CurrentDeBuffs.EnumArrayToString()}\n" +
                $"ThreadSystem: {_demoPlayer1.ThreadSystem}");
    }
}