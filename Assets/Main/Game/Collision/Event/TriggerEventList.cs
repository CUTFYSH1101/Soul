using System;
using Main.Entity;
using UnityEngine;

namespace Main.Game.Collision.Event
{
    public class TriggerEventList : IComponent
    {
        private readonly (TriggerEnterEvent enter, ITriggerStayEvent stay, TriggerExitEvent exit) _events;

        public (Collider2D[] enter, Collider2D[] stay, Collider2D[] exit) Others =>
            (_events.enter.Others, _events.stay.Others, _events.exit.Others);

        public TriggerEventList(Component component, Action<Collider2D> onEnter, Action<Collider2D> onExit)
        {
            _events.stay = new TriggerStayEvent(component);
            _events.enter = new TriggerEnterEvent(_events.stay, onEnter);
            _events.exit = new TriggerExitEvent(_events.stay, onExit);
        }

        public bool IsEnter => _events.enter.IsTrigger;
        public bool IsStay => _events.stay.IsTrigger;
        public bool IsExit => _events.exit.IsTrigger;
        public EnumComponentTag Tag => EnumComponentTag.PhysicsCollisionSystem;

        public void Update()
        {
            _events.enter.Update();
            _events.exit.Update();
        }
    }
}