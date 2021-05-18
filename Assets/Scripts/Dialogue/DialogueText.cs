using Test;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "Dia_", menuName = "Dialogue/New Dialogue", order = 1)]
    public class DialogueGenerator : ScriptableObject {
        public string DialogueID;
        public string DialogueHeadNPC;
        public DialogueData[] DialogueDetail;
        [System.Serializable] public struct DialogueData {
            public string Name;
            public string Text;
            public EMood Mood;
            public bool IsBranch;
            public string[] Branches;
            public DialogueData (EMood mood = EMood.NORMAL) {
                Mood = mood;
                Name = "";
                Text = "";
                IsBranch = false;
                Branches = new string[] { };
            }
        }

        public void CheckHeadNPC (Chara charaIn) {
            if (!(charaIn.GetName ().Equals (DialogueHeadNPC))) {
                Debug.LogError ($"Dialogue head npc: {DialogueHeadNPC} is wrong. Please recheck the data or change params on func."
                + "\n" + $"對話起頭NPC: {DialogueHeadNPC}有誤，請再次查核資料或更換函式參數。");
                return;
            }
            Debug.Log ("Dialogue head npc is correct." + "\n" + "對話起頭NPC檢核正確。");
        }
    }
}
