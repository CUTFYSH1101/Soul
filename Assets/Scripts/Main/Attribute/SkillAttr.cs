using Main.Common;

namespace Main.Attribute
{
    public class SkillAttr
    {
        private Symbol symbol;
        private float duration;
        private float cdTime;

        public Symbol Symbol
        {
            get => symbol;
            set => symbol = value;
        }

        public float Duration
        {
            get => duration;
            set => duration = value;
        }

        public float CdTime
        {
            get => cdTime;
            set => cdTime = value;
        }

        public SkillAttr()
        {
        }

        public SkillAttr(Symbol symbol, float duration, float cdTime)
        {
            this.symbol = symbol;
            this.duration = duration;
            this.cdTime = cdTime;
        }
    }
}