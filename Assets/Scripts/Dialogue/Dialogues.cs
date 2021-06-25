using DP = Dialogue.DialogueProcessing;
using DR = Dialogue.DialogueRecorder;
using DE = Dialogue.DialogueElement;
using DC = Dialogue.DialogueCenter;

namespace Dialogue {
    public static class Dialogues {
        public static readonly DP ProcessingA = new DP ();
        public static readonly DP ProcessingB1 = new DP ();
        public static readonly DP ProcessingB2 = new DP ();
        public static readonly DP ProcessingC = new DP ();

        public static void SetProcessing () {
            ProcessingA.AddElementToQueue (
                new DialogueElement ("Skull", "Hello!"),
                new DialogueElement ("Tree", "Good Morning."),
                new DialogueElement ("Skull", "How are you?", true),
                new DialogueElement ("Skull", "How’s it going?", true)
            );
            ProcessingB1.AddElementToQueue (
                new DialogueElement ("Tree", "I'm fine, thank you, and you?"),
                new DialogueElement ("Skull", "Not bad!")
            );
            ProcessingB2.AddElementToQueue (
                new DialogueElement ("Tree", "Good, thanks. How about you?"),
                new DialogueElement("Skull", "I’m doing great.")
            );
            ProcessingC.AddElementToQueue (
                new DialogueElement("Tree", "OK, bye bye!")
            );
        }
    }
}