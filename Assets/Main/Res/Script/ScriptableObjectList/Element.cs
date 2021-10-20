using System;

namespace Main.Res.Script.ScriptableObjectList
{
    [Serializable]
    public class Element<T, TY> where TY : class
    {
        public T key;
        public TY value;
    }
}