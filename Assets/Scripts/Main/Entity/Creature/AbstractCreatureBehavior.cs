using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210528;
using UnityEngine;
using Math = System.Math;
using Vector2 = UnityEngine.Vector2;
using Rb2D = Main.Entity.Controller.Rigidbody2D;
using Skills = Main.Entity.Skill_210528;

namespace Main.Entity
{
    public abstract class AbstractCreatureBehavior
    {
        protected readonly AbstractCreature AbstractCreature;
        protected readonly ICreatureAttr creatureAttr;
        protected readonly Rb2D rigidbody2D;
        protected readonly CreatureAnimManager animManager;

        protected AbstractCreatureBehavior(AbstractCreature abstractCreature)
        {
            this.AbstractCreature = abstractCreature;
            this.creatureAttr = abstractCreature.GetCreatureAttr();
            this.rigidbody2D = abstractCreature.GetRigidbody2D();
            animManager = abstractCreature.GetAnimator();
        }

        public abstract void Jump();
        public abstract void Dash(Symbol symbol, bool @switch);
        public abstract void Move(bool @switch, float dir);
        public abstract void MoveTo(bool @switch, Vector2 targetPos);

        public abstract void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default);

        // ======
        // Skill
        // ======
        public abstract void NormalAttack();

        public abstract void SpurAttack();

        public abstract void DiveAttack(Symbol type, Vector2 direction, float force);
    }

    public class PlayerBehavior : AbstractCreatureBehavior
    {
        private DictionaryAudioPlayer audioAudioPlayer;
        public override void Jump()
        {
        }

        public override void Dash(Symbol symbol, bool @switch)
        {
        }

        public override void Move(bool @switch, float dir)
        {
            if (dir != 0)
            {
                animManager.Move(true);
                var moveX = Math.Sign(dir) * creatureAttr.MoveSpeed;
                rigidbody2D.SetActiveX(moveX);
                AbstractCreature.SetMindState(MindState.Move);
            }
            else
            {
                animManager.Move(false);
                rigidbody2D.SetActiveX(0); // 當受到攻擊時，速度歸零
                if (animManager.IsTag("Attack")) // 當攻擊時，不錯誤更改狀態
                    return;
                AbstractCreature.SetMindState(MindState.Idle);
            }
        }

        public override void MoveTo(bool @switch, Vector2 targetPos)
        {
            if (@switch)
                rigidbody2D.MoveTo(targetPos, creatureAttr.MoveSpeed, creatureAttr.JumpForce);
            else
                rigidbody2D.SetGuideX(0);
        }

        public override void Hit(Vector2 direction, float force, Transform vfx = null, Vector2 position = default)
        {
        }

        public override void NormalAttack()
        {
        }

        public override void SpurAttack()
        {
        }

        private readonly DiveAttack diveAttack;
        
        public override void DiveAttack(Symbol type, Vector2 direction, float force)
        {
            diveAttack.Invoke(type);
        }

        public PlayerBehavior(AbstractCreature abstractCreature) : base(abstractCreature)
        {
            diveAttack = new DiveAttack(abstractCreature);
            audioAudioPlayer = Resources.Load<DictionaryAudioPlayer>("Audios/NAAudioPlayer");
        }
    }
}