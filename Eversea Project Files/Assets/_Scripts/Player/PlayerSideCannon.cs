using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideCannon : MonoBehaviour
{
    [SerializeField] private EnemyRuntimeList allEnemies;

    [SerializeField] private SideCannonUpgradePart upgrades;

    [SerializeField] private Cannon cannon;

    private bool canShoot = false;

    private float atkTimer;

    private void Update()
    {
        atkTimer -= Time.deltaTime;

        if (atkTimer < 0f && canShoot)
            Attack();
    }

    private void OnEnable()
    {
        upgrades.OnCurrentUpgradeChanged += CheckIfUnlocked;
    }

    private void OnDisable()
    {
        upgrades.OnCurrentUpgradeChanged -= CheckIfUnlocked;
    }

    private void CheckIfUnlocked() => canShoot = upgrades.IsUnlocked;

    private void Attack()
    {
        if (allEnemies.Items.Count == 0)
        {
            atkTimer = 0.1f;
            return;
        }

        bool foundEnemy = false;
        float targetSqDist = default;
        EnemyController targetEnemy = null;

        float sqRange = upgrades.GetCurrentUpgrade().Range * upgrades.GetCurrentUpgrade().Range;

        foreach (EnemyController enemy in allEnemies.Items)
        {
            Vector3 enemyPos = enemy.transform.position;
            Vector3 dir = enemyPos - transform.position;

            float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
            float sqDist = Helpers.SqDistance(transform.position, enemyPos);

            if (Mathf.Abs(angle) > upgrades.GetCurrentUpgrade().FOV
                || sqDist > sqRange)
                continue;

            if (!foundEnemy)
            {
                foundEnemy = true;
                targetEnemy = enemy;
            }
            else if(sqDist < targetSqDist)
            {
                targetEnemy = enemy;
                targetSqDist = sqDist;
            }
        }


        if (foundEnemy)
        {
            cannon.InitialiseShoot(targetEnemy.transform, upgrades.GetCurrentUpgrade().Damage, ResetAttackTimer);

            atkTimer = upgrades.GetCurrentUpgrade().AtkSpeed.y + Cannon.MAX_ROTATION_DURATION;
        }
        else
            atkTimer = 0.1f; // For better performance
    }

    void ResetAttackTimer()
    {
        Vector2 cd = upgrades.GetCurrentUpgrade().AtkSpeed;
        atkTimer = UnityEngine.Random.Range(cd.x, cd.y);
    }
}
