using System.Collections.Generic;
using Main.Blood;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib;
using Main.EventLib.Common;
using Main.EventLib.Condition;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.EventLib.Sub.UIEvent.Combo;
using Main.Input;
using Main.Res.Script;
using UnityEngine;

namespace Main.CreatureBehavior.Behavior.Sub
{
    public class PlayerBehavior : IBehavior
    {
        private readonly CreatureBehaviorInterface _interface;
        private ComboUIEvent _comboUIEvent;

        /// 聆聽角色雙擊A/D/左/右事件，必須搭配_dashEvent
        /// <code>
        /// if ( DBAxisClick.AndCause())
        ///     _dashEvent.Execute(new Vector2( DBAxisClick.AxisRaw() * CreatureAttr.DashForce, 0));
        /// </code>
        public DBAxisClick DBAxisClick { get; }

        private CreatureBehaviorInterface _originInterface;

        public PlayerBehavior(Creature creature)
        {
            _interface = UserInterface.CreateCreatureBehaviorInterface(creature)
                .SetSkillDebuff(EnumDebuff.Stiff, EnumDebuff.Stiff, EnumDebuff.Stiff) // 角色攻擊會造成怪物暈眩0.5秒
                .SetSkillCd(diveAttack: 7);
            // 普攻: [LookAt, 80]; 沒有內建設定
            // 突刺: [LookAt, 80];
            // 跳打: [(LookAtAxisX,-1), 80]
            // 俯衝: [LookAt, DiveForce * 0.05f]; DiveForce = 60
            UserInterface.SetKnockBack(_interface.AtkSkill.normal,
                dynDirection: () => UserInterface.GetLookAt(creature));


            // todo append to creature list, to search by spoiler
            AppendToSkillList(_interface.AtkSkill.normal.SkillAttr);
            AppendToSkillList(_interface.AtkSkill.spur.SkillAttr);
            AppendToSkillList(_interface.AtkSkill.jump.SkillAttr);
            AppendToSkillList(_interface.AtkSkill.dive.SkillAttr);

            // 自帶diveAttack cd, CameraShaker
            UserInterface.CreateCdUI("UI/PanelCd/Skill", _interface.AtkSkill.dive);
            _interface.AtkSkill.dive.AfterTouchGround += UserInterface.CreateCameraShakerEvent(0.02f, 0.1f).Execute;


            DBAxisClick = new DBAxisClick(HotkeySet.Horizontal, .3f); // 雙擊時長0.3秒
        }

        /// wallJump + jump
        public void JumpOrWallJump() => _interface.JumpOrWallJump();

        public void Dash(int dir) => _interface.Dash(dir);
        // public void Dash2(int force, float forceDuration) => _interface.Dash2(force, forceDuration);

        public void MoveUpdate() => _interface.MoveUpdate();
        public void MoveTo(Vector2 targetPos) => _interface.MoveTo(targetPos);

        // 玩家專屬，需指定攻擊符號才能造成傷害、音效 todo 怒氣值下無視符號 anim
        public void NormalAttack(BloodType shape) => _interface.NormalAttack(shape);
        public void SpurAttack(BloodType shape) => _interface.SpurAttack(shape);
        public void JumpAttack(BloodType shape) => _interface.JumpAttack(shape);
        public void DiveAttack() => _interface.DiveAttack(BloodType.Direct);

        public ComboUIEvent RegisterComboSystem() => _comboUIEvent = UserInterface.CreateComboUI();
        public void ComboUpdate() => _comboUIEvent.Trigger();
        public EnumDataTag Tag => EnumDataTag.Behavior;

        private readonly SkillDictionary _skillDictionary = new();
        public Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary => 
            _skillDictionary.SkillAttrDictionary;

        // 希望趕快支持interface default！！
        public SkillAttr FindSkillAttrByTag(EnumSkillTag tag) =>
            _skillDictionary.FindSkillAttrByTag(tag);
        public void AppendToSkillList(SkillAttr newSkill) => 
            _skillDictionary.AppendToSkillList(newSkill);
    }
}