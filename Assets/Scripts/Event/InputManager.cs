namespace Test2
{
    public class InputManager
    {
    }

    public class Input
    {
        private string key;

        public Input(string key)
        {
            this.key = key;
        }

        public bool GetButtonDown()
        {
            return UnityEngine.Input.GetButtonDown(key);
        }

        public bool GetButton()
        {
            return UnityEngine.Input.GetButton(key);
        }
    }
}