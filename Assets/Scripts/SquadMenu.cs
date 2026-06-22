using UnityEngine;

public class SquadMenu : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] SquadUnit[] squadUnits;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSquadUi();
    }

    void UpdateSquadUi()
    {
        for (int i = 0; i < campaignData.squad.Length; i++)
        {
            if (campaignData.squad[i] != null)
            {
                campaignData.squad[i].index = i;
            }
            squadUnits[i].SetUnitData(campaignData.squad[i]);
        }
    }

    public void AssignToSquad(PlayerUnitData unitData)
    {
        for(int i = 0; i < campaignData.squad.Length; i++)
        {
            if(campaignData.squad[i] == null)
            {
                campaignData.squad[i] = unitData;
                UpdateSquadUi();
                CampaignEvents.i.UpdateSquad();
                return;
            }
        }
    }

    public void SelectSquadUnit(int index)
    {
        CampaignEvents.i.SelectUnit(campaignData.squad[index]);
    }

    public void RemoveFromSquad(int index)
    {
        campaignData.squad[index].index = 0;
        campaignData.squad[index] = null;
        UpdateSquadUi();
        CampaignEvents.i.UpdateSquad();
    }

    public void MoveUnitUp(int index)
    {
        if (index == 0) return;
        (campaignData.squad[index], campaignData.squad[index - 1]) = (campaignData.squad[index - 1], campaignData.squad[index]);
        UpdateSquadUi();
        CampaignEvents.i.UpdateSquad();
    }

    public void MoveUnitDown(int index)
    {
        if (index == 3) return;
        (campaignData.squad[index], campaignData.squad[index + 1]) = (campaignData.squad[index + 1], campaignData.squad[index]);
        UpdateSquadUi();
        CampaignEvents.i.UpdateSquad();
    }

    private void Campaign_onUpdateSquad()
    {
        UpdateSquadUi();
    }

    private void OnEnable()
    {
        CampaignEvents.i.onUpdateSquad += Campaign_onUpdateSquad;
    }

    private void OnDisable()
    {
        CampaignEvents.i.onUpdateSquad -= Campaign_onUpdateSquad;  
    }
}
