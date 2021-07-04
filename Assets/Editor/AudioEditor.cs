using System;
using UnityEditor;
using UnityEngine;
using Main.Attribute;
using Main.Util;

namespace Editor
{
    [InitializeOnLoad, CustomEditor(typeof(AudioPlayer))]
    public class AudioEditor : UnityEditor.Editor
    {
        private AudioSource audioSource;
        private AudioSource AudioSource
        {
            get
            {
                if (audioSource == false)
                    audioSource = UnityTool.CreateAudioSource();
                return audioSource;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AudioPlayer audioPlayer = (AudioPlayer) target;
            if (GUILayout.Button("preview"))
            {
                audioPlayer.Play(AudioSource);
            }
        }
    }
}