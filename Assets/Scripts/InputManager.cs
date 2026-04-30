using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager i;
    public InputSystem_Actions inputSystem;

    public event System.Action<Vector3> onLeftClick;
    public event System.Action<Vector3> onRightClick;
    public event System.Action onScan;
    public event System.Action onCollect;

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
        inputSystem.ControlUnits.Scan.performed += Scan;
        inputSystem.ControlUnits.Collect.performed += Collect;
    }

    void LeftClick(InputAction.CallbackContext ctx)
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        onLeftClick?.Invoke(mouseWorldPos);
    }

    void RightClick(InputAction.CallbackContext ctx)
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        onRightClick?.Invoke(mouseWorldPos);
    }

    private void Scan(InputAction.CallbackContext obj)
    {
        onScan?.Invoke();
    }

    private void Collect(InputAction.CallbackContext obj)
    {
        onCollect?.Invoke();
    }

    public void SetControlUnits()
    {
        inputSystem.ControlUnits.Enable();
    }

    public void DisableControlUnits()
    {
        inputSystem.ControlUnits.Disable();
    }
}
