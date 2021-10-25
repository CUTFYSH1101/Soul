using System;
using System.Collections;
using Main.EventLib.Sub.UIEvent.Combo;
using Main.Game;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Test.Scene.Scripts.Main.UI
{
    public class SceneLoadingUI
    {
        //載入介面
        private readonly GameObject _loadPanel;

        //滑動進度條
        private readonly Image _progressImg;

        //進度顯示百分比
        private readonly UIText _proText;
        private IEnumerator _coroutine;

        //不斷更新的進度值
        private float CurProgressValue { get; set; } = 0;

        public SceneLoadingUI(string hierarchyPath)
        {
            _loadPanel = UnityTool.GetComponentByPath<Transform>(hierarchyPath).gameObject;

            if (_loadPanel == null) throw new Exception("場景不含有場井載入的根物件，卻試圖想要獲取它！");

            _progressImg = _loadPanel.transform.GetComponentInChildren<Image>();
            _proText = new UIText(hierarchyPath);

            if (_progressImg == null) throw new Exception("場景不含有該物件，卻試圖想要獲取它！");
            if (_proText == null) throw new Exception("場景不含有該物件，卻試圖想要獲取它！");

            _loadPanel.SetActive(false); //一開始禁用載入層。
        }

        public void NewGameScene(AsyncOperation async)
        {
            Debug.Log(_proText.Text);
            _loadPanel.SetActive(true);
            _coroutine = LoadingUpdateEnumerator(async);
            MonoClass.Instance.StartCoroutine(_coroutine);
        }

        private IEnumerator LoadingUpdateEnumerator(AsyncOperation async)
        {
            while (true)
            {
                LoadingUpdate(async);
                if (async.isDone)
                    break;
                yield return new Update();
            }
        }

        public void SetDontDestroy()
        {
            Object.DontDestroyOnLoad(_loadPanel);
        }

        private void LoadingUpdate(AsyncOperation async)
        {
            if (async == null)
            {
                return; //程序為空，跳出該函式
            }

            //總的進度值
            // int progressValue = 100;

            CurProgressValue = async.progress;

            _proText.Text = $"Loading {CurProgressValue * 100f}%"; //實時更新進度百分比的文字顯示

            _progressImg.fillAmount = CurProgressValue; //實時更新滑動進度圖片的fillAmount值

            if (CurProgressValue >= 0.9f)
            {
                async.allowSceneActivation = true; //啟用自動載入場景

                _proText.Text = "OK"; //文字顯示完成OK

                _loadPanel.SetActive(false);

                // if (_coroutine != null) MonoClass.Instance.StopCoroutine(_coroutine);
            }
        }
    }
}
/*
1. 透過ui切換動畫(一致), 需要async, async的長度根據data的大小
2. 決定是否要讀取資料
3. 決定行為(new Game, Continue, Exit)
*/