using JetBrains.Annotations;
using Main.Attribute;
using Main.Entity.Controller;
using Main.Util;
using UnityEngine;
using static Main.Common.Team;
using Team = Main.Common.Team;

namespace Main.Entity
{
    public class Director
    {
        private readonly AbstractCreatureAI abstractCreatureAI;
        private readonly Builder builder;

        public Director(Builder builder)
        {
            this.builder = builder;
            this.abstractCreatureAI = builder.GetCreatureAI();
            // 預設值
            builder.SetInit();
            builder.SetName();
            builder.SetAIStateSwitch(); // 是否啟用AI
            builder.InitAIState(); // 設定狀態
            builder.SetTeam(); // 設定陣營
            builder.SetMessage(); // 連結message遊戲物件與程式
        }

        public AbstractCreatureAI GetResult()
        {
            if (!builder.Inited)
            {
                Debug.Log("尚未初始化完畢!");
            }

            return builder.Inited ? abstractCreatureAI : null;
        }
    }

    public abstract class Builder
    {
        [NotNull] protected AbstractCreatureAI CreatureAI; // switch-case給予
        internal AbstractCreatureAI GetCreatureAI() => CreatureAI;
        public bool Inited { get; protected set; }

        public abstract void SetInit();
        public abstract void SetName();

        public abstract void InitAIState();

        /// 設定是否要採納AI模式，自動切換狀態
        public abstract void SetAIStateSwitch();

        /// 設定攻擊對象
        public abstract void SetTeam();

        /// 傳遞攻擊對象
        public abstract void SetMessage();
    }

    public class PlayerBuilder : Builder
    {
        private readonly Player player;

        public PlayerBuilder(Transform transform)
        {
            // 初始化creature
            player = new Player(
                new CreatureAttr(jumpForce: 1800, dashForce: 30f),
                transform, UnityAudioTool.GetNormalAttackAudioPlayer());
            CreatureAI = new PlayerAI(player);
            SetController();
        }

        private void SetController()
        {
            ((PlayerAI) CreatureAI).SetController(new PlayerController(player));
            Inited = !((PlayerAI) CreatureAI)?.GetController().IsEmpty() ?? false; // 偵測是否設定成功
        }

        // 初始化creatureAI
        public override void SetInit()
        {
            CreatureAI.Init(new PlayerStrategy(CreatureAI), team: Team.Player);
        }

        public override void SetName()
        {
            player.GetCreatureAttr().Name = player.GetRigidbody2D().name; // 設定成遊戲物件的名稱
        }

        public override void InitAIState()
        {
            CreatureAI.ChangeAIState(new IdleAIState());
        }

        public override void SetAIStateSwitch()
        {
            CreatureAI.SetSwitch(true);
        }

        public override void SetTeam()
        {
            CreatureAI.SetTeam(Team.Player);
        }

        public override void SetMessage()
        {
            CreatureAI.GetProfile().Init(CreatureAI);
        }
    }

    public class MonsterBuilder : Builder
    {
        private readonly Monster monster;

        public MonsterBuilder(Transform transform)
        {
            monster = new Monster(
                new CreatureAttr(jumpForce: 400, moveSpeed: 3),
                transform, null);
            CreatureAI = new MonsterAI(monster);
        }

        public override void SetInit()
        {
            /*protected static readonly Vector2 AttackRange = new Vector2(0.85f, 1.5f);
            protected static readonly Vector2 ChaseRange = new Vector2(5f, 1.5f);*/
            CreatureAI.Init(new MonsterStrategy(CreatureAI), team: Enemy,
                chaseRange: new Vector2(5f, 1.5f), attackRange: new Vector2(0.85f, 1.5f));
        }

        public override void SetName()
        {
            monster.GetCreatureAttr().Name = monster.GetRigidbody2D().name; // 設定成遊戲物件的名稱
        }

        public override void InitAIState()
        {
            CreatureAI.ChangeAIState(new IdleAIState());
        }

        public override void SetAIStateSwitch()
        {
            CreatureAI.SetSwitch(true);
        }

        public override void SetTeam()
        {
            CreatureAI.SetTeam(Enemy);
        }

        public override void SetMessage()
        {
            CreatureAI.GetProfile().Init(CreatureAI);
            Inited = true;
        }
    }
}