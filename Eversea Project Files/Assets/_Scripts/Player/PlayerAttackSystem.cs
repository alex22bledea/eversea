using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private InputReaderSO input;

    [Space(12)]
    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private MainCannonUpgradePart upgradeList;

    [SerializeField] private float attackBufferTime;

    [SerializeField] private Cannon cannon;

    [SerializeField] private Transform transformPrefab;

    private float lastPressedAttackTimer;
    private float attackTimer;

    private bool hasMouseOverEnemy;
    private Transform targetEnemy;

    private void OnEnable()
    {
        input.AttackEvent += OnAttackInput;
    }

    private void OnDisable()
    {
        input.AttackEvent -= OnAttackInput;
    }

    private void Update()
    {
        lastPressedAttackTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        #region Outlining

        if (!GameManager.Instance.IsPaused)
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 9999f, enemyMask))
            {
                Transform newTargetEnemy = hitInfo.transform;

                if (newTargetEnemy != targetEnemy)
                {
                    if (targetEnemy != null)
                        targetEnemy.GetComponentInChildren<Outline>().enabled = false;

                    newTargetEnemy.GetComponentInChildren<Outline>().enabled = true;
                    targetEnemy = newTargetEnemy;
                }

                hasMouseOverEnemy = true;
            }
            else
            {
                if (hasMouseOverEnemy && targetEnemy != null)
                {
                    targetEnemy.GetComponentInChildren<Outline>().enabled = false;
                    targetEnemy = null;
                }

                hasMouseOverEnemy = false;
            }
        }

        #endregion

        if (hasMouseOverEnemy && attackTimer < 0 && lastPressedAttackTimer > 0)
            Attack();
    }

    private void Attack()
    {
        Vector3 pos = targetEnemy.GetComponent<EnemyController>().ShootCollider.GetRandomPointInsideCollider();
        Transform shootPoint = Instantiate(transformPrefab, targetEnemy);
        shootPoint.position = pos;
        Destroy(shootPoint.gameObject, 30);

        cannon.InitialiseShoot(shootPoint, upgradeList.GetCurrentUpgrade().Damage, ResetAttackTimer);

        attackTimer = upgradeList.GetCurrentUpgrade().AtkSpeed + Cannon.MAX_ROTATION_DURATION;
    }

    void ResetAttackTimer()
    {
        lastPressedAttackTimer = 0f;
        attackTimer = upgradeList.GetCurrentUpgrade().AtkSpeed;
    }

    public void OnAttackInput()
    {
        lastPressedAttackTimer = attackBufferTime;
    }

}
