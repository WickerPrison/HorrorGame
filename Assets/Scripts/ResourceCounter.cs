using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        UpdateDisplay(0);
    }

    void UpdateDisplay(int value)
    {
        text.text = $"Resources: {value}";
    }

    private void Global_onUpdateResources(int amount)
    {
        UpdateDisplay(amount);
    }

    private void OnEnable()
    {
        GlobalEvents.i.onUpdateResources += Global_onUpdateResources;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onUpdateResources -= Global_onUpdateResources;
    }
}
