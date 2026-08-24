using UnityEngine;
using TMPro;

public class RecruitsUnit : MonoBehaviour
{
    [SerializeField] UnitStatUi statsUi;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] MenuButton buy;
    [SerializeField] CampaignData campaignData;
    Recruits recruits;
    PlayerUnitData unit;

    private void Start()
    {
        recruits = GetComponentInParent<Recruits>();
    }

    public void UpdateUnitUi(PlayerUnitData unitData)
    {
        if(unitData == null)
        {
            cost.text = "";
            buy.gameObject.SetActive(false);
        }
        else
        {
            cost.text = $"Cost: {unitData.cost}";
            buy.gameObject.SetActive(true);
            buy.interactable = unitData.cost <= campaignData.aether;
        }

        unit = unitData;
        statsUi.SetUnit(unitData);
    }

    public void UpdateBuyInteractivity()
    {
        if (unit == null) return;
        buy.interactable = unit.cost <= campaignData.aether;
    }

    public void Buy()
    {
        if (campaignData.aether < unit.cost || unit == null) return;
        campaignData.aether -= unit.cost;
        campaignData.recruits.Remove(unit);
        campaignData.playerUnits.Add(unit);
        CampaignEvents.i.UpdateResources();
        CampaignEvents.i.UpdateSquad();
        recruits.UpdateRecruitsUi();
    }
}
