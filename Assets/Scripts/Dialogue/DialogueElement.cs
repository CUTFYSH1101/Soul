using JetBrains.Annotations;

namespace Dialogue {
    public struct DialogueElement {
        [UsedImplicitly] private string _speaker;
        [UsedImplicitly] private string _lore;
        [UsedImplicitly] private bool _isOption;
        public string GetSpeaker () => _speaker;
        public string GetLore () => _lore;
        public bool GetOption () => _isOption;
        public DialogueElement (string speakerIn, string loreIn, bool option = false) {
            _speaker = speakerIn;
            _lore = loreIn;
            _isOption = option;
        }
        public DialogueElement SetSpeaker (string speakerIn) {
            _speaker = speakerIn;
            return this;
        }
        public DialogueElement SetLore (string loreIn) {
            _lore = loreIn;
            return this;
        }
        public DialogueElement SetOption (bool optionIn) {
            _isOption = optionIn;
            return this;
        }
    }
}
