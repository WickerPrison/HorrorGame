using System;
using UnityEngine;

public class CampaignEvents : MonoBehaviour
{
    public static CampaignEvents i;

    public event Action onUpdateSquad;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    public void UpdateSquad()
    {
        onUpdateSquad?.Invoke();
    }
}
