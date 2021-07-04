using System;
using Main.Common;
using Main.Entity.Attr;
using Main.Util;
using UnityEngine;
using static UnityEngine.Input;

namespace Main.Entity.Controller
{
    /*Init(controller, 0.4f, 2, GetTeam());// 1.設定為預設值
    Init(controller, 0.4f, 2, SetTeam(Team.Evil));// 2.
    Init(controller, 0.4f, 2);// 3.
    SetTeam(Team.Evil);*/
    /*private Message message;
    message = transform.GetOrLogComponent<Message>();
    message.ToString().LogLine();
    message.GetCreature().ToString().LogLine();
    message.GetCreatureAI().ToString().LogLine();
    message.GetAIState().ToString().LogLine();
    message.GetAIState().GetType().LogLine();*/
    [Serializable]
    public class PlayerAI : ICreatureAI
    {
        [SerializeField] private PlayerController playerController;

        public override void Update()
        {
            base.Update(); // 使用AI腳本控制，透過bool switch開關
            // playerController.Update(); // 使用人為控制
            /*if (anyKeyDown)
            {
                GetMessage().ToString().LogLine();
                GetMessage().GetCreature().ToString().LogLine();
                GetMessage().GetCreatureAI().ToString().LogLine();
                GetMessage().GetAIState().ToString().LogLine();
                GetMessage().GetAIState().GetType().LogLine();
                GetMessage().GetCreature().GetCreatureAttr().ToString().LogLine();
            }*/
        }

        public void SetController(PlayerController controller)
        {
            this.playerController = controller;
        }

        public PlayerController GetController() => playerController;

        public PlayerAI(ICreature creature, float attackRange = 0.4f, float chaseRange = 2) : base(creature,
            attackRange, chaseRange)
        {
        }
    }

    [Serializable]
    public class Player : ICreature
    {
        protected internal Player(ICreatureAttr creatureAttr, Transform transform) : base(creatureAttr, transform)
        {
        }
    }
}