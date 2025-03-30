using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    private LayerMask    layerMask;
    [SerializeField]
    private int          damage;

    public Light2D Light2D { get; private set; }

    private void Awake()
    {
        Light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            DetectObjectsInSector();
        }
    }

    private void DetectObjectsInSector()
    {
        if (Light2D == null) return;

        float   lightRadius    = Light2D.pointLightOuterRadius;
        float   lightAngle     = Light2D.pointLightOuterAngle;
        Vector2 lightDirection = transform.up;

        var detectedObjects    = Physics2D.OverlapCircleAll(transform.position, lightRadius, layerMask);

        foreach (var obj in detectedObjects)
        {
            Vector2 directionToTarget = (obj.transform.position - transform.position).normalized;
            float   distanceToTarget  = Vector2.Distance(transform.position, obj.transform.position);
            float   angleToTarget     = Vector2.Angle(lightDirection, directionToTarget);

            if (distanceToTarget <= lightRadius && angleToTarget <= lightAngle / 2)
            {
                obj.GetComponent<MonsterBase>().TakeDotDamage(damage);
            }
        }
    }
}
