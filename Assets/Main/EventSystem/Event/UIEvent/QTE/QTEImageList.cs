using System;
using System.Linq;
using UnityEngine;

namespace Main.EventSystem.Event.UIEvent.QTE
{
    [CreateAssetMenu(fileName = "QTEList", menuName = "QTEList", order = 1)]
    public class QteImageList : ScriptableObject
    {
        [Serializable]
        public class CompImage
        {
            public EnumQteSymbol name;
            public Sprite image;
            public Vector2 size;
        }

        [SerializeField] private CompImage[] images;

        public CompImage Get(int index) =>
            images.Length - 1 >= index && index >= 0 ? images[index] : null;

        public CompImage Get(EnumQteSymbol name)
        {
            try
            {
                var dict = images.ToDictionary(image => image.name, image => image);
                return dict.ContainsKey(name) ? dict[name] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}