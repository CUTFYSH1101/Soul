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
        [NotNull] protected AbstractCreatureAI AbstractCreatureAI; // switch-case給予
        internal AbstractCreatureAI GetCreatureAI() => AbstractCreatureAI;
        public bool Inited { get; protected set; }

        public abstract void SetInit();

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
                new ICreatureAttr(jumpForce: 1800, dashForce: 30f),
                transform, Resources.Load<DictionaryAudioPlayer>("Audios/NAAudioPlayer"));
            AbstractCreatureAI = new PlayerAI(player);
            SetController();
        }

        private void SetController()
        {
            ((PlayerAI) AbstractCreatureAI).SetController(new PlayerController(player));
            Inited = !((PlayerAI) AbstractCreatureAI)?.GetController().IsEmpty() ?? false; // 偵測是否設定成功
        }

        // 初始化creatureAI
        public override void SetInit()
        {
            AbstractCreatureAI.Init(new PlayerStrategy(AbstractCreatureAI), team: Team.Player);
        }

        public override void InitAIState()
        {
            AbstractCreatureAI.ChangeAIState(new IdleAIState());
        }

        public override void SetAIStateSwitch()
        {
            AbstractCreatureAI.SetSwitch(true);
        }

        public override void SetTeam()
        {
            AbstractCreatureAI.SetTeam(Team.Player);
        }

        public override void SetMessage()
        {
            AbstractCreatureAI.GetProfile().Init(AbstractCreatureAI);
        }
    }

    public class MonsterBuilder : Builder
    {
        private readonly Monster monster;

        public MonsterBuilder(Transform transform)
        {
            monster = new Monster(
                new ICreatureAttr(jumpForce: 400, moveSpeed: 3),
                transform, null);
            AbstractCreatureAI = new MonsterAI(monster);
        }

        public override void SetInit()
        {
            /*protected static readonly Vector2 AttackRange = new Vector2(0.85f, 1.5f);
            protected static readonly Vector2 ChaseRange = new Vector2(5f, 1.5f);*/
            AbstractCreatureAI.Init(new MonsterStrategy(AbstractCreatureAI), team: Enemy,
                chaseRange: new Vector2(5f, 1.5f), attackRange: new Vector2(0.85f, 1.5f));
        }

        public override void InitAIState()
        {
            AbstractCreatureAI.ChangeAIState(new IdleAIState());
        }

        public override void SetAIStateSwitch()
        {
            AbstractCreatureAI.SetSwitch(true);
        }

        public override void SetTeam()
        {
            AbstractCreatureAI.SetTeam(Enemy);
        }

        public override void SetMessage()
        {
            AbstractCreatureAI.GetProfile().Init(AbstractCreatureAI);
            Inited = true;
        }
    }
}