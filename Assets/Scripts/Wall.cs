using UnityEngine;

public class Wall : MonoBehaviour
{
    SpriteRenderer sprite;
    ScanningState scanState;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SpriteVisible(bool isEnabled)
    {
        sprite.enabled = isEnabled;
    }

    public void SetScanState(ScanningState state)
    {
        scanState = state;
        switch (scanState)
        {
            case ScanningState.UNSCANNED:
                sprite.color = Color.white;
                break;
            case ScanningState.DANGER:
                sprite.color = Color.red;
                break;
            case ScanningState.SAFE:
                sprite.color = Color.green;
                break;
        }
    }
}
