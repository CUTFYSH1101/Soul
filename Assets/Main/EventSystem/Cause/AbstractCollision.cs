using JetBrains.Annotations;
using UnityEngine;

namespace Main.EventSystem.Cause
{
    /// <summary>
    /// 是否碰撞。仰賴Unity Vector
    /// </summary>
    /// <typeparam skillName="T"></typeparam>
    public abstract class AbstractCollision<T> : ICause
    {
        protected T Target;
        protected T Subject;
        protected abstract Vector2 TargetPos();
        protected abstract Vector2 SubjectPos();
        protected float Radius = 0.1f;
        [CanBeNull] protected string LayerName;

        public bool AndCause()
        {
            return SubCause();
        }

        public void Reset()
        {
        }

        protected abstract bool SubCause();
    }
}