using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.CreatureAndBehavior.Behavior;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using UnityEngine;

namespace Main.Entity.Creature
{
    public class Director
    {
        private readonly Creature _result;
        private readonly Builder _builder;

        public Director(Builder builder)
        {
            _builder = builder;
            _result = builder.Creature;
            // 預設值
            builder.Init(); // 初始化、角色屬性
            builder.SetName(); // 設定角色名稱
            builder.SetAISystem(); // 是否啟用AI系統，如果是，則設定AIState初始狀態
            builder.SetBattleSystem(); // 設定陣營與戰鬥系統
        }

        public Creature GetResult()
        {
            if (!_builder.Inited)
                Debug.Log("尚未初始化完畢!");

            return _builder.Inited ? _result : null;
        }
    }

    public abstract class Builder
    {
        public Creature Creature { get; protected set; } // switch-case給予
        public bool Inited { get; protected set; }
        public abstract void Init();
        public abstract void SetName();

        /// 是否啟用AI系統，如果是，則設定AIState初始狀態。
        /// 採用AI系統時，角色會自動切換狀態。
        public abstract void SetAISystem();

        /// 設定陣營與戰鬥系統。
        /// 設定攻擊對象。
        public abstract void SetBattleSystem();
    }

    public class PlayerBuilder : Builder
    {
        public PlayerBuilder(Transform obj, DictionaryAudioPlayer audioPlayer)
        {
            Creature = new Creature(obj,
                new CreatureAttr(jumpForce: 30, dashForce: 30, diveForce: 60),
                audioPlayer);
        }

        public override void Init()
        {
            Creature.AppendComponent(new PlayerController(
                (PlayerBehavior) Creature.AppendData(new PlayerBehavior(Creature)), // 添加可以操作的行為，允許外界存取
                new CreatureInterface(Creature))); // 添加玩家控制器
        }

        public override void SetName() => Creature.CreatureAttr.Name = Creature.Transform.name; // 設定成遊戲物件的名稱

        public override void SetAISystem()
        {
        }

        public override void SetBattleSystem()
        {
            Inited = true;
        }
    }
    /*
    public class PlayerBuilder : Builder
    {
        private readonly PlayerAI player;

        public PlayerBuilder(Transform transform)
        {
            // 初始化AbstractCreature
            Creature = new Player(
                new AbstractCreatureAttr(jumpForce: 1800, dashForce: 30f),
                transform, UnityRes.GetNormalAttackAudioPlayer());
            player = new PlayerAI(Creature);
            SetController();
        }

        private void SetController()
        {
            ((Player) Creature).SetController(new PlayerController(Creature));
            Inited = !((Player) Creature)?.GetController().IsEmpty() ?? false; // 偵測是否設定成功
        }

        // 初始化AbstractCreatureAI
        public override void Init()
        {
            Creature.GetAbstractCreatureAI()
                .Init(new PlayerStrategy(Creature.GetAbstractCreatureAI()), team: Team.Player);
        }

        public override void SetName()
        {
            Creature.GetAbstractCreatureAttr().Name = Creature.GetRigidbody2D().name; // 設定成遊戲物件的名稱
        }

        public override void InitAIState()
        {
            Creature.GetAbstractCreatureAI().ChangeAIState(new IdleAIState());
        }

        public override void SetAISystem()
        {
            Creature.GetAbstractCreatureAI().SetSwitch(true);
        }

        public override void SetBattleSystem()
        {
            Creature.GetAbstractCreatureAI().SetTeam(Team.Player); // 之後改到victory

            var mediator = Creature.GetTransform()
                .GetOrAddComponent<Spoiler>()
                .Init(Creature, Team.Player);
            Creature.SetVictoryAI(mediator);
        }
    }

    public class MonsterBuilder : Builder
    {
        private readonly MonsterAI monster;

        public MonsterBuilder(Transform transform)
        {
            Creature = new Monster(
                new AbstractCreatureAttr(jumpForce: 400, moveSpeed: 3),
                transform, null);
            monster = new MonsterAI(Creature);
        }

        public override void Init()
        {
            /*protected static readonly Vector2 AttackRange = new Vector2(0.85f, 1.5f);
            protected static readonly Vector2 ChaseRange = new Vector2(5f, 1.5f);#1#
            Creature.GetAbstractCreatureAI().Init(new MonsterStrategy(monster), team: Enemy,
                chaseRange: new Vector2(5f, 1.5f), attackRange: new Vector2(0.85f, 1.5f));
        }

        public override void SetName()
        {
            Creature.GetAbstractCreatureAttr().Name = Creature.GetRigidbody2D().name; // 設定成遊戲物件的名稱
        }

        public override void InitAIState()
        {
            Creature.GetAbstractCreatureAI().ChangeAIState(new IdleAIState());
        }

        public override void SetAISystem()
        {
            Creature.GetAbstractCreatureAI().SetSwitch(true);
        }

        public override void SetBattleSystem()
        {
            Creature.GetAbstractCreatureAI().SetTeam(Enemy); // 之後改到victory

            var mediator = Creature.GetTransform()
                .GetOrAddComponent<Spoiler>()
                .Init(Creature, Team.Enemy);
            Creature.SetVictoryAI(mediator);

            Inited = true;
        }
    }
*/
}