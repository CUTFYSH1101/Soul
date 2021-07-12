using System;
using Main.Attribute;
using UnityEngine;

namespace Main.Entity
{
    /*SetBehavior(controller, 0.4f, 2, GetTeam());// 1.設定為預設值
    SetBehavior(controller, 0.4f, 2, SetTeam(Team.Evil));// 2.
    SetBehavior(controller, 0.4f, 2);// 3.
    SetTeam(Team.Evil);*/
    /*private Message message;
    message = transform.GetOrLogComponent<Message>();
    message.ToString().LogLine();
    message.GetCreature().ToString().LogLine();
    message.GetCreatureAI().ToString().LogLine();
    message.GetAIState().ToString().LogLine();
    message.GetAIState().GetType().LogLine();*/
    [Serializable]
    public class PlayerAI : AbstractCreatureAI
    {
        [SerializeField] private PlayerController playerController;

        public override void Update()
        {
            base.Update(); // 使用AI腳本控制，透過bool switch開關
            // playerController.Invoke(); // 使用人為控制
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

        /*public PlayerAI(Creature creature, float attackRange = 0.4f, float chaseRange = 2) : base(creature,
            attackRange, chaseRange)
        {
        }*/
        public PlayerAI(AbstractCreature abstractCreature) : base(abstractCreature)
        {
        }
    }

    [Serializable]
    public class Player : AbstractCreature
    {
        protected internal Player(CreatureAttr creatureAttr, Transform transform, DictionaryAudioPlayer audioPlayer) : base(creatureAttr, transform, audioPlayer)
        {
            SetBehavior(new PlayerBehavior(this,() => creatureAttr.Grounded));
        }
    }
}