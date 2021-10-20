using System;
using Main.Entity;
using Main.Res.Resources;
using Main.Util;
using Test.Scene.Scripts.Game;
using Test.Scene.Scripts.Main.SceneStateScript;
using UnityEngine;

// 主程式運作區塊
// 遊戲引擎下的場景變更工具

namespace Test.Scene.Scripts
{
    public class GameCenter : IComponent
    {
        private SceneStateScript _sceneStateScript; // 主程式運作區塊、控制按鈕、keyboard事件切換
        public SceneHandler SceneHandler { get; } // 在遊戲引擎中變更場景
        public EntityData MainCharacterData { get; set; } // 保存資料。關閉時、每五秒，會自動保存

        public GameCenter() =>
            SceneHandler = new SceneHandler(); // 必須在場景中先放loading時會顯示的UI!

        /*前一個場景中，執行SceneState.Exit
        前一個場景中，儲存entityData(也可以用ScriptableObj避免資料遺失)
        更改unity場景
        dont destroy awake
        執行SceneState.Enter
        載入entityData
        持續執行SceneState.Update*/

        /*切換到第n個場景，n>=2
        MainCharacterData.Save();
        EngineSceneHandler.ChangeScene(newMap);
        MainCharacterData.Load(); // dont destroy this
        ChangeSceneScriptState(newMap);*/
        /// 在程式中切換主程式的運作，包括按鍵觸發、按鍵觸發事件
        public void ChangeSceneScriptState(EnumMapTag newMap)
        {
            _sceneStateScript?.Exit(); // 前一個場景的主程式結束
            _sceneStateScript = SceneStateScript.NewInstance(newMap, this);
            _sceneStateScript.Enter();
            Debug.Log($"切換為新的主程式狀態：{_sceneStateScript.GetType().Name}");
        }

        public EnumComponentTag Tag => EnumComponentTag.SceneManagement;

        public void Update() => _sceneStateScript?.Update();
    }

    //mainCharacter
    [Serializable]
    public struct EntityData
    {
        public Vector2 last_position;

        public float attack_speed;

        public string max_hp, current_hp;

        public EntityData(Vector2 lastPosition, string maxHp, string currentHp, float attackSpeed = 1f)
        {
            last_position = lastPosition;
            attack_speed = attackSpeed;
            max_hp = maxHp;
            current_hp = currentHp;
        }

        private static void SetVector2ByPlayerPrefs(string key, Vector2 value)
        {
            PlayerPrefs.SetFloat($"{key}_x", value.x); // position_x
            PlayerPrefs.SetFloat($"{key}_y", value.y); // position_y
        }

        private static Vector2 GetVector2ByPlayerPrefs(string key)
        {
            Vector2 temp;
            temp.x = PlayerPrefs.GetFloat($"{key}_x");
            temp.y = PlayerPrefs.GetFloat($"{key}_y");
            return temp;
        }

        public void Save()
        {
            SetVector2ByPlayerPrefs("last_position", last_position);
            PlayerPrefs.SetString("current_hp", current_hp);
            PlayerPrefs.SetString("max_hp", max_hp);
            PlayerPrefs.SetFloat("attack_speed", attack_speed);

            "資料儲存完畢".LogLine();
        }

        public void Load()
        {
            last_position = GetVector2ByPlayerPrefs("last_position");
            current_hp = PlayerPrefs.GetString("current_hp");
            max_hp = PlayerPrefs.GetString("max_hp");
            attack_speed = PlayerPrefs.GetFloat("attack_speed");

            "玩家資料讀取完畢".LogLine();
        }
        // 可以用scriptable object儲存
        // 角色配點信息
        // 角色位置
    }

    public class PData
    {
        // 任務總類與數量
        // 每個任務完成進度
        // PlayerPrefab.SaveXXX
        // 載入的bgm
    }
}