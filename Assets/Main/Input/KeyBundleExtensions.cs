using Main.Input.DataType;
using UnityEngine;

namespace Main.Input
{
    /// 客戶端
    public static class KeyBundleExtensions
    {
        public static KeyBundle Add(this KeyBundle origin, EnumJoyStick key) => 
            origin.Add(new ExtraJoystick(PorN.Positive, key));

        public static KeyBundle Add(this KeyBundle origin, KeyCode key) =>
            origin.Add(new ExtraKey(PorN.Positive, key));

        public static KeyBundle Add(this KeyBundle origin, KeyCode posKey, KeyCode negKey) =>
            origin.Add(new ExtraKey(PorN.Positive, posKey))
                .Add(new ExtraKey(PorN.Negative, negKey));

        public static KeyBundle Add(this KeyBundle origin, int key) =>
            origin.Add(new ExtraMouse(PorN.Positive, key));

        public static KeyBundle Add(this KeyBundle origin, int posKey, int negKey) =>
            origin.Add(new ExtraMouse(PorN.Positive, posKey))
                .Add(new ExtraMouse(PorN.Negative, negKey));
    }
}