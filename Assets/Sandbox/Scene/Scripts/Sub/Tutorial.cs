using Main.CreatureAI.Sub;
using Main.Entity;
using Main.EventLib.Sub.BattleSystem;
using Main.Game.Collision;
using Main.Input;
using Test.Scene.Scripts.Main.SceneStateScript;
using static Main.CreatureAI.EnumCreatureStateTag;
using static Main.Entity.Creature.Factory.EnumCreatureTag;

namespace Test.Scene.Scripts.Sub
{
    public class Tutorial : SceneStateScript
    {
        private CommonInputSystem _commonInputListener; // 監聽部分按鍵事件

        public override void Enter()
        {
            // 目標：
            // 角色idle
            // 按下m可以開啟小地圖
            // 按下esc會關閉遊戲

            CreatureSystem.Instance.Init();
            CollisionManager.ThreeLevelGroundSetting(); // 三層地板

            // 把主角的狀態變更為idle，使無法攻擊
            var players = CreatureSystem.FindCreaturesByTag(Player);
            if (players != null)
                foreach (var player in players)
                    ((PlayerStrategy)player.FindComponentByTag(EnumComponentTag.CreatureStrategy))?
                        .ChangeState(Idle);

            _commonInputListener = new CommonInputSystem();
        }

        public override void Update()
        {
            CreatureSystem.Instance.Update();
            _commonInputListener.Update(); // 監聽按鍵事件
        }

        public override void Exit()
        {
        }
    }
}