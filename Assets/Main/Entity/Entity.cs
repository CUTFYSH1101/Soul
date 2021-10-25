using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<EnumComponentTag, IComponent> _dictCompByTag =
            new();

        private readonly List<Action> _updateList = new();
        private readonly Dictionary<EnumDataTag, IData> _dataDictionary = new();

        public virtual void Update() =>
            _updateList.ForEach(update => update?.Invoke());

        public IComponent Append(IComponent component)
        {
            if (_dictCompByTag.ContainsKey(component.Tag))
            {
                Debug.LogError("資料已經含有相同類型的組件");
                return null;
            }

            _dictCompByTag.Add(component.Tag, component);
            _updateList.Add(component.Update);
            return component;
        }

        public bool Contains(IComponent component) => _dictCompByTag.ContainsKey(component.Tag);

        public IComponent Remove(IComponent component)
        {
            if (!_dictCompByTag.ContainsKey(component.Tag))
            {
                Debug.LogError("不含有該類型的組件");
                return null;
            }

            _dictCompByTag.Remove(component.Tag);
            _updateList.Remove(component.Update);
            return component;
        }

        public bool Contains(EnumComponentTag tag) => _dictCompByTag.ContainsKey(tag);

        public IComponent RemoveByTag(EnumComponentTag tag)
        {
            if (!_dictCompByTag.ContainsKey(tag))
            {
                Debug.LogError("不含有該類型的組件");
                return null;
            }

            var component = _dictCompByTag[tag];
            _dictCompByTag.Remove(tag);
            _updateList.Remove(component.Update);
            return component;
        }

        public IComponent FindByTag(EnumComponentTag tag) =>
            _dictCompByTag.ContainsKey(tag) ? _dictCompByTag[tag] : null;

        public T FindComponent<T>() where T : class, IComponent =>
            (T)_dictCompByTag.FirstOrDefault(element => element.Value.GetType() == typeof(T)).Value;

        public IData Append(IData data)
        {
            if (_dataDictionary.ContainsKey(data.Tag))
            {
                Debug.LogError("資料已經含有相同類型的參數");
                return null;
            }

            _dataDictionary.Add(data.Tag, data);
            return data;
        }

        public bool Contains(IData data) => _dataDictionary.ContainsKey(data.Tag);

        public IData Remove(IData data)
        {
            if (!_dataDictionary.ContainsKey(data.Tag))
            {
                Debug.LogError("不含有該類型的資料");
                return null;
            }

            _dataDictionary.Add(data.Tag, data);
            return data;
        }

        public bool Contains(EnumDataTag tag) => _dataDictionary.ContainsKey(tag);

        public IData RemoveByTag(EnumDataTag tag)
        {
            if (!_dataDictionary.ContainsKey(tag))
            {
                Debug.LogError("不含有該類型的資料");
                return null;
            }

            var data = _dataDictionary[tag];
            _dataDictionary.Remove(tag);
            return data;
        }

        public IData FindByTag(EnumDataTag tag) => _dataDictionary.ContainsKey(tag) ? _dataDictionary[tag] : null;
    }
}