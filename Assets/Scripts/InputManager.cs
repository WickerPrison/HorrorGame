using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState
{
    NONE, CONTROL_UNITS, MENU, PAUSED
}

public class InputManager : MonoBehaviour
{
    public static InputManager i;
    public InputSystem_Actions inputSystem;

    public InputState inputState { get; private set; } = InputState.NONE;

    public event System.Action<Vector3> onLeftClick;
    public event System.Action<Vector3> onRightClick;
    public event System.Action<int> onAbility;
    public event System.Action onPortal;
    public event System.Action onLeaveMission;
    public event System.Action<int> onSelectButton;
    public event System.Action onPause;
    public event System.Action onUnpause;

    void Awake()
    {
        if(i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;

        inputSystem = new InputSystem_Actions();
        inputSystem.ControlUnits.LeftClick.performed += LeftClick;
        inputSystem.ControlUnits.RightClick.performed += RightClick;
        inputSystem.ControlUnits.Ability1.performed += Ability1;
        inputSystem.ControlUnits.Ability2.performed += Ability2;
        inputSystem.ControlUnits.Ability3.performed += Ability3;
        inputSystem.ControlUnits.Ability4.performed += Ability4;
        inputSystem.ControlUnits.Portal.performed += Portal;
        inputSystem.ControlUnits.LeaveMission.performed += LeaveMission;
        inputSystem.ControlUnits.Select1.performed += Select1;
        inputSystem.ControlUnits.Select2.performed += Select2;
        inputSystem.ControlUnits.Select3.performed += Select3;
        inputSystem.ControlUnits.Select4.performed += Select4;
        inputSystem.ControlUnits.Pause.performed += Pause;
        inputSystem.Menu.Pause.performed += Pause;
        inputSystem.Paused.Unpause.performed += Unpause;
    }

    private void LeaveMission(InputAction.CallbackContext ctx)
    {
        onLeaveMission?.Invoke();
    }

    void LeftClick(InputAction.CallbackContext ctx)
    {
        LeftClick();
    }

    public void LeftClick()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        LeftClick(mouseWorldPos);
    }

    public void LeftClick(Vector2 clickPos)
    {
        onLeftClick?.Invoke(clickPos);
    }

    void RightClick(InputAction.CallbackContext ctx)
    {
        RightClick();
    }

    public void RightClick()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        RightClick(mouseWorldPos);
    }

    public void RightClick(Vector2 clickPos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(clickPos, Layers.clickableMask);
        foreach(Collider2D hit in hits)
        {
            if (hit != null && hit.gameObject.TryGetComponent(out IInterceptRightClick rightClickInterceptor))
            {
                bool shouldContinue = rightClickInterceptor.RightClick();
                if (!shouldContinue) return;
            }
        }

        onRightClick?.Invoke(clickPos);
    }

    void Ability1(InputAction.CallbackContext ctx)
    {
        onAbility?.Invoke(0);
    }

    void Ability2(InputAction.CallbackContext ctx)
    {
        onAbility?.Invoke(1);
    }

    void Ability3(InputAction.CallbackContext ctx)
    {
        onAbility?.Invoke(2);
    }

    void Ability4(InputAction.CallbackContext ctx)
    {
        onAbility?.Invoke(3);
    }

    public void Portal(InputAction.CallbackContext ctx)
    {
        onPortal?.Invoke();
    }

    void Select1(InputAction.CallbackContext ctx)
    {
        onSelectButton?.Invoke(0);
    }

    void Select2(InputAction.CallbackContext ctx)
    {
        onSelectButton?.Invoke(1);
    }

    void Select3(InputAction.CallbackContext ctx)
    {
        onSelectButton?.Invoke(2);
    }

    void Select4(InputAction.CallbackContext ctx)
    {
        onSelectButton?.Invoke(3);
    }

    void Pause(InputAction.CallbackContext ctx)
    {
        onPause?.Invoke();
    }

    void Unpause(InputAction.CallbackContext ctx)
    {
        onUnpause?.Invoke();
    }

    public void SetInputState(InputState state)
    {
        Debug.Log($"input state: {state}");
        inputState = state;
        switch (state)
        {
            case InputState.CONTROL_UNITS:
                SetControlUnits();
                break;
            case InputState.MENU:
                SetMenu();
                break;
            case InputState.PAUSED:
                SetPaused();
                break;
            case InputState.NONE:
                DisableAllStates();
                break;
        }
    }

    void SetControlUnits()
    {
        inputSystem.ControlUnits.Enable();
        inputSystem.Paused.Disable();
        inputSystem.Menu.Disable();
    } 
    
    void SetMenu()
    {
        inputSystem.ControlUnits.Disable();
        inputSystem.Paused.Disable();
        inputSystem.Menu.Enable();
    }

    void SetPaused()
    {
        inputSystem.ControlUnits.Disable();
        inputSystem.Paused.Enable();
        inputSystem.Menu.Disable();
    }

    void DisableAllStates()
    {
        inputSystem.ControlUnits.Disable();
        inputSystem.Paused.Disable();
        inputSystem.Menu.Disable();
    }
}
