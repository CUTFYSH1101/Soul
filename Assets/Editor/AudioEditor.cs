using Main.AnimAndAudioSystem.Audios.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad, CustomEditor(typeof(AudioPlayer))]
    public class AudioEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var audioPlayer = (AudioPlayer) target;
            if (GUILayout.Button("preview")) 
                audioPlayer.Play(audioPlayer.AudioSource);
        }
    }
}