using Main.Res.Script.Audio;
using UnityEditor;
using UnityEngine;

namespace Main.Res.Editor
{
    [InitializeOnLoad, CustomEditor(typeof(DemoPlayer))]
    public class DemoAudioEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var audioPlayer = (DemoPlayer) target;
            if (GUILayout.Button("preview")) 
                audioPlayer.Play(audioPlayer.AudioSource);
        }
    }
}