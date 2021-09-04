using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.AnimAndAudioSystem.Main.Common;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using static Main.Input.HotkeySet;

namespace Main.AnimAndAudioSystem
{
    public class GameLoop : MonoBehaviour
    {
        public Animator player;
        public bool dashOrMove;
        [Range(-1, 1)] public int jumping;
        public bool diveAttacking;
        public bool onAir;
        public DictionaryAudioPlayer audioPlayer;
        public AnimAndAudioSystem system;
        private void Awake()
        {
            if (player == null)
            {
                Debug.LogError("System為空");
                return;
            }

            system = new AnimAndAudioSystem(player,audioPlayer);
        }

        private void Update()
        {
// ======
            if (UnityInput.anyKeyDown)
            {
                if (Input.Input.GetButtonDown(Fire1))
                    system.Play(Convert(Fire1));
                if (Input.Input.GetButtonDown(Fire2))
                    system.Play(Convert(Fire2));
                if (Input.Input.GetButtonDown(Fire3))
                    system.Play(Convert(Fire3));
            }

// ======
            // 確認是否可以移動(固定怪/負面狀態下不可移動)
            if (!dashOrMove)
                system.Move(Input.Input.GetAxisRaw(Horizontal) != 0);
            else
                system.Dash(Input.Input.GetAxisRaw(Horizontal) != 0); // 按兩下放開，會衝刺一段時間後停下，可被攻擊、俯衝等中斷

            if (Input.Input.GetButtonDown(Jump))
            {
                // 保留空中移動速度
                // 平台掉落、按鍵觸發
                system.JumpUpdate(jumping = -jumping);
            }

            // 在空中(平台摔落/跳躍) && 按下攻擊鍵 && 可攻擊狀態
            // 允許空中衝刺後攻擊
            if (onAir)
            {
                if (Input.Input.GetButtonDown(Control))
                    system.DiveAttack(EnumSymbol.Direct, diveAttacking = !diveAttacking);
                if (Input.Input.GetButtonDown(Fire1))
                    system.JumpAttack(Convert(Fire1));
                if (Input.Input.GetButtonDown(Fire2))
                    system.JumpAttack(Convert(Fire2));
                if (Input.Input.GetButtonDown(Fire3))
                    system.JumpAttack(Convert(Fire3));
            }
            else
            {
                // 在地面上 && 按下方向鍵 && 攻擊鍵 && 可攻擊狀態
                // 允許打斷衝刺、移動
                if (Input.Input.GetButton(Horizontal))
                {
                    if (Input.Input.GetButtonDown(Fire1))
                        system.SpurAttack(Convert(Fire1));
                    if (Input.Input.GetButtonDown(Fire2))
                        system.SpurAttack(Convert(Fire2));
                    if (Input.Input.GetButtonDown(Fire3))
                        system.SpurAttack(Convert(Fire3));
                }
                // 在地面上 && 按下攻擊鍵 && 可攻擊狀態
                else
                {
                    if (Input.Input.GetButtonDown(Fire1))
                        system.Attack(Convert(Fire1));
                    if (Input.Input.GetButtonDown(Fire2))
                        system.Attack(Convert(Fire2));
                    if (Input.Input.GetButtonDown(Fire3))
                        system.Attack(Convert(Fire3));
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