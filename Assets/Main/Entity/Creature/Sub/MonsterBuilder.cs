using Main.Blood;
using Main.CreatureAI.Sub;
using Main.CreatureBehavior.Behavior.Sub;
using Main.EventLib.Sub.BattleSystem;
using Main.EventLib.Sub.CreatureEvent.Skill;
using UnityEngine;

namespace Main.Entity.Creature.Sub
{
    public class MonsterBuilder : Builder
    {
        public MonsterBuilder(Transform obj)
        {
            Creature = new Creature(obj,
                new CreatureAttr(jumpForce: 30, moveSpeed: 2),
                null);
        }

        public override void Init()
        {
        }


        public override void SetAISystem()
        {
            // 添加角色行為、角色AI策略
            var behavior = Creature.AppendData(new MonsterBehavior(Creature));
            
            Creature.AppendComponent(new MonsterStrategy(
                new CreatureInterface(Creature), behavior as MonsterBehavior, Team.Enemy,
                new Vector2(10f, 1.5f), new Vector2(0.85f, 1.5f)));
        }

        public override void SetBattleSystem()
        {
            Creature.AppendComponent(
                Spoiler.Instance(Creature, Team.Enemy));
            
            // aa沒有血條
            Creature.AppendComponent(
                BloodHandler.Instance(Creature.Transform));
        }
    }
}