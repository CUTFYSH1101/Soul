using UnityEngine;

namespace Main.Blood {
    [CreateAssetMenu( fileName = "BloodElementsRegistrationList", menuName = "Registration List/Blood Element", order = 1)]
    public class BloodElementsRegistrationList : ScriptableObject {
        [Header ( "Monster Using" )]
        public GameObject c250Circle;
        public GameObject c250Crossx;
        public GameObject c250Square;
        public GameObject x250Circle;
        public GameObject x250Crossx;
        public GameObject x250Square;

        [Header ( "Graphic Interface Using" )]
        public GameObject c375Circle;
        public GameObject c375Crossx;
        public GameObject c375Square;
        public GameObject x375Circle;
        public GameObject x375Crossx;
        public GameObject x375Square;
    }
}