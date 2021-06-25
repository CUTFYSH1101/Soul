using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
    public void ToDialogue () {
        SceneManager.LoadScene (2);
    }

    public void ToBlood () {
        SceneManager.LoadScene (1);
    }

    public void ToHome () {
        SceneManager.LoadScene (0);
    }
}
