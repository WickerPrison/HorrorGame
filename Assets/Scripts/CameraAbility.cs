using System.Linq;
using UnityEngine;

public class CameraAbility : MonoBehaviour, IHaveVision, ITakeDamage, IInterceptRightClick
{
    [SerializeField] float setVisionRange;
    public float visionRange { get; set; }
    SpriteMask visionMask;
    Room room;
    PlayerManager playerManager;

    private void Start()
    {
        visionRange = setVisionRange;
        AddToVisionManager();
        visionMask = GetComponentInChildren<SpriteMask>();
        visionMask.transform.localScale = visionRange * 2 * Vector3.one;
        room = Utils.GetRoom(transform.position);
        room.AddDamageTaker(this);
        playerManager = PlayerManager.i;
    }

    public void TakeDamage(int _)
    {
        Cleanup();
        Destroy(gameObject);
    }

    public bool RightClick()
    {
        if 
        (
            playerManager.selectedUnits.Count != 1 || 
            playerManager.selectedUnits[0].data.UsesOfAbilityType(AbilityType.CAMERA) > 0
        )
        {
            return true;
        }

        playerManager.selectedUnits[0].SetDestination(transform.position, () => GetCollected(playerManager.selectedUnits[0]));
        return false;
    }

    void GetCollected(PlayerUnit unit)
    {
        Cleanup();
        unit.data.GainUsesOfAbilityType(AbilityType.CAMERA, 1);
        PlayerEvents.i.UnitStatChange(unit);
        Destroy(gameObject);
    }

    void Cleanup()
    {
        room.RemoveDamageTaker(this);
        RemoveFromVisionManager();
    }

    public void AddToVisionManager()
    {
        VisionManager.i.AddVision(this);
    }

    public void RemoveFromVisionManager()
    {
        VisionManager.i.RemoveVision(this);
    }
}
