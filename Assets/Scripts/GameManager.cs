using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        GlobalEvents.i.onPause += Global_onPause;
        GlobalEvents.i.onUnpause += Global_onUnpause;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onPause -= Global_onPause;
        GlobalEvents.i.onUnpause -= Global_onUnpause;
    }

    private void Start()
    {
        InputManager.i.SetInputState(InputState.CONTROL_UNITS);
    }

    private void Global_onUnpause()
    {
        Time.timeScale = 1;
    }

    private void Global_onPause()
    {
        Time.timeScale = 0;
    }
}
