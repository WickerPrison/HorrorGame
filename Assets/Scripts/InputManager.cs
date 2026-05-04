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
    public event System.Action onHack;

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
        inputSystem.ControlUnits.Hack.performed += Hack;
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

        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, Layers.clickableMask);
        if(hit != null && hit.gameObject.TryGetComponent<Door>(out Door door))
        {
            door.RightClick();
            return;
        }

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

    private void Hack(InputAction.CallbackContext obj)
    {
        onHack?.Invoke();
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
