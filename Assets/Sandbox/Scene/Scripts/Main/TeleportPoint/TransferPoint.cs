using System;
using System.ComponentModel;
using Main.Entity.Creature;
using Main.EventLib.Sub.BattleSystem;
using Main.Game.Coroutine;
using Test.Scene.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using Component = UnityEngine.Component;

namespace Sandbox.Scene.Scripts.Main.TeleportPoint
{
    public class TransferPoint : MonoBehaviour
    {
        [SerializeField, Tooltip("傳送到的地圖。")] private EnumMapTag destination;

        [SerializeField, Tooltip("傳送到的位置。世界座標")]
        private Vector2 targetPos;
        private float _first; // 避免載入場景第一秒玩家在傳送點內，而誤被傳送
        private const string TransferPos = "transfer_pos";
        private void Start()
        {
            _first = Time.unscaledTime;

            SetPlayerPos(GetVector2ByPlayerPrefs(TransferPos));
        }

        private static void SetPlayerPos(Vector2 pos)
        {
            if (pos == default) return;
            var players = CreatureSystem.FindCreaturesByTag(Factory.EnumCreatureTag.Player);
            if (players?[0] == null) return;
            players[0].Transform.position = pos;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.unscaledTime - _first < 0.1f)
                return;

            Transfer(other);
        }

        private void Transfer(Component other)
        {
            var spoiler = CreatureSystem.FindComponent<Spoiler>(other.transform.root);
            if (spoiler == null || spoiler.Team != Team.Player) return;

            SceneManager.LoadSceneAsync(destination.ReflectSceneName());
            SetVector2ByPlayerPrefs(TransferPos, targetPos);
        }

        private void OnApplicationQuit()
        {
            SetVector2ByPlayerPrefs(TransferPos, Vector2.zero);
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
    }
}