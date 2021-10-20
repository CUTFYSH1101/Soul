using UnityEngine;

namespace Main.EventLib.Sub.UIEvent.CameraShaker
{
    [RequireComponent(typeof(Camera))]
    public class Blur : MonoBehaviour
    {
        private Shader _blurShader;
        public Shader BlurShader
        {
            get => _blurShader;
            set
            {
                _blurShader = value;
                // 初始化blurShader
                if (!_blurShader.isSupported) return;
                _material = new Material(_blurShader);
                _material.hideFlags = HideFlags.DontSave;
            }
        }

        private Material _material;

        [Range(0, 4)]
        public int iterations = 2;
	
        [Range(0.2f, 5.0f)]
        public float blurSpread = 1.2f;

        private static readonly int BlurSize = Shader.PropertyToID("_BlurSize");

        /// 是否開啟blur效果
        public bool Switch { get; set; } = false;
        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (_material != null && Switch) {

                // 呼叫RenderTexture.GetTemporary是因為我們用了兩個pass
                RenderTexture buffer0 = RenderTexture.GetTemporary(src.width, src.height, 0);
                buffer0.filterMode = FilterMode.Bilinear;

                Graphics.Blit(src, buffer0);

                for (int i = 0; i < iterations; i++) {
                    _material.SetFloat(BlurSize, 1.0f + i * blurSpread);

                    // 存取第一個Pass的結果
                    RenderTexture buffer1 = RenderTexture.GetTemporary(src.width, src.height, 0);

                    // 縱向(Vertical)處理
                    Graphics.Blit(buffer0, buffer1, _material, 0);
                    // 記得釋放
                    RenderTexture.ReleaseTemporary(buffer0);

                    // 存取第二個Pass的結果
                    buffer0 = buffer1;
                    buffer1 = RenderTexture.GetTemporary(src.width, src.width, 0);

                    // 橫向(Horizontal)處理
                    Graphics.Blit(buffer0, buffer1, _material, 1);

                    RenderTexture.ReleaseTemporary(buffer0);
                    buffer0 = buffer1;
                }

                // 最後才處理完，返回結果
                Graphics.Blit(buffer0, dest);
                RenderTexture.ReleaseTemporary(buffer0);
            } else {
                Graphics.Blit(src, dest);
            }
        }
    }
}