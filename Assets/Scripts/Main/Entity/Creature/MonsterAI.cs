using System;
using Main.Attribute;
using UnityEngine;

namespace Main.Entity.Controller
{
    [Serializable]
    public class MonsterAI : AbstractCreatureAI
    {
        /*public MonsterAI(AbstractCreature abstractCreature, float attackRange = 0.4f, float chaseRange = 2, Team team = Team.Peace) :
            base(abstractCreature, attackRange, chaseRange, team)
        {
        }*/
        public MonsterAI(AbstractCreature abstractCreature) : base(abstractCreature)
        {
        }
    }

    [Serializable]
    public class Monster : AbstractCreature
    {
        protected internal Monster(ICreatureAttr creatureAttr, Transform transform, DictionaryAudioPlayer audioPlayer) :
            base(creatureAttr, transform, audioPlayer)
        {
        }
    }
}