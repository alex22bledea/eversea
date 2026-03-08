using UnityEngine;

public class EnemyAttackSystem : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO stats;
    [SerializeField] private PlayerReference playerRef;
    [SerializeField] private Cannon cannon;
    [SerializeField] private Transform transformPrefab;

    private float attackTimer;

    private void Update()
    {
        attackTimer -= Time.deltaTime;
    }

    public void TryToAttack()
    {
        if (attackTimer < 0)
            Attack();
    }

    private void Attack()
    {
        Vector3 pos = playerRef.Value.ShootCollider.GetRandomPointInsideCollider();
        Transform shootPoint = Instantiate(transformPrefab, playerRef.Value.transform);
        shootPoint.position = pos;
        Destroy(shootPoint.gameObject, 30);

        cannon.InitialiseShoot(shootPoint, stats.Damage, ResetAttackTimer);

        attackTimer = stats.AttackCooldown.y + Cannon.MAX_ROTATION_DURATION;
    }

    private void ResetAttackTimer()
    {
        attackTimer = UnityEngine.Random.Range(stats.AttackCooldown.x, stats.AttackCooldown.y);
    }
}


