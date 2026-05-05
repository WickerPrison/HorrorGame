using UnityEngine;

public class UnitsUi : MonoBehaviour
{
    [SerializeField] UnitStatUi[] unitUis;

    private void OnEnable()
    {
        GlobalEvents.i.onSelectUnits += Global_onSelectUnits;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onSelectUnits -= Global_onSelectUnits;
        
    }

    private void Global_onSelectUnits(System.Collections.Generic.List<PlayerUnit> selectedUnits)
    {
        for(int i = 0; i < unitUis.Length; i++)
        {
            if(selectedUnits.Count > i)
            {
                unitUis[i].SetUnit(selectedUnits[i].data);
            }
            else
            {
                unitUis[i].SetUnit(null);
            }
        }
    }
}
