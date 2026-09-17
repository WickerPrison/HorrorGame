using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager i;

    public List<CanvasGroup> sceneCanvases = new List<CanvasGroup>();
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] CanvasGroup pauseCanvasGroup;
    InputState previousState;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    private void Start()
    {
        SetPause(false);
    }

    private void OnEnable()
    {
        InputManager.i.onPause += OnPause;
        InputManager.i.onUnpause += OnUnpause;
    }

    private void OnDisable()
    {
        InputManager.i.onPause -= OnPause;
        InputManager.i.onUnpause -= OnUnpause;
    }

    private void OnPause()
    {
        SetPause(true);
        previousState = InputManager.i.inputState;
        InputManager.i.SetInputState(InputState.PAUSED);
        GlobalEvents.i.OnPause();
    }

    public void OnUnpause()
    {
        SetPause(false);
        InputManager.i.SetInputState(previousState);
        previousState = InputState.NONE;
        GlobalEvents.i.OnUnpause();
    }


    void SetPause(bool paused)
    {
        pauseCanvasGroup.interactable = paused;
        pauseCanvas.enabled = paused;
        foreach(CanvasGroup canvasGroup in sceneCanvases)
        {
            canvasGroup.interactable = !paused;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("PlaceholderMainMenu");
    }
}
