﻿using System;
using Main.Blood;
using JetBrains.Annotations;
using Main.Util;
using Main.EventLib.Common;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.EventLib.Sub.CreatureEvent.StateEvent;
using Main.EventLib.Sub.UIEvent.QTE;
using Main.Res.Script;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventLib.Sub.CreatureEvent.Skill.Attribute
{
    // 會根據不同技能，而有不同的
    // 1.擊退距離和方向
    // 2.負面狀態
    // 3.技能自帶特效
    public class SkillAttr
    {
        public EnumSkillTag SkillTag { get; }
        public BloodType BloodType { get; set; }
        public EnumQteShape QteShape { get; set; }
        public float Duration { get; set; }

        public float CdTime { get; set; }

        public EnumBuff Buff { get; set; }
        public EnumDebuff Debuff { get; set; }
        // private DeBuffList _deBuffList = new DeBuffList(); // todo改為
        public SkillAttr SetDebuff(EnumDebuff debuff)
        {
            Debuff = debuff;
            return this;
        }

        /// 基本資料
        /// <param name="skillTag">辨識用</param>
        /// <param name="bloodType"></param>
        /// <param name="duration">0表示沒有，技能持續時間是根據動畫時長</param>
        /// <param name="cdTime">0表示沒有</param>
        public SkillAttr(EnumSkillTag skillTag = default, BloodType bloodType = default,
            float duration = 0, float cdTime = 0)
        {
            SkillTag = skillTag;
            BloodType = bloodType;
            Duration = duration;
            CdTime = cdTime;
            QteShape = EnumQteShape.None;
            Buff = EnumBuff.None;
            Debuff = EnumDebuff.None;
            Knockback = default;
            VFX = (null, null, default, false);
        }

        /// [擊退力,移動方向,最終移動力]
        public KnockbackAttr Knockback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force">擊退力大小，越大擊退距離越遠，0表示沒有攻擊方向</param>
        /// <param name="dynDirection">null表示沒有攻擊方向</param>
        /// <param name="switch"></param>
        public SkillAttr SetKnockBack(float force = 80, Func<Vector2> dynDirection = null, bool @switch = true)
        {
            Knockback = new KnockbackAttr(force, dynDirection, @switch);
            return this;
        }

        /// [物件,頂部朝向(0,1,0),偏移位置]
        public (Transform obj, Func<Vector2> dynDirection, Vector2 offsetPos, bool @switch) VFX { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">技能本身特效</param>
        /// <param name="dynDirection">頂部朝向(0,1,0)</param>
        /// <param name="offsetPos">技能特效產生位置偏移，default表示原位</param>
        /// <param name="switch"></param>
        public SkillAttr SetVFX([CanBeNull] Transform obj, Func<Vector2> dynDirection = null, Vector2 offsetPos = default,
            bool @switch = false)
        {
            var _ = VFX;
            _.obj = obj;
            _.dynDirection = dynDirection;
            _.offsetPos = offsetPos;
            _.@switch = @switch;
            VFX = _;
            return this;
        }

        public override string ToString() =>
            $"--{GetType().Name}--\n" +
            this.GetMembersToNameString().ArrayToString("\n", false);
    }
}