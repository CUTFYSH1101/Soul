using Main.Attribute;
using Main.Common;
using Main.Extension.Util;
using Main.Game.Collision;
using UnityEngine;
using static Main.Common.Symbol;

namespace Main.Entity.Skill_210528
{
    /*public class DiveAttack : AbstractSkillWrapper
    {
        private readonly AbstractCreature abstractCreature;
        private readonly ICreatureAttr attr;
        private readonly Rigidbody2D rigidbody2D;
        private readonly float gravityScale;
        private float diveForce => attr.DiveForce;

        private float recoilForce => attr.RecoilForce;

        // 總技能時長
        private readonly ICause totalDuration;
        private readonly ICause chargedDuration;
        private readonly SkillAttr skillAttr;
        private readonly float groundClearance = 0.7f;

        private bool MoreThenGroundClearance =>
            rigidbody2D.RayCast(abstractCreature.GetPosition(), Vector2.down, groundClearance);

        private Vector2 dir;
        private MoveAnim dashAnim;

        public DiveAttack(AbstractCreature abstractCreature, float cdTime = 0.2f) :
            base(abstractCreature.GetRigidbody2D().GetOrAddComponent<MonoClass>(), cdTime, Stopwatch.Mode.LocalGame)
        {
            skillAttr = new SkillAttr(Symbol.None, 0.5f, cdTime);

            this.abstractCreature = abstractCreature;
            rigidbody2D = abstractCreature.GetRigidbody2D();
            gravityScale = rigidbody2D.instance.gravityScale;
            attr = abstractCreature.GetCreatureAttr();
            dashAnim = new MoveAnim(abstractCreature.GetAnimator());

            totalDuration = new CDCause(skillAttr.Duration);
            chargedDuration = new CDCause(0.3f);

            causeEnter.cause = () => !attr.Grounded && MoreThenGroundClearance; // mindState = 空中時，允許俯衝
            causeToAction2.cause1 = chargedDuration;
            causeToAction3.cause = () => attr.Grounded || totalDuration.Cause(); // 或是duration停止時
            causeInterrupt.cause = () => attr.Grounded || totalDuration.Cause();
        }

        public void Invoke(Symbol symbol)
        {
            // Debug.Log(abstractCreature.GetAnimator().HasParameter(UnityAnimID.DiveAttacking));
            if (state == State.Waiting)
            {
                skillAttr.Symbol = symbol;
                // skillAttr.Symbol = Direct;
                if (totalDuration.Cause())
                    totalDuration.Reset();
                base.Invoke();
            }
        }

        // ChargedAnimation 蓄力動畫
        protected override void EnterAction1()
        {
            attr.MindState = MindState.Attack;
            rigidbody2D.instance.gravityScale = 0;
        }

        // Crash
        protected override void Action2()
        {
            // 關閉蓄力動畫
            rigidbody2D.instance.gravityScale = gravityScale;

            // 播放攻擊中動畫
            dir = new Vector2(abstractCreature.IsFacingRight ? 0.7f : -0.7f, -0.7f);
            abstractCreature.DiveAttack(dir, diveForce, skillAttr.Symbol); // 注意需要先更改skillAttr的數值，才有辦法呼叫
        }

        // Stiff
        protected override void Action3()
        {
            // 創造傷害範圍
            // dir = new Vector2(0, 0.1f);
            // rigidbody2D.AddForce_OnActive(dir * recoilForce,ForceMode2D.Impulse);
            // Debug.Log(dir);
            // 回到idle
            abstractCreature.DiveAttackStop(skillAttr.Symbol);
            // stop move / jump
            // open
            attr.MindState = MindState.Idle;
            // rigidbody2D.StartCoroutine(Stiff());
        }

        // Idle
        protected override void ExitAction4()
        {
        }
    }*/

    public class DiveAttack : AbstractSkill
    {
        private const float CdTime = 0.2f;
        private readonly SkillAttr skillAttr;
        private readonly SkillTemplate skillTemplate;
        private readonly float height = 1.5f;

        private bool MoreThenGroundClearance =>
            skillTemplate.GetCreature().GetTransform().GroundClearance() > height;

        public DiveAttack(AbstractCreature abstractCreature) :
            base(abstractCreature.MonoClass(), CdTime)
        {
            skillAttr = new SkillAttr(Direct, 0.5f, CdTime);
            skillTemplate = new SkillTemplate(abstractCreature, 0.5f);
            var attr = skillTemplate.GetCreatureAttr();

            causeEnter = () => !attr.Grounded && MoreThenGroundClearance; // mindState = 空中時，允許俯衝
            causeExit = () => attr.Grounded || skillTemplate.DurationCause();

            onEnter = () => attr.MindState = MindState.Attack;
            onExit = () => attr.MindState = MindState.Idle; // 注意exit是否from knockback
        }

        public void Invoke(Symbol symbol)
        {
            // Debug.Log(abstractCreature.GetAnimator().HasParameter(UnityAnimID.DiveAttacking));// 注意不要呼叫anim中沒有的參數
            // 等動畫播放完才可再次攻擊
            if (state == State.Waiting && !skillTemplate.IsTag("Attack"))
            {
                skillAttr.Symbol = symbol;
                // skillAttr.Symbol = Direct;
                skillTemplate.Reset();
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            Vector2 dir = new Vector2(skillTemplate.GetDirX * 0.7f, -0.7f).normalized;
            // 切換動畫
            skillTemplate.GetAnimator().DiveAttack(skillAttr.Symbol, true); // 注意需要先更改skillAttr的數值，才有辦法呼叫
            // 添加力
            skillTemplate.AddForce_OnActive(dir * skillTemplate.GetCreatureAttr().DiveForce, ForceMode2D.Impulse);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            // 切換動畫
            skillTemplate.GetAnimator().DiveAttack(skillAttr.Symbol, false);
        }
    }
}