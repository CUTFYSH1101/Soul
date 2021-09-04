using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Input;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Main.EventSystem.Event.UIEvent.QTE
{
    public abstract class AbstractQteSkill : AbstractCreatureEventA, ISkill
    {
        private readonly QteUIEvent _uiEvent;
        private EnumQteSymbol _attacker;
        private CreatureInterface _target;
        public SkillAttr SkillAttr { get; private set; }

        /// 1.自帶傷害事件 或
        /// 2.由呼叫的Skill判斷，移除此項
        protected AbstractQteSkill(Creature creature, QteUIEvent uiEvent,
            float cd = 5, float duration = 0.5f) :
            base(creature, new EventAttr(cd, duration))
        {
            _uiEvent = uiEvent;
            _uiEvent.Duration = duration;
        }

        /// 更新持續時間
        public void SetDuration(float duration = 0.5f)
        {
            EventAttr.MaxDuration = duration;
            _uiEvent.Duration = duration;
            SkillAttr.Duration = duration;
        }

        /// <code>
        /// InitSkillAttr(xxx)
        ///     .SetKnockBack
        ///     .SetVFX
        /// </code>
        /// <param name="tag"></param>
        /// <returns></returns>
        public SkillAttr InitSkillAttr(EnumSkillTag tag) =>
            SkillAttr = new SkillAttr(tag, duration: EventAttr.MaxDuration, cdTime: EventAttr.CdTime)
                .SetDeBuff(DeBuff.Dizzy);

        public void Invoke(Creature target, EnumQteSymbol attacker)
        {
            _target = new CreatureInterface(target);
            _attacker = attacker;
            _uiEvent.Invoke(attacker);
            base.Invoke();
        }

        protected override void Enter()
        {
            _target.MindState = EnumMindState.UnderQteEvent;
        }

        protected override void Update()
        {
            if (Input.Input.GetButtonDown(HotkeySet.Qte1) && _attacker == EnumQteSymbol.Square ||
                Input.Input.GetButtonDown(HotkeySet.Qte2) && _attacker == EnumQteSymbol.Cross ||
                Input.Input.GetButtonDown(HotkeySet.Qte3) && _attacker == EnumQteSymbol.Circle)
            {
                _uiEvent.Miss = true;
                Debug.Log("成功打斷");
            }
        }

        protected override void Exit()
        {
            if (_uiEvent.Miss)
            {
                _target.MindState = EnumMindState.Parry;
                UserInterface.CreateCreatureBehaviorInterface(CreatureInterface.GetCreature()).InvokeParryEvent();
                UserInterface.Hit(CreatureInterface.GetCreature(), SkillAttr);
                Debug.Log("玩家反抗，播放動畫和音效，對施放怪物造成暈眩");
                return;
            }

            UserInterface.Hit(_target.GetCreature(), SkillAttr);
            Debug.Log("對玩家造成暈眩1秒，傷害同之前");
            /*
            1.Spoiler.SetTargetHit(target, target, self.skill);
            2.在攻擊方hierarchy路徑下，攻擊方position上創造一個傷害碰撞箱:
                new GameObject(){
                    layer = "Attack".GetLayer(),
                    AddComponent<CircleCollider2D>().isTrigger = true;
                    Destroy(this, 0.02f);
                }，並設定攻擊方
                    Creature.Attr.MindState=Attack
                    Creature.Attr.CurrentSkill=xxx
            */
        }
    }
}
/*
1.invoke
- if qte 正確觸發
    設定ui為miss，並中斷
    使attacker受到反嗜
- else 
    使injured受到
*/