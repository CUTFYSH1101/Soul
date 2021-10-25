using Main.Blood;
using Main.CreatureAI.Sub;
using Main.CreatureBehavior.Behavior.Sub;
using Main.EventLib;
using Main.EventLib.Sub.BattleSystem;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.Res.Script.Audio;
using UnityEngine;

namespace Main.Entity.Creature.Sub
{
    public class PlayerBuilder : Builder
    {
        public PlayerBuilder(Transform obj, DictAudioPlayer audioPlayer)
        {
            Creature = new Creature(obj,
                new CreatureAttr(jumpForce: 30, dashForce: 40, diveForce: 60),
                audioPlayer);
        }

        public override void Init()
        {
            // 添加可以操作的行為，允許外界存取
            var data = (PlayerBehavior)Creature.Append(
                new PlayerBehavior(Creature));
            // 添加玩家控制器
            var controller = Creature.Append(
                new PlayerController(data,
                    new CreatureInterface(Creature)));
            // 添加角色策略
            Creature.Append(
                new PlayerStrategy((IPlayerController)controller));
        }

        public override void SetAISystem()
        {
        }

        public override void SetBattleSystem()
        {
            var combo = UserInterface.CreateComboUI();
            Creature.Append(
                Spoiler.Instance(Creature, Team.Player).InitEvent(combo.Trigger));
            
            Creature.Append(
                BloodHandler.Instance(Creature.Transform));
        }
    }
}