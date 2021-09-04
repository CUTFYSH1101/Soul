using Main.AnimAndAudioSystem.Anims.Scripts;
using Main.EventSystem.Common;
using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Game;
using UnityEngine;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    public class CreatureInterface
    {
        public string ObjName => _creature.Transform.name;
        private readonly Creature _creature;
        public Creature GetCreature() => _creature;
        public CreatureAttr GetCreatureAttr() => _creature.CreatureAttr;
        public bool IsKilled() => _creature.IsKilled();
        public bool MovableDyn => _creature.CreatureAttr.MovableDyn;
        public bool AttackableDyn => _creature.CreatureAttr.AttackableDyn;

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
        public Vector2 LookAt => new Vector2(LookAtAxisX, 0);
        public bool Grounded => _creature.CreatureAttr.Grounded;

        public Vector2 GetAbsolutePosition() => _creature.AbsolutePosition;
        public UnityRb2D GetRigidbody2D() => _creature.Rigidbody2D;
        public CreatureAnimManager GetAnimManager() => _creature.AnimManager;
        public DictionaryAudioPlayer GetAudioPlayer() => _creature.AudioPlayer;
        
        public bool IsTag(string tag) => _creature.AnimManager.IsTag(tag);

        // todo resetX, resetY, resetAll
        public void AddForce_OnActive(Vector2 force, ForceMode2D mode2D = ForceMode2D.Force) =>
            _creature.Rigidbody2D.AddForce_OnActive(force, mode2D);
        public void Play(DictionaryAudioPlayer.Key key) =>
            GetAudioPlayer()?.Play(key);

        public void Play(EnumSymbol key) =>
            GetAudioPlayer()?.Play(key);

        public CreatureInterface(Creature creature) => _creature = creature;
    }
}