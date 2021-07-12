using System;
using JetBrains.Annotations;
using Main.Common;
using Main.Entity;
using Main.Extension.Util;
using UnityEngine;
using Main.Util;

namespace Main.Attribute
{
    public class SkillAttr
    {
        public SkillName SkillName { get; }
        public Symbol Symbol { get; set; }
        public AbstractCreature Container { get; private set; }
        public void SetContainer(AbstractCreature parent) => Container = parent;
        public float Duration { get; }

        public float CdTime { get; }

        /// 擊退力
        public float Knockback { get; }

        /// 擊退力方向
        [NotNull] private readonly Func<Vector2> direction;

        /// 擊退力方向。必回傳歸一值
        public Func<Vector2> Direction => () =>
            direction().normalized;

        /// 攻擊偏移位置
        public Vector2 OffsetPos { get; }

        /// 技能本身特效
        [CanBeNull]
        public Transform VFX { get; }

        public Buff Buff { get; set; }
        public DeBuff DeBuffBuff { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param skillName="skillName">辨識用</param>
        /// <param skillName="symbol"></param>
        /// <param skillName="duration">0.1f表示技能持續時間是根據動畫時長</param>
        /// <param skillName="cdTime"></param>
        /// <param skillName="direction">null!或default表示沒有攻擊方向</param>
        /// <param skillName="knockback"></param>
        /// <param skillName="vfx">技能本身特效</param>
        public SkillAttr(SkillName skillName, Symbol symbol,
            float duration, float cdTime,
            [NotNull] Func<Vector2> direction, float knockback = 30, Vector2 offsetPos = default,
            Transform vfx = null)
        {
            SkillName = skillName;
            Symbol = symbol;
            Duration = duration;
            CdTime = cdTime;
            this.direction = direction;
            Knockback = knockback;
            OffsetPos = offsetPos;
            VFX = vfx;
        }

        public override string ToString() =>
            $"--{GetType().Name}--\n"+
            this.GetMembersToNameString().ArrayToString("\n",false);
    }
}