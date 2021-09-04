using System;
using System.Collections.Generic;
using Main.Util;
using UnityEngine;

namespace Main.Entity
{
    public class Entity
    {
        private int _id;
        public int Id
        {
            get
            {
                if (_id == default) _id = GetHashCode();
                return _id;
            }
        }

        private readonly List<IComponent> _componentList = new List<IComponent>();
        private readonly List<Action> _updateList = new List<Action>();
        private readonly Dictionary<EnumDataTag, IData> _dataDictionary = new Dictionary<EnumDataTag, IData>();
        
        public virtual void Update() => 
            _updateList.ToArray().Foreach(update => update?.Invoke());

        public IComponent AppendComponent(IComponent component)
        {
            _componentList.Add(component);
            _updateList.Add(component.Update);
            return component;
        }

        public IComponent RemoveComponent(IComponent component)
        {
            _componentList.Remove(component);
            _updateList.Remove(component.Update);
            return component;
        }

        public IData AppendData(IData data)
        {
            if (_dataDictionary.ContainsKey(data.Tag))
            {
                Debug.LogError("資料已經含有相同類型的參數");
                return null;
            }
            _dataDictionary.Add(data.Tag,data);
            return data;
        }

        public IData FindDataByTag(EnumDataTag tag) => _dataDictionary.ContainsKey(tag) ? _dataDictionary[tag] : null;
    }
}