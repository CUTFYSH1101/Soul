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
    public class Victory : SceneStateScript
    {
        private CommonInputSystem _commonInputListener; // 監聽部分按鍵事件

        public override void Enter()
        {
            // 目標：
            // 角色aware，允許攻擊，next level同下，並儲存載入角色資料，或用scriptable object
            // 載入怪物，next level時會載入其他怪物地圖
            // 按下esc關閉遊戲
            // 切換地圖後會切換關卡Scene，但遊戲規則不變
            // 如果UI全遊戲採用同一種固定的，那dont destroy: 1.database, 2.unity UGUI, 3. character
            // 當角色從右邊地圖換下一個地圖，則從左方進入，相反亦如此

            CreatureSystem.Instance.Init();
            CollisionManager.ThreeLevelGroundSetting(); // 三層地板

            // 把主角的狀態變更為aware，允許攻擊
            var players = CreatureSystem.FindCreaturesByTag(Player);
            if (players != null)
                foreach (var player in players)
                    ((PlayerStrategy)player.FindComponentByTag(EnumComponentTag.CreatureStrategy))?
                        .ChangeState(Aware);

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