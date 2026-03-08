using System;
using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField, Tooltip("the amount of degrees it can rotate in 1 second")]
    private float rotationSpeed;

    [Space(12)]
    [SerializeField] public const float MAX_ROTATION_DURATION = 2f;

    [Space(12)]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform muzzle;

    [Space(12)]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private GameObject explosionVFXPrefab;

    private Transform target;
    private Vector3 targetPos;
    private Quaternion lookRotation;
    private float damage;

    Action OnShoot;

    Coroutine currentCoroutine;

    public void InitialiseShoot(Transform target, float damage, Action onShoot)
    {

        if (currentCoroutine != null)
        {
            // Forcefully stop current coroutine and shoot the last awaiting shot

            StopCoroutine(currentCoroutine);

            transform.rotation = lookRotation;

            Shoot();
        }

        OnShoot = onShoot;

        this.target = target;
        this.damage = damage;
        targetPos = target.position;

        float angle = Vector3.SignedAngle(transform.forward, target.position.NoY() - transform.position.NoY(), Vector3.up);

        currentCoroutine = StartCoroutine(LookAt(Mathf.Abs(rotationSpeed / angle)));
    }

    private IEnumerator LookAt(float speed)
    {
        float time = 0f;

        Quaternion initialRotation = transform.rotation;

        while (time < 1)
        {
            if (target != null)
                targetPos = target.transform.position;

            lookRotation = Quaternion.LookRotation(targetPos.NoY() - transform.position.NoY());

            transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * speed;

            yield return null;
        }

        Shoot();

        currentCoroutine = null;
    }

    private void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.Init(target, damage);
        OnShoot();
        GameObject VFXGameObject = Instantiate(explosionVFXPrefab, shootPoint.position, Quaternion.LookRotation(muzzle.up));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * 10f);
    }
}
