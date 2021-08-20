using Main.Game;
using UnityEngine;

public class MonoClass : MonoBehaviour
{
    public static MonoBehaviour Instance =>
        GameObject.Find("GameLoop").transform.GetOrAddComponent<MonoClass>();
}