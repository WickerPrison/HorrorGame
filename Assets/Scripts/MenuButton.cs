using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButton : Button
{
    TMP_Text text;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TMP_Text>();
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (text == null) return;

        ColorBlock cb = colors;

        Color targetColor = state switch
        {
            SelectionState.Normal => cb.normalColor,
            SelectionState.Highlighted => cb.highlightedColor,
            SelectionState.Pressed => cb.pressedColor,
            SelectionState.Selected => cb.selectedColor,
            SelectionState.Disabled => cb.disabledColor,
            _ => cb.normalColor
        };

        // Optional: respect the fade duration for smooth transitions
        if (instant)
        {
            text.color = targetColor;
        }
        else
        {
            text.CrossFadeColor(targetColor, cb.fadeDuration, true, true);
        }
    }
}
