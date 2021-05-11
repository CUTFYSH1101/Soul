using System;
using Dialogue;
using Test;
using UnityEngine;

namespace DefaultNamespace {
    public class Test : MonoBehaviour {
        public DialogueGenerator DialogueIn;
        public Chara character;
        private void Start () {
            character = new Chara ();
            character.SetName ("Charaa");
            DialogueIn.CheckHeadNPC (character);
        }

        private void Update () {
            
        }
    }
}
