using Main.Blood;
using Main.Res.CharactersRes.Animations.Scripts;
using Main.Res.Script.Audio;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using static Main.Input.HotkeySet;

namespace Main.Res.Script
{
    public class GameLoop : MonoBehaviour
    {
        public Animator player;
        public bool dashOrMove;
        [Range(-1, 1)] public int jumping;
        public bool diveAttacking;
        public bool onAir;
        public DictAudioPlayer audioPlayer;
        public (CreatureAnimInterface anim, DictAudioPlayer audioPlayer) facade;

        private void Awake()
        {
            if (player == null)
            {
                Debug.LogError("System為空");
                return;
            }

            facade = (new CreatureAnimInterface(player), audioPlayer);
        }

        private void Update()
        {
// ======
            if (UnityInput.anyKeyDown)
            {
                if (Input.Input.GetButtonDown(Fire1))
                    facade.audioPlayer.Play(Convert(Fire1));
                if (Input.Input.GetButtonDown(Fire2))
                    facade.audioPlayer.Play(Convert(Fire2));
                if (Input.Input.GetButtonDown(Fire3))
                    facade.audioPlayer.Play(Convert(Fire3));
            }

// ======
            // 確認是否可以移動(固定怪/負面狀態下不可移動)
            if (!dashOrMove)
                facade.anim.Move(Input.Input.GetAxisRaw(Horizontal) != 0);
            else
                facade.anim.Dash(Input.Input.GetAxisRaw(Horizontal) != 0); // 按兩下放開，會衝刺一段時間後停下，可被攻擊、俯衝等中斷

            if (Input.Input.GetButtonDown(Jump))
            {
                // 保留空中移動速度
                // 平台掉落、按鍵觸發
                facade.anim.JumpUpdate(jumping = -jumping);
            }

            // 在空中(平台摔落/跳躍) && 按下攻擊鍵 && 可攻擊狀態
            // 允許空中衝刺後攻擊
            if (Input.Input.GetButtonDown(Fire1))
            {
                if (onAir)
                    facade.anim.AtkJump(Convert(Fire1));
                else if (Input.Input.GetButton(Horizontal))
                    facade.anim.AtkSpur(Convert(Fire1));
                else
                    facade.anim.AtkNormal(Convert(Fire1));
            }

            if (Input.Input.GetButtonDown(Fire2))
            {
                if (onAir)
                    facade.anim.AtkJump(Convert(Fire2));
                else if (Input.Input.GetButton(Horizontal))
                    facade.anim.AtkSpur(Convert(Fire2));
                else
                    facade.anim.AtkNormal(Convert(Fire2));
            }

            if (Input.Input.GetButtonDown(Fire3))
            {
                if (onAir)
                    facade.anim.AtkJump(Convert(Fire3));
                else if (Input.Input.GetButton(Horizontal))
                    facade.anim.AtkSpur(Convert(Fire3));
                else
                    facade.anim.AtkNormal(Convert(Fire3));
            }

            if (Input.Input.GetButtonDown(Control) && onAir)
                facade.anim.AtkDiveCrash(diveAttacking = !diveAttacking);
        }

        private BloodType Convert(string key) =>
            key switch
            {
                "Fire1" => BloodType.CSquare,
                "Fire2" => BloodType.CCrossx,
                "Fire3" => BloodType.CCircle,
                _ => default
            };
    }
}