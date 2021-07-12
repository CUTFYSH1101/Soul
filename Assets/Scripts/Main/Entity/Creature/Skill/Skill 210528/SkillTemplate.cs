using Main.Attribute;
using Main.Common;
using UnityEngine;
using static Main.Attribute.DictionaryAudioPlayer;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity
{
    public class SkillTemplate
    {
        private readonly AbstractCreature creature;
        public AbstractCreature GetCreature() => creature;
        public CreatureAttr GetCreatureAttr() => creature.GetCreatureAttr();
        public Rigidbody2D GetRigidbody2D() => creature.GetRigidbody2D();
        public CreatureAnimManager GetAnimator() => creature.GetAnimator();

        public MindState MindState
        {
            get => creature.GetCreatureAttr().MindState;
            set => creature.GetCreatureAttr().MindState = value;
        }

        public SkillName SkillName
        {
            get => creature.GetCreatureAttr().SkillName;
            set => creature.GetCreatureAttr().SkillName = value;
        }

        public bool IsTag(string tag) => creature.GetAnimator().IsTag(tag);

        public void AddForce_OnActive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force) =>
            creature.GetRigidbody2D().AddForce_OnActive(force, mode2D);

        public bool IsFacingRight => creature.IsFacingRight;
        public float GetDirX => IsFacingRight ? 1 : -1;

        public void Play(Key key) =>
            creature.GetAudioPlayer()?.Play(creature.GetAudioPlayer().AudioSource, key);

        public void Play(Symbol key) =>
            creature.GetAudioPlayer()?.Play(creature.GetAudioPlayer().AudioSource, key);

        public AudioSource Setting(Key key) =>
            creature.GetAudioPlayer()?.Setting(creature.GetAudioPlayer().AudioSource, key);
        
        public SkillTemplate(AbstractCreature creature)
        {
            this.creature = creature;
        }
    }
}