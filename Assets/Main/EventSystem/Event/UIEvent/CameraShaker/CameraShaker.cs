// https://ithelp.ithome.com.tw/articles/10251609

// *錯誤解決辦法：
// http://bradlypaul.blogspot.com/2016/05/shaderfind.html

using System;
using JetBrains.Annotations;
using Main.EventSystem.Event.Attribute;
using Main.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.EventSystem.Event.UIEvent.CameraShaker
{
    public class CameraShaker : UIEvent
    {
        // 加載資源
        private static CameraShaker _instance;
        public static CameraShaker Instance => _instance ??= new CameraShaker("Main Camera", 0.05f);
        private static Shader Blur => Shader.Find("UI/Blur"); // 注意會引起問題*

        private readonly Transform _camera;
        private Vector3 _cameraPos;
        private readonly float _quake;
        private readonly Blur _blur;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraName">鏡頭名稱</param>
        /// <param name="duration">持續時間</param>
        /// <param name="quake">震動係數</param>
        public CameraShaker(string cameraName, float duration = 0.05f, float quake = 0.2f) :
            base(new EventAttr(0, duration))
        {
            CauseEnter = new FuncCause(() => !IsEmpty());

            _camera = UnityTool.GetComponent<Transform>(cameraName);
            _blur = _camera.GetOrAddComponent<Blur>();
            var shader = Blur;
            if (shader != null)
                _blur.BlurShader = shader;
            _blur.Switch = false;
            _quake = quake;
        }

        private bool IsEmpty() =>
            _camera == null || _blur == null || _blur.BlurShader == null;

        public new void Invoke() => base.Invoke();

        protected override void Enter()
        {
            Debug.Log("VAR");
            _cameraPos = _camera.localPosition; // 如果相機跟隨某物件，此時會停止跟隨，直到Exit
            _blur.Switch = true; // 開關模糊效果
        }

        protected override void Update()
        {
            _camera.localPosition = _cameraPos + Random.insideUnitSphere * _quake;
        }

        protected override void Exit()
        {
            _camera.localPosition = _cameraPos;
            _blur.Switch = false;
        }
    }
}