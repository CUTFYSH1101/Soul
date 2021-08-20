using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.EventSystem.Common
{
    [Serializable]
    public class DeBuffList
    {
        [SerializeField] private List<DeBuff> list = new List<DeBuff>();

        public DeBuff? Get(int index)
        {
            if (!list.Any() || list.Count <= index || index < 0) return null;
            return list[index];
        }

        public DeBuff[] Get() => !list.Any() ? null : list.ToArray();

        public void Append(DeBuff newDeBuff) => list.Add(newDeBuff);

        public void Remove(DeBuff deBuff) => list.Remove(deBuff);

        public bool DuringDeBuff => list.Any();
    }
}