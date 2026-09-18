using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents i;

    public event System.Action onPause;
    public event System.Action onUnpause;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    public void OnPause()
    {
        onPause?.Invoke();
    }

    public void OnUnpause()
    {
        onUnpause?.Invoke();
    }
}
