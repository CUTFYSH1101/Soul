namespace Blood {
    public struct BloodElement {
        private int _id;
        private string _name;
        public string GetElementName () => _name;
        public int GetElementId () => _id;
        public BloodElement SetElementName (int idIn = 0, string nameIn = "unknown") {
            this._id = idIn;
            this._name = nameIn;
            return this;
        }
    }
}
