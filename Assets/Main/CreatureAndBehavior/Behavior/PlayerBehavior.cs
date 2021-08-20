using Main.Entity.Creature;
using Main.EventSystem;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem;
using Main.EventSystem.Event.CreatureEventSystem.Skill;

namespace Main.CreatureAndBehavior.Behavior
{
    public class PlayerBehavior
    {
        private CreatureBehaviorInterface _interface;
        public PlayerBehavior(AbstractCreature creature)
        {
            _interface = UserInterface.CreateCreatureBehaviorInterface(creature)
                .InitSkillDeBuff(DeBuff.Stiff,DeBuff.Stiff,DeBuff.Stiff);
            // todo append to creature list, to search by spoiler
            // UserInterface.CreateCdUI("UI/PanelCd/Skill", 2); todo diveAttack cdTime
        }
    }
    /*[Serializable]
    public class PlayerBehavior : AbstractCreatureBehavior
    {
        [NotNull] private readonly DictionaryAudioPlayer audioAudioPlayer;
        [NotNull] private readonly BaseBehavior baseBehavior;
        private readonly CDEvent diveAttackColdDownUI;
        private readonly CDEvent healingKitColdDownUI;

        public PlayerBehavior(Creature.Creature creature, [NotNull] Func<bool> getGrounded) : base(creature)
        {
            this.getGrounded = getGrounded;
            audioAudioPlayer = UnityRes.GetNormalAttackAudioPlayer();
            baseBehavior = new BaseBehavior(creature, getGrounded, audioAudioPlayer);
            moveEvent = new MoveEvent(creature, HotkeySet.Horizontal,
                baseBehavior.Move, baseBehavior.Dash);
            jumpEvent = new JumpEvent(creature, baseBehavior);
            normalAttack = new NormalAttack(creature);
            normalAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff; // 給予擊退，搭配event
            diveAttack = new DiveAttack(creature, 7);
            spurAttack = new SpurAttack(creature,
                () => moveEvent.Switch(false),
                () => moveEvent.Switch(true));
            spurAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;
            jumpAttack = new JumpAttack(creature);
            spurAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;

            AppendAttr(normalAttack.SkillAttr);
            AppendAttr(diveAttack.SkillAttr);
            AppendAttr(spurAttack.SkillAttr);
            AppendAttr(jumpAttack.SkillAttr);

            diveAttackColdDownUI = new CDEvent("UI/PanelCd/Skill", diveAttack.SkillAttr.CdTime);
            healingKitColdDownUI = new CDEvent("UI/PanelCd/HealingKit", 5);

            diveAttack.AppendOnEnterEvent(() =>
            {
                // 冷卻時間icon
                diveAttackColdDownUI.SetDuration(diveAttack.SkillAttr.CdTime); // 紀錄現在cd時間
                diveAttackColdDownUI.Invoke(); // 開始倒數計時

                // 相機
                UnityRes.GetCameraShaker().Invoke();
            });
        }

        [NotNull] private readonly JumpEvent jumpEvent;

        public override void Jump() => jumpEvent.Invoke(); // wallJump + jump

        private readonly MoveEvent moveEvent;

        public void Move() => moveEvent.MoveUpdate();

        public void MoveTo(Vector2 targetPos) => moveEvent.MoveTo(targetPos);
        

        [NotNull] private readonly NormalAttack normalAttack;

        public void NormalAttack(EnumSymbol symbol)
        {
            normalAttack.Invoke(symbol); // 玩家專屬normalAttack、音效
        }

        [NotNull] private readonly SpurAttack spurAttack;

        public void SpurAttack(EnumSymbol symbol)
        {
            spurAttack.Invoke(symbol); // 玩家專屬
        }

        [NotNull] private readonly DiveAttack diveAttack;

        public void DiveAttack(EnumSymbol symbol)
        {
            baseBehavior.InterruptDash();
            diveAttack.Invoke(symbol);
        }

        [NotNull] private readonly JumpAttack jumpAttack;

        public void JumpAttack(EnumSymbol symbol)
        {
            jumpAttack.Invoke(symbol);
        }
    }*/
}