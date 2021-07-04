using UnityEngine;
using UnityEngine.Events;

namespace Event
{
    public class TriggerEvent : MonoBehaviour
    {
        public UnityEvent onEnterEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onEnterEvent.Invoke();
        }
    }
}