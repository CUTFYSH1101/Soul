namespace Test2
{
    public class Input
    {
        private string key;

        public Input(string key)
        {
            this.key = key;
        }

        public bool GetButtonDown()
        {
            return Main.Common.Input.GetButtonDown(key);
        }

        public bool GetButton()
        {
            return Main.Common.Input.GetButton(key);
        }
    }
}