using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Game;
using Main.Res.CharactersRes.Animations.Scripts;
using Main.Res.Script;
using Main.Res.Script.Audio;
using UnityEngine;

namespace Main.EventLib.Sub.CreatureEvent.Skill
{
    public class CreatureInterface
    {
        public string ObjName => _creature.Transform.name;
        private readonly Creature _creature;
        public Creature GetCreature() => _creature;
        public CreatureAttr GetAttr() => _creature.CreatureAttr;
        public bool IsKilled() => _creature.IsKilled();
        public bool MovableDyn => _creature.CreatureAttr.EnableMoveDyn;
        public bool AttackableDyn => _creature.CreatureAttr.EnableAttackDyn;
        // public void InterruptMoving() => new CreatureBehaviorInterface(_creature).InterruptMoving();
        public EnumMindState MindState
        {
            get => _creature.CreatureAttr.MindState;
            set => _creature.CreatureAttr.MindState = value;
        }

        public EnumSkillTag CurrentSkill
        {
            get => _creature.CreatureAttr.CurrentSkill;
            set => _creature.CreatureAttr.CurrentSkill = value;
        }
        public bool IsFacingRight => _creature.IsFacingRight;
        public float LookAtAxisX => IsFacingRight ? 1 : -1;
        public Vector2 LookAt => new(LookAtAxisX, 0);
        public bool Grounded => _creature.CreatureAttr.Grounded;

        public Vector2 GetAbsolutePosition() => _creature.AbsolutePosition;
        public ProxyUnityRb2D GetRb2D() => _creature.Rigidbody2D;
        public CreatureAnimInterface GetAnim() => _creature.AnimInterface;
        public DictAudioPlayer GetAudioPlayer() => _creature.AudioPlayer;
        
        public bool IsTag(string tag) => _creature.AnimInterface.IsTag(tag);

        // todo resetX, resetY, resetAll
        public void AddForce_OnActive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force) =>
            _creature.Rigidbody2D.AddForce_OnActive(force, mode2D);
        public void Play(DictAudioPlayer.Key key) =>
            GetAudioPlayer()?.Play(key);

        public void Play(EnumShape key) =>
            GetAudioPlayer()?.Play(key);

        public CreatureInterface(Creature creature) => _creature = creature;
    }
}