using System.Collections.Generic;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventSystem;
using Main.EventSystem.Cause;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.EventSystem.Event.UIEvent.Combo;
using Main.Input;
using Main.Util;

namespace Main.CreatureAndBehavior.Behavior
{
    public class PlayerBehavior : IBehavior
    {
        private readonly CreatureBehaviorInterface _interface;
        private ComboUIEvent _comboUIEvent;

        /// 聆聽角色雙擊A/D/左/右事件，必須搭配_dashEvent
        /// <code>
        /// if ( DBAxisClick.AndCause())
        ///     _dashEvent.Invoke(new Vector2( DBAxisClick.AxisRaw() * CreatureAttr.DashForce, 0));
        /// </code>
        public DBAxisClick DBAxisClick { get; }

        public PlayerBehavior(Creature creature)
        {
            _interface = UserInterface.CreateCreatureBehaviorInterface(creature)
                .InitSkillDeBuff(DeBuff.Stiff, DeBuff.Stiff, DeBuff.Stiff) // 角色攻擊會造成怪物暈眩0.5秒
                .InitSkillCd(diveAttack: 7);
            // 普攻: [LookAt, 80]; 沒有內建設定
            // 突刺: [LookAt, 80];
            // 跳打: [(LookAtAxisX,-1), 80]
            // 俯衝: [LookAt, DiveForce * 0.05f]; DiveForce = 60
            UserInterface.SetKnockBack(_interface.Skill.normalAttack,
                dynDirection: () => UserInterface.GetLookAt(creature));


            // todo append to creature list, to search by spoiler
            AppendToSkillList(_interface.Skill.normalAttack.SkillAttr);
            AppendToSkillList(_interface.Skill.spurAttack.SkillAttr);
            AppendToSkillList(_interface.Skill.jumpAttack.SkillAttr);
            AppendToSkillList(_interface.Skill.diveAttack.SkillAttr);

            UserInterface.CreateCdUI("UI/PanelCd/Skill", _interface.Skill.diveAttack);
            _interface.Skill.diveAttack.AfterTouchGround += UserInterface.CreateCameraShakerEvent(0.02f, 0.1f).Invoke;


            DBAxisClick = new DBAxisClick(HotkeySet.Horizontal, .3f); // 雙擊時長0.3秒
        }

        /// wallJump + jump
        public void JumpOrWallJump() => _interface.JumpOrWallJump();

        public void Dash(int dir) => _interface.Dash(dir);
        public void MoveUpdate() => _interface.MoveUpdate();
        // public void MoveTo(Vector2 targetPos) => moveEvent.MoveTo(targetPos); todo moveTo

        // 玩家專屬，需指定攻擊符號才能造成傷害、音效 todo 怒氣值下無視符號 anim
        public void NormalAttack(EnumSymbol symbol) => _interface.NormalAttack(symbol);
        public void SpurAttack(EnumSymbol symbol) => _interface.SpurAttack(symbol);
        public void JumpAttack(EnumSymbol symbol) => _interface.JumpAttack(symbol);
        public void DiveAttack() => _interface.DiveAttack(EnumSymbol.Direct);

        public ComboUIEvent RegisterComboSystem() => _comboUIEvent = UserInterface.CreateComboUI();
        public void ComboUpdate() => _comboUIEvent.Invoke();
        public EnumDataTag Tag => EnumDataTag.Behavior;

        public Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary { get; } =
            new Dictionary<EnumSkillTag, SkillAttr>();

        public bool ContainTag(EnumSkillTag tag) => 
            SkillAttrDictionary.ContainsKey(tag);

        public SkillAttr FindSkillAttrByTag(EnumSkillTag tag)
        {
            if (SkillAttrDictionary.ContainsKey(tag))
                return SkillAttrDictionary[tag];

            return null;
        }

        public void AppendToSkillList(SkillAttr newSkill)
        {
            if (SkillAttrDictionary.ContainsKey(newSkill.SkillTag))
                "字典中已含有相同key值".LogErrorLine();

            SkillAttrDictionary.Add(newSkill.SkillTag, newSkill);
        }
    }
}