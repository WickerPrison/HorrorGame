using UnityEngine;

public class EnableChildrenOnObjectSeen : MonoBehaviour
{
    [SerializeField] HiddenTillSeen hiddenTillSeen;
    bool triggered = false;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (triggered) return;

        if (!hiddenTillSeen.hidden)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            triggered = true;
        }
    }
}
