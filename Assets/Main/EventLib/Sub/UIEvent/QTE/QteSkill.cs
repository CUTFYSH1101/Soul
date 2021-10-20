using System;
using JetBrains.Annotations;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Input;
using UnityEngine;

namespace Main.EventLib.Sub.UIEvent.QTE
{
/*
1.開啟UI同步計時
->2.按鍵正確 / UI計時結束，回傳Miss布林值
2.根據Miss布林值決定是格擋 / 受擊與攻擊動畫
->3.格檔或其他動畫播放完畢
3.角色可以自由行動 / 受到負面狀態無法動彈一陣子(接其他的事件)
 */
/* // 攻擊目標的方式，一共兩種
1.使用Spoiler.SetTargetHit(target, target, self.skill);
2.在攻擊方hierarchy路徑下，攻擊方position上創造一個傷害碰撞箱:
    new GameObject(){
    layer = "Attack".GetLayer(),
    AddComponent<CircleCollider2D>().isTrigger = true;
    Destroy(this, 0.02f);
    }，並設定攻擊方
    Creature.Attr.MindState=Attack
    Creature.Attr.CurrentSkill=xxx
*/
/*
ui持續時間
qte持續時間
技能持續時間
動畫時間=給玩家思考時間=怪物施放攻擊動畫前的等待時間
整段持續時間=從動畫UI被打開->怪物攻擊動畫結束
因此角色恢復正常都是等待這一段時間之後
 */
    public class QteSkill : AbsEventObject, IEvent3, IWorkOnCreature, ISkill
    {
        private readonly QteUIEvent _uiEvent;
        private EnumQteShape _attacker;
        private CreatureInterface _target;
        private Action _targetOnHitAction;

        /// 1.自帶傷害事件 或
        /// 2.由呼叫的Skill判斷，移除此項
        public QteSkill(Creature creature, QteUIEvent uiEvent)
        {
            this.Build(creature, EnumOrder.Attack, EnumCreatureEventTag.AtkNormal);
            // EventAttr = new EventAttr(cd, duration);
            _uiEvent = uiEvent;
            // CauseExit = () => _uiEvent.Miss);
            FinalAct += () =>
            {
                if (_target == null) return;
                UserInterface.RemoveState(_target.GetCreature(), EnumOtherState.UnderQte);
                if (_uiEvent.Miss)
                    AttackerGetHitBack();
                else
                    TargetGetHit();
            };
        }

        private void TargetGetHit()
        {
            _targetOnHitAction?.Invoke();
            UserInterface.Hit(_target.GetCreature(), SkillAttr);
            Debug.Log("對玩家造成暈眩1秒，傷害同之前");
            _target.MindState = EnumMindState.Idle;
        }

        private void AttackerGetHitBack()
        {
            _target.MindState = EnumMindState.Parry;
            UserInterface.CreateCreatureBehaviorInterface(CreatureInterface.GetCreature())
                .InvokeParryEvent();
            UserInterface.Hit(CreatureInterface.GetCreature(), SkillAttr);
            Debug.Log("玩家反抗，播放動畫和音效，對施放怪物造成暈眩");
        }

        public void Execute(Creature target, EnumQteShape attacker)
        {
            if (State != EnumState.Free) return;

            _target = new CreatureInterface(target);
            _attacker = attacker;
            Director.CreateEvent();
        }

        public void Enter()
        {
            // TargetStopMove();
            _uiEvent.Execute(_attacker);
            _uiEvent.Miss = false;
        }
        // 讓角色不要動
        private void TargetStopMove()
        {
            #region 無效化
            // _target.MindState = EnumMindState.UnderQteEvent;// 無效，因為一旦切入moving，就會不管三七二十一更改狀態
            // _target.InterruptMoving();// 無效，原因同上
            // _target.GetRigidbody2D().Velocity *= 0.3f;// 無效...
            #endregion
            #region 在Update加入這一行
            // _target.GetRigidbody2D().Velocity = Vector2.zero;
            #endregion
            UserInterface.AppendState(_target.GetCreature(), EnumOtherState.UnderQte);
        }

        public void Update()
        {
            if (TheUserPressesRightBtn)
            {
                _uiEvent.Miss = true;
                Debug.Log("成功打斷");
            }
        }

        private bool TheUserPressesRightBtn =>
            Input.Input.GetButtonDown(HotkeySet.Qte1) && _attacker == EnumQteShape.Square ||
            Input.Input.GetButtonDown(HotkeySet.Qte2) && _attacker == EnumQteShape.Cross ||
            Input.Input.GetButtonDown(HotkeySet.Qte3) && _attacker == EnumQteShape.Circle;

        public void Exit()
        {
            // FinalAct?.Invoke();
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public SkillAttr SkillAttr { get; set; }
        public CreatureInterface CreatureInterface { get; set; }

        public class Builder
        {
            private QteSkill Skill { get; set; }
            private QteUIEvent UIEvent { get; set; }

            public Builder(Creature creature)
            {
                UIEvent = UserInterface.CreateQteUI();
                Skill = new QteSkill(creature, UIEvent);
            }

            public Builder InitSkillAttr(EnumSkillTag tag)
            {
                Skill.SkillAttr = new SkillAttr(tag).SetDebuff(EnumDebuff.Dizzy);
                return this;
            }

            /// 注意順序在InitSkillAttr後
            public Builder InitAttr(float cd = 0, float duration = 0.5f)
            {
                UIEvent?.SetDuration(duration);
                Skill?.SetDuration(duration);
                UIEvent?.SetCd(cd);
                Skill?.SetCd(cd);
                /*(UIEvent as IEvent)?.SetDuration(duration);
                (Skill as IEvent)?.SetDuration(duration);
                (UIEvent as IEvent)?.SetCd(cd);
                (Skill as IEvent)?.SetCd(cd);*/
                return this;
            }

            public Builder InitVFXEvent([NotNull] Action targetOnHitAction)
            {
                Skill._targetOnHitAction = targetOnHitAction;
                return this;
            }

            public QteSkill GetSkill() => Skill;
            public QteUIEvent GetUI() => UIEvent;
        }
    }
    /*public class QteSkill : AbstractCreatureEventA, ISkill
    {
        private readonly QteUIEvent _uiEvent;
        private EnumQteSymbol _attacker;
        private CreatureInterfaceForSkill _target;
        private Action _targetOnHitAction;
        public SkillAttr SkillAttr { get; private set; }

        /// 1.自帶傷害事件 或
        /// 2.由呼叫的Skill判斷，移除此項
        public QteSkill(Creature creature, QteUIEvent uiEvent,
            float cd = 5, float duration = 0.5f) :
            base(creature, new EventAttr(cd, duration))
        {
            _uiEvent = uiEvent;
            // SetCd(cd).SetDuration(duration);
            InitCreatureEventOrder(EnumCreatureEventTag.NormalAttack, EnumOrder.Attack);
            CauseExit = () => _uiEvent.Miss);
            FinalEvent += () =>
            {
                if (_uiEvent.Miss)
                {
                    _target.MindState = EnumMindState.Parry;
                    UserInterface.CreateCreatureBehaviorInterface(CreatureInterfaceForSkill.GetCreature()).InvokeParryEvent();
                    UserInterface.Hit(CreatureInterfaceForSkill.GetCreature(), SkillAttr);
                    Debug.Log("玩家反抗，播放動畫和音效，對施放怪物造成暈眩");
                    return;
                }

                _targetOnHitAction?.Invoke();
                UserInterface.Hit(_target.GetCreature(), SkillAttr);
                Debug.Log("對玩家造成暈眩1秒，傷害同之前");
                _target.MindState = EnumMindState.Idle;
            };
        }
        /// 更新持續時間
        public QteSkill SetDuration(float duration = 0.5f)
        {
            _uiEvent.Duration = duration;
            EventAttr.MaxDuration = duration;
            // _uiEvent.CdTime = duration;
            SkillAttr.Duration = duration;
            return this;
        }
        public QteSkill SetCd(float cd = 5f)
        {
            _uiEvent.CdTime = cd;
            EventAttr.CdTime = cd;
            // _uiEvent.Duration = cd;
            SkillAttr.CdTime = cd;
            return this;
        }// todo qte cd會造成有時怪物必須兩輪才能攻擊
        public QteSkill InitSkillAttr(EnumSkillTag tag)
        {
            SkillAttr = new SkillAttr(tag, duration: EventAttr.MaxDuration, cdTime: EventAttr.CdTime)
                .SetDeBuff(DeBuff.Dizzy);
            return this;
        }

        public QteSkill InitVFXEvent([NotNull] Action targetOnHitAction)
        {
            this._targetOnHitAction = targetOnHitAction;
            return this;
        }

        public void Invoke(Creature target, EnumQteSymbol attacker)
        {
            if (!Finished) return;

            _target = new CreatureInterfaceForSkill(target);
            _attacker = attacker;
            // Debug.Log(_attacker);
            base.Invoke();
        }

        protected override void Enter()
        {
            _uiEvent.Invoke(_attacker);
            _target.MindState = EnumMindState.UnderQteEvent;
            _target.InterruptMoving();
            _uiEvent.Miss = false;
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
            FinalEvent?.Invoke();
        }
    }*/
}
/*
1.invoke
- if qte 正確觸發
    設定ui為miss，並中斷
    使attacker受到反嗜
- else 
    使injured受到
*/