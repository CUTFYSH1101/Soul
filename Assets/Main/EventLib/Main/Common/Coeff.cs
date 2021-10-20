namespace Main.EventLib.Common
{
    public class BoolItem: CoeffItem<bool>
    {
        public BoolItem(bool coeff) : base(coeff)
        {
        }

        public override bool Value
        {
            get => value;
            set
            {
                if (coeff)
                    this.value = value;
            }
        }
    }
}