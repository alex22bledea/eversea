using Crest;
using UnityEngine;
using UnityEngine.Animations;

public class SmokeTrail : MonoBehaviour
{
    [SerializeField] private float lifeTimeAfterProjectileDestroyed;

    public void Init(Projectile projectile)
    {
        projectile.OnDestroyed += () =>
        {
            GetComponent<PositionConstraint>().enabled = false;
            Destroy(gameObject, lifeTimeAfterProjectileDestroyed);
        };

        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = projectile.transform;
        constraintSource.weight = 1f;

        GetComponent<PositionConstraint>().AddSource(constraintSource);
    }
}
