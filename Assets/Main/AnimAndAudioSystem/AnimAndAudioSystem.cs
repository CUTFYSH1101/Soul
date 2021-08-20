using System;
using JetBrains.Annotations;
using Main.AnimAndAudioSystem.Anims.Scripts;
using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.AnimAndAudioSystem.Main.Common;
using Main.EventSystem.Util;
using UnityEngine;

namespace Main.AnimAndAudioSystem
{
    [Serializable]
    public class AnimAndAudioSystem
    {
        private readonly (CreatureAnimManager animManager, DictionaryAudioPlayer audioPlayer) _instance;

        public AnimAndAudioSystem([NotNull] Animator animator, DictionaryAudioPlayer audioPlayer)
        {
            _instance.audioPlayer = audioPlayer;
            _instance.animManager = new CreatureAnimManager(animator);
        }
        
        public AnimAndAudioSystem([NotNull] Animator animator, string resPathAudioPlayer)
        {
            _instance.audioPlayer = UnityRes.GetNormalAttackAudioPlayer(resPathAudioPlayer);
            _instance.animManager = new CreatureAnimManager(animator);
        }

        public void Play(EnumSymbol symbol)
        {
            if (_instance.audioPlayer != null)
                _instance.audioPlayer.Play(symbol);
        }

        public void Play(DictionaryAudioPlayer.Key key)
        {
            if (_instance.audioPlayer != null)
                _instance.audioPlayer.Play(key);
        }

        public void Play(DictionaryAudioPlayer.Key key, float nowDistance, float maxDistance)
        {
            if (_instance.audioPlayer != null)
                _instance.audioPlayer.Play(key, nowDistance, maxDistance);
        }

        public void SetAttackSpeed(float value) => _instance.animManager.SetAttackSpeed(value);
        public void Knockback(bool @switch) => _instance.animManager.Knockback(@switch);
        public void DiveAttack(EnumSymbol type, bool @switch) => _instance.animManager.DiveAttack(type, @switch);
        public void SpurAttack(EnumSymbol type) => _instance.animManager.SpurAttack(type);
        public void Attack(EnumSymbol type) => _instance.animManager.Attack(type);
        public void Move(bool @switch) => _instance.animManager.Move(@switch);
        public void Dash(bool @switch) => _instance.animManager.Dash(@switch);
        public void JumpUpdate(int speedY) => _instance.animManager.JumpUpdate(speedY);
        public void WallJump() => _instance.animManager.WallJump();
        public void JumpAttack(EnumSymbol symbol) => _instance.animManager.JumpAttack(symbol);
        public void Killed() => _instance.animManager.Killed();
        public void Revival() => _instance.animManager.Revival();
        public void Interrupt() => _instance.animManager.Interrupt();
    }
}