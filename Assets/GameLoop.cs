using Test.Scene.Scripts;
using Test.Scene.Scripts.Game;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // 主角、怪物戰鬥系統、生物事件系統、
    // ECS、狀態模式
    public GameCenter center;

    private void Awake()
    {
        center = new GameCenter();
        center.ChangeSceneScriptState(EnumMapTag.MainMenu);
    }

    private void Update()
    {
        center.Update();
    }
}