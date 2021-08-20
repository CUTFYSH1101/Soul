using UnityEngine;
using UnityEngine.UI;

namespace SoftMask.Samples.Scripts {
    public class SoftMaskToggler : MonoBehaviour {
        public GameObject mask;
        public bool doNotTouchImage = false;

        public void Toggle(bool enabled) {
            if (mask) {
                mask.GetComponent<SoftMask.Scripts.SoftMask>().enabled = enabled;
                mask.GetComponent<Mask>().enabled = !enabled;
                if (!doNotTouchImage)
                    mask.GetComponent<Image>().enabled = !enabled;
            }
        }
    }
}
