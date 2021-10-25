using Main.Blood;
using JetBrains.Annotations;
using Main.Entity;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.Input;
using Main.Res.Script;
using static Main.Input.Input;

namespace Main.CreatureBehavior.Behavior.Sub
{
    public class PlayerController : IPlayerController
    {
        public EnumComponentTag Tag => EnumComponentTag.PlayerController;
        [NotNull] private readonly PlayerBehavior _behavior;
        [NotNull] private readonly CreatureInterface _interface;
        public bool EnableAttack { get; set; } = true;

        public PlayerController([NotNull] PlayerBehavior behavior, [NotNull] CreatureInterface @interface)
        {
            _behavior = behavior;
            _interface = @interface;
        }

        public int DashForce { get; set; } = 30;
        public float DashForceDuration { get; set; } = 0.15f;
        public void Update()
        {
            if (Game.GamePause.IsGamePause)
                return;
            /*
            if (UnityInput.GetButton("Fire1"))
            {
                var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(UnityInput.mousePosition);
                // Debug.Log(mousePos.y);
                _behavior.MoveTo(mousePos);
                return;
            }
            */
            /*
            if (_behavior.DBAxisClick.AndCause())
                _behavior.Dash2(_behavior.DBAxisClick.AxisRaw() * DashForce, DashForceDuration);
            */
            if (_behavior.DBAxisClick.AndCause())
                _behavior.Dash(_behavior.DBAxisClick.AxisRaw());

            if (EnableAttack)
            {
                if (GetButtonDown(HotkeySet.Control) && !_interface.Grounded)
                    _behavior.DiveAttack();
                if (GetButtonDown(HotkeySet.Fire1))
                {
                    if (!_interface.Grounded)
                        _behavior.JumpAttack(BloodType.CSquare);
                    else if (GetButton(HotkeySet.Horizontal))
                        _behavior.SpurAttack(BloodType.CSquare);
                    else
                        _behavior.NormalAttack(BloodType.CSquare);
                }

                if (GetButtonDown(HotkeySet.Fire2))
                {
                    if (!_interface.Grounded)
                        _behavior.JumpAttack(BloodType.CCrossx);
                    else if (GetButton(HotkeySet.Horizontal))
                        _behavior.SpurAttack(BloodType.CCrossx);
                    else
                        _behavior.NormalAttack(BloodType.CCrossx);
                }

                if (GetButtonDown(HotkeySet.Fire3))
                {
                    if (!_interface.Grounded)
                        _behavior.JumpAttack(BloodType.CCircle);
                    else if (GetButton(HotkeySet.Horizontal))
                        _behavior.SpurAttack(BloodType.CCircle);
                    else
                        _behavior.NormalAttack(BloodType.CCircle);
                }
            }

            if (GetButtonDown("Jump"))
            {
                _behavior.JumpOrWallJump();
            }
            _behavior.MoveUpdate();
        }
    }
}


/*
1.
如何方便的debug？
快速修改一些參數？
要求：
參數和調整後的數值要顯示在畫面上
可以即時察看更改前後
例如跳躍高度是由重量、跳躍力、阻力、摩擦力影響，尤其跳躍力、重量最為重要
因此只要畫面上能夠修改這兩個數值，之後按Space鍵，會即時更改跳躍高度

2.
如何開啟debug模式\後門\秘笈
*/