using System;
using Main.Common;
using Main.Entity.Attr;
using Main.Util;
using UnityEngine;

namespace Main.Entity.Controller
{
    [Serializable]
    public class MonsterAI : ICreatureAI
    {
        public MonsterAI(ICreature creature, float attackRange = 0.4f, float chaseRange = 2, Team team = Team.Peace) : base(creature, attackRange, chaseRange, team)
        {
        }
        
    }
    [Serializable]
    public class Monster : ICreature
    {
        protected internal Monster(ICreatureAttr creatureAttr, Transform transform) : base(creatureAttr, transform)
        {
        }
    }
}