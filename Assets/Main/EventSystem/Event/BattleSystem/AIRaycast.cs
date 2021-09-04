using System;
using System.Linq;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.Game.Collision;
using Main.Util;
using UnityEngine;

namespace Main.EventSystem.Event.BattleSystem
{
    public abstract class AIRaycast 
    {
        // ======
        // 參數set
        // ======
        [NotNull] private readonly Transform _dynEyePos;
        [NotNull] private readonly Vector2 _size;
        [CanBeNull] private readonly Func<IMediator, bool> _filter;

        public AIRaycast([NotNull] Transform dynEyePos,
            [NotNull] Vector2 size,
            [CanBeNull] Func<IMediator, bool> filter)
        {
            _dynEyePos = dynEyePos;
            _size = size;
            _filter = filter;

            // filter = mediator => mediator.IsEnemy(Team.Player);
        }

        public Vector2 Size => _size;
        public IMediator Mediator { get; private set; }
        public Action<Creature> OnSetTarget { private get; set; }

        public bool UpdateCreatureInView()
        {
            Mediator = GetAll(AllFilter()).FirstOrNull();
            OnSetTarget?.Invoke(Mediator?.Creature);
            return Mediator != null && !Mediator.IsEmpty();
        }

        // 抓取所有物件
        public Spoiler[] GetAll() =>
            _dynEyePos.AnyInView(_size.x, _size.y,
                collider2D => collider2D.GetComponent<Spoiler>());

        public IMediator[] GetAll(Func<IMediator, bool> filter) =>
            GetAll().Where(filter).ToArray();

        public Func<IMediator, bool> AllFilter() =>
            source => !source.IsKilled && (_filter == null || _filter(source));
    }

    public class AnyEnemyInView : AIRaycast
    {
        public AnyEnemyInView([NotNull] Transform dynEyePos, Vector2 size, Team selfTeam) :
            base(dynEyePos, size, p => p.IsEnemy(selfTeam))
        {
        }
    }

    public class AnyInView : AIRaycast
    {
        public AnyInView([NotNull] Transform dynEyePos, Vector2 size) :
            base(dynEyePos, size, null)
        {
        }
    }
}