using System;
using System.Collections;
using Main.Game;
using Main.Util;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Time = UnityEngine.Time;

namespace Sandbox.Scene.Scripts.Main.GUI
{
    public class LoadingFadeOutVFX : ILoadingVFX // todo 測試是否可用
    {
        private readonly GameObject _loadPanel; //載入介面
        private readonly Image _loadPanelImg;
        private float CurProgressValue { get; set; } = 0;

        public LoadingFadeOutVFX(string hierarchyPath)
        {
            _loadPanel = UnityTool.GetComponentByPath<Transform>(hierarchyPath).gameObject;
            if (_loadPanel == null) throw new Exception($"場景中找不到{hierarchyPath}");

            _loadPanelImg = _loadPanel.transform.GetComponentInChildren<Image>();
            _loadPanel.SetActive(false); //一開始禁用載入層。
        }

        public void NewGameScene(AsyncOperation async)
        {
            _loadPanel.SetActive(true);
            MonoClass.Instance.StartCoroutine(LoadingUpdateEnumerator(async));
        }

        public IEnumerator LoadingUpdateEnumerator(AsyncOperation async)
        {
            UnityTool.SetDontDestroy(MonoClass.Instance); // 並在結束後Destroy
            UnityTool.SetDontDestroy(_loadPanel.transform.root);
            _loadPanel.SetActive(true);
            while (true)
            {
                LoadingUpdate(async);
                if (async.isDone)
                    break;
                yield return new Update();
            }

            UnityTool.SetDontDestroy(_loadPanel.transform.root, false);
            UnityTool.SetDontDestroy(MonoClass.Instance, false);
            var timer = new CdTimer(0.99f, Stopwatch.Mode.RealWorld);
            timer.Reset();
            while (!timer.IsTimeUp)
            {
                LoadingUpdate(async);
                yield return new Update();
            }
        }

        public void LoadingUpdate(AsyncOperation async)
        {
            if (async == null) return; //程序為空，跳出該函式

            CurProgressValue = async.progress;

            if (!(CurProgressValue >= 0.9f)) return;
            async.allowSceneActivation = true; //啟用自動載入場景

            var color = _loadPanelImg.color;
            color.a -= Time.deltaTime * 5;
            if (color.a <= 0) color.a = 0;
            _loadPanelImg.color = color;
        }
    }
}