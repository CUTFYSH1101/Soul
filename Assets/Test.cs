using Extension.Common;
using Main.Entity.Controller;
using Main.Util;
using UnityEngine;
using i = UnityEngine.Input;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (i.GetButtonDown("Horizontal"))
        {
            Debug.Log("Horizontal");
        }

        if (i.GetButtonDown("Mouse X"))
        {
            Debug.Log("Mouse X");
        }

        if (i.GetButtonDown("Fire1"))
        {
            var pp =
                UnityTool.GetComponents<Transform>(
                    null, Strings.CharacterName.Warrior.ToString())[0];// OK
            Debug.Log($"我找到{pp.name}");
            var m = pp.GetComponent<Message>();
            m.GetCreatureAttr().ToString().LogLine();
        }
    }
}