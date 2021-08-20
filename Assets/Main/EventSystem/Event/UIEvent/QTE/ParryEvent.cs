using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Cause;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.StateEvent;
using Main.EventSystem.Util;

namespace Main.EventSystem.Event.UIEvent.QTE
{
    public class ParryEvent
    {
        public void Invoke(AbstractCreature subject)
        {
            var duration = new CdCause(0.2f);
            duration.Reset();
            new UnityCoroutine().CreateActionC(
                () =>
                {
                    subject.SetMindState(EnumMindState.Parry);
                    UserInterface.CreateCreatureBehaviorInterface(subject)
                        .NormalAttack(EnumSymbol.Square); // 播放攻擊動畫
                },
                onExit: () => subject.SetMindState(EnumMindState.Idle), causeExit: () => duration.OrCause());
        }
    }
}