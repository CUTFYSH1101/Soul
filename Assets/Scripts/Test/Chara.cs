namespace Test {
    public class Chara {
        private string name;

        public Chara SetName (string name) {
            this.name = name;
            return this;
        }
        public string GetName () => name;
    }
}
