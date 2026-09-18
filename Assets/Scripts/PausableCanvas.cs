using UnityEngine;

public class PausableCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        PauseManager.i.sceneCanvases.Add(canvasGroup);
    }
}
