using System;
using System.Collections;
using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer slash;
    [SerializeField] float dist;
    [SerializeField] float angle;
    [SerializeField] float slashTime;
    Vector3 scale;
    Vector3 reverseScale;
    public Action stopAttackingMethod;

    private void Awake()
    {
        slash.enabled = false;
        scale = transform.localScale;
        reverseScale = new Vector3(scale.x, -1 * scale.y, scale.z);
    }

    public void Animation(Vector3 origin, Transform target, Action<bool> hitMethod)
    {
        Vector3 targetDirection = target.transform.position - origin;
        SetPosition(origin, targetDirection, angle);
        StartCoroutine(Slash(origin, targetDirection, target, hitMethod));
    }

    public void Animation(Vector3 origin, Vector3 targetDirection, Transform target, Action<bool> hitMethod)
    {
        SetPosition(origin, targetDirection, angle);
        StartCoroutine(Slash(origin, targetDirection, target, hitMethod));
    }

    // TODO: reduce this to 1 slash and make it the resposibility of the caller to call twice for a double slash
    IEnumerator Slash(Vector3 origin, Vector3 direction, Transform target, Action<bool> hitMethod)
    {
        slash.enabled = true;
        yield return new WaitForSeconds(0.1f);

        bool attacking = true;
        float timer = slashTime;
        while(timer > 0)
        {
            float currentAngle = Mathf.Lerp(-angle, angle, timer / slashTime);
            if(attacking && currentAngle < 0)
            {
                hitMethod(HitTarget(target, origin, direction));
                attacking = false;
            }
            SetPosition(origin, direction, currentAngle);
            timer -= Time.deltaTime;
            yield return null;
        }

        slash.enabled = false;

        yield return new WaitForSeconds(0.2f);


        transform.localScale = reverseScale;

        slash.enabled = true;

        attacking = true;
        timer = slashTime;
        while (timer > 0)
        {
            float currentAngle = Mathf.Lerp(angle, -angle, timer / slashTime);
            if(attacking && currentAngle > 0)
            {
                hitMethod(HitTarget(target, origin, direction));
                attacking = false;
            }
            SetPosition(origin, direction, currentAngle);
            timer -= Time.deltaTime;
            yield return null;
        }

        slash.enabled = false;
        stopAttackingMethod();
    }

    void SetPosition(Vector3 origin, Vector3 targetDirection, float angle)
    {
        Vector3 direction = Utils.RotateDirection(targetDirection, angle);
        transform.rotation = Utils.Look(direction);
        transform.position = origin + direction.normalized * dist;
    }

    bool HitTarget(Transform target, Vector3 origin, Vector3 direction)
    {
        if (Vector3.Distance(target.position, origin) > dist * 1.5) return false;
        if (Utils.GetAngle(direction, target.position - origin) > angle) return false;
        return true;
    }
}
