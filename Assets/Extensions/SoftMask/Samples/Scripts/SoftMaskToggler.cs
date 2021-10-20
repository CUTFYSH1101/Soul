using UnityEngine;
using UnityEngine.UI;

namespace Extensions.SoftMask.Samples.Scripts {
    public class SoftMaskToggler : MonoBehaviour {
        public GameObject mask;
        public bool doNotTouchImage = false;

        public void Toggle(bool enabled) {
            if (mask) {
                mask.GetComponent<SoftMasking.SoftMask>().enabled = enabled;
                mask.GetComponent<Mask>().enabled = !enabled;
                if (!doNotTouchImage)
                    mask.GetComponent<Image>().enabled = !enabled;
            }
        }
    }
}
