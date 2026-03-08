using Crest;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event Action OnDestroyed;

    private Transform target;
    private Vector3 targetPos;
    private Vector3 startingPos;
    private Vector3 posXZ;

    private SampleHeightHelper sampleHeightHelper = new SampleHeightHelper();

    [SerializeField] private float minY = -10f;

    [SerializeField] private float speed;
    [SerializeField] private float underWaterThreshold = 1f;
    private float storedDamage;

    [SerializeField] private AnimationCurve arcYAnimCurve;

    [Header("VFX")]
    [SerializeField] private Transform explosionVFX;
    [SerializeField] private Transform waterSplashVFX;
    [SerializeField] private SmokeTrail smokeTrailVFX;

    private bool targetReached;

    private void FixedUpdate()
    {
        if (targetReached)
            return;

        if(target != null)
            targetPos = target.position;

        Vector3 moveDir = (targetPos - posXZ).normalized;


        float totalDist = Vector3.Distance(startingPos, targetPos);
        float distToStart = Vector3.Distance(posXZ, startingPos);
        float distNormalized;
        if (distToStart < totalDist)
        {
            posXZ += moveDir * speed * Time.fixedDeltaTime;
            float distToTarget = Vector3.Distance(posXZ, targetPos);
            distNormalized = 1 - distToTarget / totalDist;
        }
        else
        {
            posXZ -= moveDir * speed * Time.fixedDeltaTime;
            float distToTarget = Vector3.Distance(posXZ, targetPos);
            distNormalized = 1 + distToTarget / totalDist;
        }

        float maxHeight = totalDist / 4f;
        float heightDif = targetPos.y - startingPos.y;
        float posY = arcYAnimCurve.Evaluate(distNormalized) * maxHeight + heightDif * distNormalized;

        transform.position = new Vector3(posXZ.x, startingPos.y + posY, posXZ.z);

        sampleHeightHelper.Init(transform.position, 0f, true);

        if (sampleHeightHelper.Sample(out float oceanHeight) && oceanHeight - underWaterThreshold >= transform.position.y) // projectile hit the water // TODO : Water splash effect!
        {
            // Debug.Log(gameObject + " hit the water!");

            Instantiate(waterSplashVFX, transform.position + new Vector3(0f, underWaterThreshold, 0f), Quaternion.identity);

            targetReached = true;
            Destroy(gameObject);
        }

        if (transform.position.y < minY) // Object went out of boundaries
        {
            targetReached = true;
            Destroy(gameObject);
        }
    }

    public void Init(Transform target, float storedDamage)
    {
        this.target = target;
        this.storedDamage = storedDamage;

        targetPos = target.position;

        startingPos = transform.position;
        posXZ = transform.position.NoY();

        SmokeTrail smokeTrail = Instantiate(smokeTrailVFX, transform.position, Quaternion.identity);
        smokeTrail.Init(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        targetReached = true;

        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            healthSystem.TakeDamage(storedDamage);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
