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

    private void Awake()
    {
        slash.enabled = false;
        scale = transform.localScale;
        reverseScale = new Vector3(scale.x, -1 * scale.y, scale.z);
    }

    public void Animation(Vector3 origin, Vector3 targetDirection)
    {
        SetPosition(origin, targetDirection, angle);
        StartCoroutine(Slash(origin, targetDirection));
    }

    IEnumerator Slash(Vector3 origin, Vector3 direction)
    {
        slash.enabled = true;
        yield return new WaitForSeconds(0.1f);

        float timer = slashTime;
        while(timer > 0)
        {
            float currentAngle = Mathf.Lerp(-angle, angle, timer / slashTime);
            SetPosition(origin, direction, currentAngle);
            timer -= Time.deltaTime;
            yield return null;
        }

        slash.enabled = false;

        yield return new WaitForSeconds(0.2f);


        transform.localScale = reverseScale;

        slash.enabled = true;

        timer = slashTime;
        while (timer > 0)
        {
            float currentAngle = Mathf.Lerp(angle, -angle, timer / slashTime);
            SetPosition(origin, direction, currentAngle);
            timer -= Time.deltaTime;
            yield return null;
        }

        slash.enabled = false;
    }

    void SetPosition(Vector3 origin, Vector3 targetDirection, float angle)
    {
        Vector3 direction = Utils.RotateDirection(targetDirection, angle);
        transform.rotation = Utils.Look(direction);
        transform.position = origin + direction.normalized * dist;
    }
}
