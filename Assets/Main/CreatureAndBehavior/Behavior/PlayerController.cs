using JetBrains.Annotations;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.Input;
using static Main.Input.Input;

namespace Main.CreatureAndBehavior.Behavior
{
    public class PlayerController : IComponent
    {
        public int Id => GetHashCode();
        [NotNull] private readonly PlayerBehavior _behavior;
        [NotNull] private readonly CreatureInterface _interface;
        public PlayerController([NotNull] PlayerBehavior behavior, [NotNull] CreatureInterface @interface)
        {
            _behavior = behavior;
            _interface = @interface;
        }
        public void Update()
        {
            if (Game.GamePause.IsGamePause)
                return;

            if (_behavior.DBAxisClick.AndCause())
                _behavior.Dash(_behavior.DBAxisClick.AxisRaw());
            if (GetButtonDown(HotkeySet.Control) && !_interface.Grounded)
                _behavior.DiveAttack();
            if (GetButtonDown(HotkeySet.Fire1))
            {
                if (!_interface.Grounded)
                    _behavior.JumpAttack(EnumSymbol.Square);
                else if (GetButton(HotkeySet.Horizontal))
                    _behavior.SpurAttack(EnumSymbol.Square);
                else
                    _behavior.NormalAttack(EnumSymbol.Square);
            }

            if (GetButtonDown(HotkeySet.Fire2))
            {
                if (!_interface.Grounded)
                    _behavior.JumpAttack(EnumSymbol.Cross);
                else if (GetButton(HotkeySet.Horizontal))
                    _behavior.SpurAttack(EnumSymbol.Cross);
                else
                    _behavior.NormalAttack(EnumSymbol.Cross);
            }

            if (GetButtonDown(HotkeySet.Fire3))
            {
                if (!_interface.Grounded)
                    _behavior.JumpAttack(EnumSymbol.Circle);
                else if (GetButton(HotkeySet.Horizontal))
                    _behavior.SpurAttack(EnumSymbol.Circle);
                else
                    _behavior.NormalAttack(EnumSymbol.Circle);
            }

            if (GetButtonDown("Jump"))
                _behavior.JumpOrWallJump();
            _behavior.MoveUpdate();
        }
    }
}