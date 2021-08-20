using Main.AnimAndAudioSystem.Anims.Scripts;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.EventSystem.Event.CreatureEventSystem.StateEvent;
using Main.Game;

namespace Main.CreatureAndBehavior.Behavior
{
    /*public abstract class AbstractCreatureBehavior
    {
        protected readonly Creature.Creature Creature;
        protected readonly CreatureAttr CreatureAttr;
        protected readonly UnityRb2D Rigidbody2D;
        protected readonly CreatureAnimManager AnimManager;
        protected float GetDirX => Creature.IsFacingRight ? 1 : -1;
        private readonly SkillAttrList _list = new SkillAttrList();

        protected void AppendAttr(SkillAttr newSkillAttr) =>
            _list.Append(newSkillAttr);

        public SkillAttr FindByName(EnumSkillTag skillTag) =>
            _list.Find(skillTag);

        protected AbstractCreatureBehavior(Creature.Creature creature)
        {
            Creature = creature;
            CreatureAttr = creature.CreatureAttr;
            Rigidbody2D = creature.Rigidbody2D;
            AnimManager = creature.AnimManager;
            _knockback = new Knockback(creature);
        }

        public abstract void Jump();

        private readonly Knockback _knockback;
        /// 更改受擊方的精神狀態，並產生對應行為
        public void Hit(SkillAttr skillAttr)
        {
            if (skillAttr == null)
                return;
            HitEvent.Invoke(Creature, _knockback, skillAttr);
        }

        public void Killed()
        {
            AnimManager.Killed(); // IsTag("Die") == true
            CreatureAttr.MindState = EnumMindState.Dead;
            CreatureAttr.Alive = false;
        }

        public void Revival()
        {
            AnimManager.Revival(); // IsTag("Die") == false
            CreatureAttr.MindState = EnumMindState.Idle;
            CreatureAttr.Alive = true;
        }

        public override string ToString() =>
            $"--【{GetType().Name}】--\n" +
            _list;
    }*/
}