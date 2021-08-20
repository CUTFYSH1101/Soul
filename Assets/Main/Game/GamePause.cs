using UnityEngine;

namespace Main.Game
{
    public class GamePause
    {
        public static bool IsGamePause => Time.timeScale == 0;
        public static void PauseGame ()
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        public static void ResumeGame ()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}