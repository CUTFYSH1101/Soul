using System;
using Main.Util;
using UnityEngine;

namespace Main.EventLib.Sub.UIEvent.QTE
{
    [CreateAssetMenu(fileName = "QteImageList", menuName = "Registration List/Qte Images", order = 1)]
    public class QteImageList : ScriptableObject
    {
        [Serializable]
        public class ImageData
        {
            public EnumQteShape name;
            public Sprite image;
            public float size;
        }

        [SerializeField] private ImageData[] images;

        public ImageData Find(int index) =>
            images.Length - 1 >= index && index >= 0 ? images[index] : null;

        public ImageData Find(EnumQteShape name) => 
            images.FirstOrNull(image => image.name == name);
    }
}