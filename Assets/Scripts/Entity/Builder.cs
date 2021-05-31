using JetBrains.Annotations;
using Main.Common;
using Main.Entity.Attr;
using Main.Entity.Controller;
using Main.Util;
using UnityEngine;
using static Main.Common.Team;
using Team = Main.Common.Team;

namespace Main.Entity
{
    public class Director
    {
        private readonly ICreatureAI creatureAI;
        private readonly Builder builder;

        public Director(Builder builder)
        {
            this.builder = builder;
            this.creatureAI = builder.GetCreatureAI();
            // 預設值
            builder.SetInit();
            builder.SetAIStateSwitch(); // 是否啟用AI
            builder.InitAIState(); // 設定狀態
            builder.SetTeam(); // 設定陣營
            builder.SetMessage(); // 連結message遊戲物件與程式
        }

        public ICreatureAI GetResult()
        {
            if (!builder.Inited)
            {
                Debug.Log("尚未初始化完畢!");
            }

            return builder.Inited ? creatureAI : null;
        }
    }

    public abstract class Builder
    {
        [NotNull] protected ICreatureAI creatureAI; // switch-case給予
        internal ICreatureAI GetCreatureAI() => creatureAI;
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
            player = new Player(
                new ICreatureAttr(jumpForce: 1800),
                transform);
            creatureAI = new PlayerAI(player);
            SetController();
        }

        private void SetController()
        {
            ((PlayerAI) creatureAI)?.SetController(new PlayerController(player));
            Inited = !((PlayerAI) creatureAI)?.GetController().IsEmpty() ?? false;
        }

        public override void SetInit()
        {
            creatureAI.Init(player, new PlayerStrategy(creatureAI), 0, 0, Team.Player);
        }

        public override void InitAIState()
        {
            creatureAI.ChangeAIState(new IdleAIState());
        }

        public override void SetAIStateSwitch()
        {
            creatureAI.SetSwitch(true);
        }

        public override void SetTeam()
        {
            creatureAI.SetTeam(Team.Player);
        }

        public override void SetMessage()
        {
            creatureAI.GetMessage().Init(creatureAI);
        }
    }

    public class MonsterBuilder : Builder
    {
        private readonly Monster monster;

        public MonsterBuilder(Transform transform)
        {
            monster = new Monster(
                new ICreatureAttr(jumpForce: 400, moveSpeed: 3),
                transform);
            creatureAI = new MonsterAI(monster);
        }

        public override void SetInit()
        {
            creatureAI.Init(creature: monster, aiStrategy: new MonsterStrategy(creatureAI), team: Enemy);
        }

        public override void InitAIState()
        {
            creatureAI.ChangeAIState(new IdleAIState());
        }

        public override void SetAIStateSwitch()
        {
            creatureAI.SetSwitch(true);
        }

        public override void SetTeam()
        {
            creatureAI.SetTeam(Enemy);
        }

        public override void SetMessage()
        {
            creatureAI.GetMessage().Init(creatureAI);
            Inited = true;
        }
    }
}