using System;
using System.Linq;
using JetBrains.Annotations;
using Main.Game.Collision;
using Main.Util;
using Main.Entity;
using Main.Entity.Creature;
using UnityEngine;

namespace Main.EventLib.Sub.BattleSystem
{
    public abstract class EnvironmentCheck
    {
        // ======
        // 參數set
        // ======
        [NotNull] private readonly Transform _dynEyePos;
        [NotNull] private readonly Vector2 _sensingRange;
        [CanBeNull] private readonly Func<IMediator, bool> _filter;

        public EnvironmentCheck([NotNull] Transform dynEyePos,
            [NotNull] Vector2 sensingRange,
            [CanBeNull] Func<IMediator, bool> filter)
        {
            _dynEyePos = dynEyePos;
            _sensingRange = sensingRange;
            _filter = filter;

            // filter = mediator => mediator.IsEnemy(Team.Player);
        }

        public Vector2 SensingRange => _sensingRange;
        public IMediator Mediator { get; private set; }
        public Action<Creature> OnSetTarget { private get; set; }

        public bool UpdateCreatureInView()
        {
            Mediator = GetAll(AllFilter()).FirstOrNull();
            OnSetTarget?.Invoke(Mediator?.Creature);
            return Mediator != null && !Mediator.IsEmpty();
        }

        /*
        public Spoiler[] GetAll() =>
            _dynEyePos.AnyInView(_sensingRange.x, _sensingRange.y,
                collider2D => collider2D.GetComponent<Spoiler>());
        */
        // 抓取所有物件
        public Spoiler[] GetAll() =>
            (Spoiler[]) _dynEyePos.AnyInView(_sensingRange.x, _sensingRange.y, collider2D =>
                    (Spoiler) BattleInterface.FindComponent(collider2D.transform,
                        EnumComponentTag.BattleSpoilerSystem));

        public IMediator[] GetAll(Func<IMediator, bool> filter) =>
            GetAll()?.Where(filter).ToArray();

        public Func<IMediator, bool> AllFilter() =>
            source => !source.IsKilled && (_filter == null || _filter(source));
    }

    public class AnyEnemyInView : EnvironmentCheck
    {
        public AnyEnemyInView([NotNull] Transform dynEyePos, Vector2 sensingRange, Team selfTeam) :
            base(dynEyePos, sensingRange, p => p.IsEnemy(selfTeam))
        {
        }
    }

    public class AnyInView : EnvironmentCheck
    {
        public AnyInView([NotNull] Transform dynEyePos, Vector2 sensingRange) :
            base(dynEyePos, sensingRange, null)
        {
        }
    }
}