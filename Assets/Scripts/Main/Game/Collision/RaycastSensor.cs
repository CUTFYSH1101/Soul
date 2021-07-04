using System;
using System.Linq;
using JetBrains.Annotations;
using Main.Common;
using Main.Entity;
using Main.Util;
using UnityEngine;

namespace Main.Game.Collision
{
    public class RaycastSensor
    {
        [CanBeNull]
        public Profile GetProfile() => currentProfile;

        // 把條件式整理設定進去
        public Profile[] GetFilteredCreatureAll() =>
            eyeTrans.AnyInView(size.x, size.y, AllFilter)
                .Select(hit2D => hit2D.GetComponent<Profile>())
                .ToArray();

        [CanBeNull]
        public Profile GetFilteredCreatureFirst() =>
            (Profile) GetFilteredCreatureAll().FirstOrNull();

        // currentProfile = filteredCreature;
        public void Update([CanBeNull] Profile filteredCreature) =>
            currentProfile = filteredCreature.IsEmpty() ? null : filteredCreature;

        /// 每個單位的判斷式
        private bool AllFilter(Collider2D hit2D)
        {
            if (filter == null)
                return hit2D.GetComponent<Profile>().Get(p => !p.IsKilled()) != null;
            return hit2D.GetComponent<Profile>().Get(p => !p.IsKilled() && filter(p)) != null;
        }

        [CanBeNull] private readonly Func<Profile, bool> filter;

        // ======
        // 參數set
        // ======
        [NotNull] private readonly Transform eyeTrans;
        private readonly Vector2 size;

        public RaycastSensor([NotNull] Transform eyeTrans,
            [NotNull] Func<bool> isFacingRight, // 動態方向
            Vector2 size,
            [NotNull] Func<Profile, bool> filter)
        {
            this.eyeTrans = eyeTrans;
            this.size = size;
            this.filter = filter;
        }

        private Profile currentProfile;
    }

    // 收納相同步驟
    public abstract class AbstractAnyInView
    {
        private readonly RaycastSensor raycastSensor;
        public Vector2 Size { get; }

        protected AbstractAnyInView([NotNull] Transform eyeTrans,
            [NotNull] Func<bool> isFacingRight,
            Vector2 size, Func<Profile, bool> filter) =>
            raycastSensor = new RaycastSensor(eyeTrans, isFacingRight, Size = size, filter);

        public Action<AbstractCreature> SetTarget { protected get; set; }

        public bool UpdateCreatureInView()
        {
            raycastSensor.Update(raycastSensor.GetFilteredCreatureFirst());
            SetTarget?.Invoke(raycastSensor.GetProfile()?.GetCreature());
            return raycastSensor.GetProfile() != null;
        }
    }

    public class AnyEnemyInView : AbstractAnyInView
    {
        public AnyEnemyInView([NotNull] Transform eyeTrans, [NotNull] Func<bool> isFacingRight, Vector2 size,
            Team team) : base(eyeTrans, isFacingRight, size, p => p.IsEnemy(team))
        {
        }
    }

    public class AnyCreatureInView : AbstractAnyInView
    {
        public AnyCreatureInView([NotNull] Transform eyeTrans, [NotNull] Func<bool> isFacingRight, Vector2 size) :
            base(eyeTrans, isFacingRight, size, null)
        {
        }
    }
}