using Main.Entity.Creature;

namespace Main.EventLib.Sub.BattleSystem
{
    // 擔當creature, creatureAI, profile的角色，供給AI判斷資料
    public interface IMediator
    {
        Creature Creature { get; }
        Team Team { get; }
        bool IsKilled { get; }

        bool IsEnemy(Team target);
    }
}