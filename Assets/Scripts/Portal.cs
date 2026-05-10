using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] ColorData colorData;
    [SerializeField] SpriteRenderer pentagram;

    private void Start()
    {
        Utils.GetRoom(transform.position).portal = this;
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            pentagram.color = colorData.danger;
        }
        else
        {
            pentagram.color = colorData.powered;
        }
    }
}
