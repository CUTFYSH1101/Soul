using System;
using Main.Attribute;
using Main.Common;
using Main.Event;
using UnityEngine;
using static Main.Attribute.DictionaryAudioPlayer;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity
{
    public class SkillTemplate
    {
        private readonly AbstractCreature abstractCreature;

        /*public AbstractCreature GetTarget() => abstractCreature.GetCreatureAI().GetAIState().GetTarget();
        public IAIState GetAIState() => abstractCreature.GetCreatureAI().GetAIState();*/
        public AbstractCreature GetCreature() => abstractCreature;
        public ICreatureAttr GetCreatureAttr() => abstractCreature.GetCreatureAttr();
        public Rigidbody2D GetRigidbody2D() => abstractCreature.GetRigidbody2D();
        public CreatureAnimManager GetAnimator() => abstractCreature.GetAnimator();

        public MindState MindState
        {
            get => abstractCreature.GetCreatureAttr().MindState;
            set => abstractCreature.GetCreatureAttr().MindState = value;
        }

        public bool IsTag(string tag) => abstractCreature.GetAnimator().IsTag(tag);

        public void AddForce_OnActive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force) =>
            abstractCreature.GetRigidbody2D().AddForce_OnActive(force, mode2D);

        public bool IsFacingRight => abstractCreature.IsFacingRight;
        public float GetDirX => IsFacingRight ? 1 : -1;

        public void Play(Key key) =>
            abstractCreature.GetAudioPlayer().Play(abstractCreature.GetAudioPlayer().AudioSource, key);

        public void Play(Symbol key) =>
            abstractCreature.GetAudioPlayer().Play(abstractCreature.GetAudioPlayer().AudioSource, key);

        public AudioSource Setting(Key key) =>
            abstractCreature.GetAudioPlayer().Setting(abstractCreature.GetAudioPlayer().AudioSource, key);

        private readonly ICause totalDuration;
        public Func<bool> DurationCause => totalDuration.Cause;

        public void Reset()
        {
            if (totalDuration.Cause())
                totalDuration.Reset();
        }

        public SkillTemplate(AbstractCreature abstractCreature, float duration)
        {
            this.abstractCreature = abstractCreature;
            totalDuration = new CDCause(duration);
        }
    }
}