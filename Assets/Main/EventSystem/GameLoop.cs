using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.EventSystem.Cause;
using Main.Input;
using Main.Util;
using UnityEngine;
using Time = Main.Util.Time;
using UnityInput = UnityEngine.Input;

namespace Main.EventSystem
{
    public class GameLoop : MonoBehaviour
    {
        /*
        private ConcreteEventA eventA = new ConcreteEventA(2);
        private ConcreteEventB eventB = new ConcreteEventB();

        private ConcreteEventC eventC = new ConcreteEventC(() => Debug.Log("Before Enter"),
            () => Debug.Log("After Exit"));
            */

        public DictionaryAudioPlayer audioPlayer;
        public Transform player;

        public Color color;
        private (GUIStyle style, Rect rect) _gui;
        private DemoPlayer _demoPlayer1;

        public void Awake()
        {
            _demoPlayer1 = new DemoPlayer(player, audioPlayer);
            _gui.style = new GUIStyle {fontSize = 32, normal = {textColor = color}};
            _gui.rect = new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.25f, Screen.width * 0.25f);
        }

        private void Update()
        {
            _demoPlayer1.Update();
            if(UnityInput.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        private void OnGUI()
        {
            // GUI.color = color;
            GUI.Label(_gui.rect, $"測試：\n" +
                                $"{_demoPlayer1.CreatureAttr.MindState.ToString()}\n" +
                                $"{_demoPlayer1.CreatureAttr.CurrentSkill.ToString()}\n" +
                                $"grounded: {(_demoPlayer1.CreatureAttr.Grounded ? "Yes" : "No")}\n" +
                                $"CanNotControl: {_demoPlayer1.CreatureAttr.CanNotControlled()}\n" +
                                $"debuff: {_demoPlayer1.CreatureAttr.CurrentDeBuffs.EnumArrayToString()}\n" +
                                $"ThreadSystem: {_demoPlayer1.ThreadSystem}", _gui.style);
        }
    }
}