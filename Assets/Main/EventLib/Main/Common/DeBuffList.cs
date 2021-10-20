using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.EventLib.Common
{
    [Serializable]
    public class DeBuffList
    {
        [SerializeField] private List<EnumDebuff> list = new();

        public EnumDebuff? GetCurrentDeBuffs(int index)
        {
            if (!list.Any() || list.Count <= index || index < 0) return null;
            return list[index];
        }

        public EnumDebuff[] GetCurrentDeBuffs() => !list.Any() ? null : list.ToArray();

        public void Append(EnumDebuff newDebuff) => list.Add(newDebuff);

        public void Remove(EnumDebuff debuff) => list.Remove(debuff);

        public bool DuringDeBuff => list.Any();
    }
}