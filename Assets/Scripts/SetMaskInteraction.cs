using UnityEngine;

public class SetMaskInteraction : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteMaskInteraction interaction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.maskInteraction = interaction;
    }
}
