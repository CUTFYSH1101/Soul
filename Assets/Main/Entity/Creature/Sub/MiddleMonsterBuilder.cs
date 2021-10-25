using Main.CreatureAI.Sub;
using Main.CreatureBehavior.Behavior.Sub;
using Main.EventLib.Sub;
using Main.EventLib.Sub.BattleSystem;
using Main.EventLib.Sub.CreatureEvent.Skill;
using UnityEngine;

namespace Main.Entity.Creature.Sub
{
    public class MiddleMonsterBuilder : Builder
    {
        public MiddleMonsterBuilder(Transform obj) =>
            Creature = new Director(new MonsterBuilder(obj)).GetResult(); // 繼承monster的物件

        public override void Init()
        {
            Creature.CreatureAttr.MoveSpeed = 1.5f;
        }

        public override void SetAISystem()
        {
            if (Creature.Contains(EnumDataTag.Behavior))
                Creature.RemoveByTag(EnumDataTag.Behavior);
            if (Creature.Contains(EnumComponentTag.CreatureStrategy))
                Creature.RemoveByTag(EnumComponentTag.CreatureStrategy);

            var behavior = Creature.Append(new MiddleMonsterBehavior(Creature));

            var strategy = Creature.Append(new MiddleMonsterStrategy(
                new CreatureInterface(Creature), behavior as MiddleMonsterBehavior, Team.Enemy,
                new Vector2(10f, 1.5f), new Vector2(3.2f, 1.5f)));
            DebugMode.DoWhenOpen += () =>
                Creature.Remove(strategy);
            DebugMode.DoWhenClose += () =>
                Creature.Append(strategy);
        }

        public override void SetBattleSystem()
        {
        }
    }
}