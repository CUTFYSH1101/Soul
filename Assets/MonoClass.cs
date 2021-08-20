using UnityEngine;

public class MonoClass : MonoBehaviour
{
    public static MonoBehaviour instance;

    public static MonoBehaviour Instance =>
        instance ??= GameObject.Find("GameLoop").transform.gameObject.AddComponent<MonoClass>();
}