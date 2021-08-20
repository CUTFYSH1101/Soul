using System;
using System.Collections.Generic;
using Main.Util;

namespace Main.Entity
{
    public class IEntity
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

        private readonly List<IComponent> _components = new List<IComponent>();
        private readonly List<Action> _updateList = new List<Action>();
        
        public virtual void Update() => 
            _updateList.ToArray().Foreach(update => update?.Invoke());

        public IComponent AppendComponent(IComponent component)
        {
            _components.Add(component);
            _updateList.Add(component.Update);
            return component;
        }

        public IComponent RemoveComponent(IComponent component)
        {
            _components.Remove(component);
            _updateList.Remove(component.Update);
            return component;
        }
    }
}