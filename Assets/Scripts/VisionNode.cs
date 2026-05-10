using UnityEngine;

public class VisionNode : MonoBehaviour, IHaveVision
{
    [SerializeField] float setVisionRange;
    public float visionRange { get; set; }

    private void Start()
    {
        visionRange = setVisionRange;
        AddToVisionManager();
    }

    public void AddToVisionManager()
    {
        VisionManager.i.AddVision(this);
    }

    public void RemoveFromVisionManager()
    {
        VisionManager.i.RemoveVision(this);
    }

    public void ShowSprite(bool show)
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = show;
    }
}
