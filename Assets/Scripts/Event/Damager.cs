using System;
using Main.Common;
using Main.Entity.Controller;
using Main.Util;
using UnityEngine;
using UnityEngine.Events;
using Rigidbody2D = UnityEngine.Rigidbody2D;

namespace Event
{
    public class Damager : MonoBehaviour
    {
        public Symbol symbol;
        private Vector2 direction;
        private float mass = 1;
        public Transform vfx;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherController = other.GetComponent<IHitable>();
            if (!otherController.IsEmpty())
            {
                direction = transform.root.Normalize(other);
                Debug.Log(other.name+direction);
                if (vfx.IsEmpty())
                    otherController.Hit(direction, force: 40);
                else
                    otherController.Hit(direction, force: 40, vfx, other.transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (Vector2) transform.position + direction);
            Gizmos.DrawSphere(direction + (Vector2) transform.position, 0.1f);
        }
    }
}