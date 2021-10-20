using System.Collections.Generic;
using System.Linq;
using Main.Util;
using UnityEngine;

namespace Main.Res.Script.ScriptableObjectList
{
    public class AbsObjList<T, TY> : ScriptableObject where TY : class
    {
        [SerializeField] protected Element<T, TY>[] array;
        private Dictionary<T, TY> _dict = new();

        public TY Find(T key)
        {
            if (_dict.Count != array.Length) _dict = array.ToDictionary(e => e.key, e => e.value);
            return _dict.ContainsKey(key) ? _dict[key] : null;
        }

        public void Append(T key, TY value)
            => array.Add(new Element<T, TY> { key = key, value = value });
    }
}