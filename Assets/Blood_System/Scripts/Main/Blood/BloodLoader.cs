using UnityEngine;
using BERegL = Main.Blood.BloodElementsRegistrationList;

namespace Main.Blood {
    public class BloodLoader : MonoBehaviour {
        [SerializeField] [Header ( "Registration Setting" )]
        public BloodElementsRegistrationList bloodElements;
        /*public Canvas bloodPrefab; // 通用canvas
        public BloodHandler handler; // 通用腳本*/
        
        private void Start ( ) {
            Debug.Log ( "Blood handler load successfully." );
        }
    }
}