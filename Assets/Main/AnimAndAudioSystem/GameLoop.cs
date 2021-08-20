using Main.AnimAndAudioSystem.Anims.Scripts;
using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.AnimAndAudioSystem.Main.Common;
using UnityEngine;
using Input = Main.AnimAndAudioSystem.Main.Input.Input;
using UnityInput = UnityEngine.Input;
using static Main.AnimAndAudioSystem.Main.Input.HotkeySet;

namespace Main.AnimAndAudioSystem
{
    public class GameLoop : MonoBehaviour
    {
        public Animator player;
        public bool dashOrMove;
        [Range(-1, 1)] public int jumping;
        public bool diveAttacking;
        public bool onAir;
        private CreatureAnimManager animManager;
        public DictionaryAudioPlayer audioPlayer;

        private void Awake()
        {
            if (player == null)
            {
                Debug.LogError("animManager為空");
                return;
            }

            animManager = new CreatureAnimManager(player);
        }

        private void Update()
        {
// ======
            if (UnityInput.anyKeyDown)
            {
                if (Input.GetButtonDown(Fire1))
                    audioPlayer.Play(Convert(Fire1));
                if (Input.GetButtonDown(Fire2))
                    audioPlayer.Play(Convert(Fire2));
                if (Input.GetButtonDown(Fire3))
                    audioPlayer.Play(Convert(Fire3));
            }

// ======
            // 確認是否可以移動(固定怪/負面狀態下不可移動)
            if (!dashOrMove)
                animManager.Move(Input.GetAxisRaw(Horizontal) != 0);
            else
                animManager.Dash(Input.GetAxisRaw(Horizontal) != 0); // 按兩下放開，會衝刺一段時間後停下，可被攻擊、俯衝等中斷

            if (Input.GetButtonDown(Jump))
            {
                // 保留空中移動速度
                // 平台掉落、按鍵觸發
                animManager.JumpUpdate(jumping = -jumping);
            }

            // 在空中(平台摔落/跳躍) && 按下攻擊鍵 && 可攻擊狀態
            // 允許空中衝刺後攻擊
            if (onAir)
            {
                if (Input.GetButtonDown(Control))
                    animManager.DiveAttack(EnumSymbol.Direct, diveAttacking = !diveAttacking);
                if (Input.GetButtonDown(Fire1))
                    animManager.JumpAttack(Convert(Fire1));
                if (Input.GetButtonDown(Fire2))
                    animManager.JumpAttack(Convert(Fire2));
                if (Input.GetButtonDown(Fire3))
                    animManager.JumpAttack(Convert(Fire3));
            }
            else
            {
                // 在地面上 && 按下方向鍵 && 攻擊鍵 && 可攻擊狀態
                // 允許打斷衝刺、移動
                if (Input.GetButton(Horizontal))
                {
                    if (Input.GetButtonDown(Fire1))
                        animManager.SpurAttack(Convert(Fire1));
                    if (Input.GetButtonDown(Fire2))
                        animManager.SpurAttack(Convert(Fire2));
                    if (Input.GetButtonDown(Fire3))
                        animManager.SpurAttack(Convert(Fire3));
                }
                // 在地面上 && 按下攻擊鍵 && 可攻擊狀態
                else
                {
                    if (Input.GetButtonDown(Fire1))
                        animManager.Attack(Convert(Fire1));
                    if (Input.GetButtonDown(Fire2))
                        animManager.Attack(Convert(Fire2));
                    if (Input.GetButtonDown(Fire3))
                        animManager.Attack(Convert(Fire3));
                }
            }
        }

        private EnumSymbol Convert(string key) =>
            key switch
            {
                "Fire1" => EnumSymbol.Square,
                "Fire2" => EnumSymbol.Cross,
                "Fire3" => EnumSymbol.Circle,
                _ => default
            };
    }
}