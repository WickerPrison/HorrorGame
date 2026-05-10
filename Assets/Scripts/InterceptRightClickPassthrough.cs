using UnityEngine;

public class InterceptRightClickPassthrough : MonoBehaviour, IInterceptRightClick
{
    [SerializeField] IInterceptRightClick passthrough;

    private void Start()
    {
        passthrough = transform.parent.GetComponentInParent<IInterceptRightClick>();
    }

    public bool RightClick()
    {
        return passthrough.RightClick();
    }
}
