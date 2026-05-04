using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager i;
    public InputSystem_Actions inputSystem;

    public event System.Action<Vector3> onLeftClick;
    public event System.Action<Vector3> onRightClick;
    public event System.Action<int> onAbility;

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


    public void SetControlUnits()
    {
        inputSystem.ControlUnits.Enable();
    }

    public void DisableControlUnits()
    {
        inputSystem.ControlUnits.Disable();
    }
}
