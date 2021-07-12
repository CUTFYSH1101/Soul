using UnityEngine;

namespace Main.Event
{
    public class Damager : MonoBehaviour
    {
        /*public readonly SkillAttr skillAttr = new SkillAttr();
        private Vector2 direction;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherController = other.GetComponent<IHittable>();
            if (!otherController.IsEmpty())
            {
                direction = transform.root.Normalize(other);
                Debug.Log(other.skillName+direction);
                if (skillAttr.VFX.IsEmpty())
                    otherController.Hit(direction, force: 40);
                else
                    otherController.Hit(direction, force: 40, skillAttr.VFX, other.transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (Vector2) transform.position + direction);
            Gizmos.DrawSphere(direction + (Vector2) transform.position, 0.1f);
        }*/
    }
}