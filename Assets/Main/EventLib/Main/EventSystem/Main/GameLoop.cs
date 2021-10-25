using System;
using Main.Util;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Test;
using UnityEngine;

namespace Main.EventLib.Main.EventSystem.Main
{
    public class GameLoop : MonoBehaviour
    {
        private class TestOrderbyEvent : AbsEventObject, IEvent3, IWorkOnCreature
        {
            public TestOrderbyEvent(Creature creature)
            {
                this.Build(creature, EnumOrder.DeBuff, EnumCreatureEventTag.Debuff);
                FinalAct += () => Debug.Log("TestOrderByEvent FinalAct");
            }

            public void Invoke()
            {
                Director.CreateEvent();
            }

            public void Enter()
            {
                Debug.Log("TestOrderByEvent Enter");
            }

            public void Update()
            {
                Debug.Log("TestOrderByEvent Update");
            }

            public void Exit()
            {
                Debug.Log("TestOrderByEvent Exit");
            }

            public Func<bool> FilterIn { get; }
            public Func<bool> ToInterrupt { get; }
            public CreatureInterface CreatureInterface { get; set; }
        }

        class TestDeBuff : AbsEventObject, IEvent2
        {
            public TestDeBuff()
            {
                this.Build();
            }

            public void Invoke()
            {
                (Director as EventBuilder.Director).Builder.PreCreateCheck().LogLine();
                Director.CreateEvent();
            }

            public void Enter()
            {
                Debug.Log("TestDeBuff Enter");
            }

            public void Exit()
            {
                Debug.Log("TestDeBuff Exit");
            }

            public Func<bool> FilterIn { get; }
            public Func<bool> ToInterrupt { get; }
        }

        private TestOrderbyEvent _orderbyEvent;
        private TestDeBuff _testDeBuff;
        public Transform player;
        private Creature _creature;
        private IComponent _creatureThreadSystem;
        private GUILog _gui;

        private void Start()
        {
            _creature = new Creature(player, new CreatureAttr());
            _creatureThreadSystem = _creature.FindByTag(EnumComponentTag.CreatureThreadSystem);
            Debug.Log(_creatureThreadSystem.ToString());
            _gui = new GUILog();
            Log();
        }

        private void Log()
        {
            // _creature = new Creature(player, new CreatureAttr());
            // creature.AppendComponent(new CreatureThreadSystem());
            /*Debug.Log("orderByEvent--");
            orderByEvent = new TestOrderByEvent(_creature);
            orderByEvent.Invoke();*/
            Debug.Log("_testDeBuff--");
            _testDeBuff = new TestDeBuff();
            _testDeBuff.Invoke();
        }

        private void Update()
        {
            _creature.Update();
            if (UnityEngine.Input.anyKeyDown)
            {
                Log();
            }
        }

        private void OnGUI()
        {
            _gui.ShowTextOnScene($"{_creatureThreadSystem.ToString()}");
        }
    }
}