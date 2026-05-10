using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUnit : MonoBehaviour, ITakeDamage, IHaveVision
{
    private Seeker seeker;
    private AIPath aiPath;
    bool selected = false;
    [SerializeField] SpriteRenderer outline;
    [SerializeField] float setVisionRange;
    public float visionRange { get; set; }
    SpriteMask visionMask;
    Terminal interactTerminal = null;
    bool goingToTerminal = false;
    UnitAbilities unitAbilities;
    Action destinationCallback;
    bool atDestination = false;
    public PlayerUnitData data;
    public TestPlayerUnitData testData;
    [SerializeField] ColorData colorData;
    [SerializeField] TextMeshProUGUI unitName;

    private void Awake()
    {
        LoadTestData();
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        unitAbilities = GetComponent<UnitAbilities>();
        PlayerEvents.i.UnitExists(this);
        PlayerEvents.i.UnitStatChange(this);
        AddToVisionManager();
        visionRange = setVisionRange;
        visionMask = GetComponentInChildren<SpriteMask>();
        visionMask.transform.localScale = visionRange * 2 * Vector3.one;
        Debug.Log(data);
        unitName.text = data.name;
    }

    private void Update()
    {
        if (!atDestination && aiPath.reachedDestination)
        {
            atDestination = true;
            if(destinationCallback != null)
            {
                destinationCallback();
                destinationCallback = null;
            }
        }


        if(interactTerminal != null && goingToTerminal)
        {
            if (Vector2.Distance(transform.position, interactTerminal.transform.position) <= 1f)
            {
                goingToTerminal = false;
                interactTerminal.StartPowering();
            }
        }
    }

    public void SetSelected(bool isSelected)
    {
        selected = isSelected;
        if (selected)
        {
            outline.color = colorData.player;
            unitName.color = colorData.player;
        }
        else
        {
            outline.color = colorData.powered;
            unitName.color = colorData.powered;
        }
    }

    public void PerformAbility(int abilityIndex)
    {
        if (data.abilities[abilityIndex] == Ability.NONE) return;
        unitAbilities.PerformAbility(data.abilities[abilityIndex]);
    }

    public void SetDestination(Vector3 destination, Action callback = null)
    {
        aiPath.isStopped = false;
        unitAbilities.InterruptAbilities(); //TODO: consider moving this somewhere else
        destinationCallback = callback;
        atDestination = false;
        seeker.StartPath(transform.position, destination);
        aiPath.destination = destination;
    }

    public void Stop()
    {
        seeker.CancelCurrentPathRequest();
        aiPath.SetPath(null);
        aiPath.isStopped = true;
    }

    public void TakeDamage(int amount)
    {
        data.health -= amount;
        PlayerEvents.i.UnitStatChange(this);
        if(data.health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        VisionManager.i.RemoveVision(this);
        PlayerEvents.i.UnitDeath(this);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GlobalEvents.i.onDeselectAll += PlayerEvents_onDeselectAll;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onDeselectAll -= PlayerEvents_onDeselectAll;
    }

    private void PlayerEvents_onDeselectAll(object sender, System.EventArgs e)
    {
        SetSelected(false);
    }

    void LoadTestData()
    {
        if (testData != null)
        {
            data = new PlayerUnitData(testData.unitName, testData.maxHealth);
            data.morality = testData.morality;
            data.abilities = testData.abilities;
        }
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
