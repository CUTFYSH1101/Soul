using Main.AnimAndAudioSystem.Main.Common;
using Main.EventSystem.Event.CreatureEventSystem;

namespace Main.CreatureAndBehavior.Behavior
{
    public class MonsterBehavior
    {
        private readonly CreatureBehaviorInterface _interface;

        public MonsterBehavior(Entity.Creature.Creature creature)
        {
            _interface = new CreatureBehaviorInterface(creature);
            _interface.InitSkillCd(normalAttack: 2); // 每兩秒攻擊一次
        }
        // public void MoveTo(Vector2 targetPos) => moveEvent.MoveTo(targetPos); todo moveTo
        
        public void NormalAttack() => _interface.NormalAttack(EnumSymbol.Direct);
    }
    /*
    public class MonsterBehavior : AbstractCreatureBehavior
    {
        [NotNull] private readonly DictionaryAudioPlayer audioAudioPlayer;
        [NotNull] private readonly Func<bool> getGrounded;
        [NotNull] private readonly BaseBehavior baseBehavior;

        public MonsterBehavior(Creature creature, [NotNull] Func<bool> getGrounded) : base(creature)
        {
            this.getGrounded = getGrounded;
            audioAudioPlayer = UnityRes.GetNormalAttackAudioPlayer();
            baseBehavior = new BaseBehavior(creature, getGrounded, audioAudioPlayer);
            jumpEvent = new JumpEvent(creature, baseBehavior);
            moveController = new MoveController(creature, HotkeySet.Horizontal,
                baseBehavior.Move, baseBehavior.Dash);
            normalAttack = new NormalAttack(creature, 2); // 設定每兩秒才能攻擊一次
            // normalAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;
            AppendAttr(normalAttack.SkillAttr);
        }
        /// 不含鄧牆跳
        public override void Jump() => baseBehavior.Jump(); // jump

        public void Move()
        {
            moveController.Update();
        }

        public override void MoveTo(bool @switch, Vector2 targetPos)
        {
            baseBehavior.MoveTo(@switch, targetPos);
        }


        public override void Hit(SkillAttr skillAttr)
        {
            baseBehavior.Hit(skillAttr);
        }

        private readonly NormalAttack normalAttack;
        public void NormalAttack()
        {
            // 每兩秒攻擊一次
            normalAttack.Invoke(EnumSymbol.Direct);
        }
    }
*/
}