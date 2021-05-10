namespace Inventory {
    public abstract class AbstractItem {
        private string Name = "";
        private string Desc = "";
        private int Stack = 0;
        public abstract void ItemEtTrigger ();
    }
    
    public class KeyItem : AbstractItem{
        public override void ItemEtTrigger () {
        }
    }

    public class CommonItem : AbstractItem {
        public override void ItemEtTrigger () {
            //event list
        }
    }
}
