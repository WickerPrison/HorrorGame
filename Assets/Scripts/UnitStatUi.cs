using UnityEngine;
using TMPro;

public class UnitStatUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI morality;
    [SerializeField] TextMeshProUGUI health;

    private void Start()
    {
        SetUnit(null);
    }

    public void SetUnit(PlayerUnitData data)
    {
        if(data != null)
        {
            nameText.text = data.name;
            morality.text = $"Morality: {data.morality}";
            health.text = $"Health: {data.health}/{data.maxHealth}";
        }
        else
        {
            nameText.text = "";
            morality.text = "";
            health.text = "";
        }
    }
}
