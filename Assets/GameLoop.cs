using Main.EventSystem.Cause;
using Main.EventSystem.Event;
using Main.EventSystem.Event.Demo;
using Main.EventSystem.Event.Demo.EventA;
using Main.EventSystem.Util;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private UnityCoroutine _coroutine1, _coroutine;
    private CdEvent _cdEvent = new CdEvent(2);
    private CdCause _cdCause = new CdCause(1);
    private ConcreteEventA _eventTest;
    private void Awake()
    {
        /*
            coroutine1 = new UnityCoroutine();
            coroutine = new UnityCoroutine();
            coroutine1.CreateActionA(
                () => Debug.Log("第一個開始"), () => true,
                () => Debug.Log("第一個結束"), () => false,
                () => Debug.Log("第一個更新")
            );
            coroutine.CreateActionA(
                () => Debug.Log("第二個開始"), () => true,
                () => Debug.Log("第二個結束"), () => false,
                () => Debug.Log("第二個更新")
            );
        */
        _eventTest = new ConcreteEventA();
    }

    private void Update()
    {
        // Debug.Log(cdCause.AndCause() + " " + cdCause.OrCause());
        if (Input.anyKeyDown)
        {
            // coroutine1.InterruptCoroutine();
            // cdEvent.Invoke();
            // cdCause.Reset();
            _eventTest.Invoke();
        }
    }
}