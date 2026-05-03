using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ColorData colorData;
    ScanningState scanState;

    public void SpriteVisible(bool isEnabled)
    {
        sprite.enabled = isEnabled;
    }

    public void SetScanState(ScanningState state, bool powered)
    {
        scanState = state;
        switch (scanState, powered)
        {
            case (ScanningState.UNSCANNED, true):
                sprite.color = colorData.powered;
                break;
            case (ScanningState.UNSCANNED, false):
                sprite.color = colorData.unpowered;
                break;
            case (ScanningState.DANGER, _):
                sprite.color = colorData.danger;
                break;
            case (ScanningState.SAFE, _):
                sprite.color = colorData.safe;
                break;
        }
    }
}
