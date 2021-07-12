using Main.Attribute;
using UnityEngine;

namespace Main.Util
{
    public static class UnityAudioTool
    {
        public static DictionaryAudioPlayer GetNormalAttackAudioPlayer() =>
            Resources.Load<DictionaryAudioPlayer>("Audios/NAAudioPlayer");
    }
}