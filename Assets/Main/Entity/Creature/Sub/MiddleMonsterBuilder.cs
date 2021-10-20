using Main.CreatureBehavior.Behavior.Sub;
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
            if (Creature.ContainsData(EnumDataTag.Behavior))
                Creature.RemoveDataByTag(EnumDataTag.Behavior);
            if (Creature.ContainsComponent(EnumComponentTag.CreatureStrategy))
                Creature.RemoveComponentByTag(EnumComponentTag.CreatureStrategy);

            var behavior = Creature.AppendData(new MiddleMonsterBehavior(Creature));

            /*Creature.AppendComponent(new MiddleMonsterStrategy(
                new CreatureInterface(Creature), behavior as MiddleMonsterBehavior, Team.Enemy,
                new Vector2(10f, 1.5f), new Vector2(3.2f, 1.5f)));*/
        }

        public override void SetBattleSystem()
        {
        }
    }
}