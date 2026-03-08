using Crest;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO stats;
    [SerializeField] private Vector3VariableSO playerPos;
    [SerializeField] private EnemyRuntimeList spawnedEnemiesList;

    [field: Space(12)]
    [field: SerializeField] public BoxCollider ShootCollider { get; private set; }

    [Space(12)]
    [SerializeField] private float animDuration;

    [Space(12)]
    [SerializeField] private float bottomH;
    [SerializeField] private float topH;

    private HealthSystem healthSystem;

    private CustomControlledBoat movement;

    private EnemyAttackSystem attackSystem;

    private bool hadProperDestroy;

    private bool isActive = false;

    private bool isSpawning = false;
    private bool isDespawning = false;

    private void Awake()
    {
        spawnedEnemiesList.Add(this);
        movement = GetComponent<CustomControlledBoat>();
        healthSystem = GetComponent<HealthSystem>();
        attackSystem = GetComponent<EnemyAttackSystem>();

        isActive = false;
    }

    private void Start()
    {
        healthSystem.OnDeath += HealthSystem_OnDeath;

        movement._turnPower = stats.TurnPower;
        movement._enginePower = stats.EnginePower;
    }

    private void HealthSystem_OnDeath()
    {
        StartCoroutine(DespawnEnemy());
    }

    private void Update()
    {
        if ( !isActive)
            return;

        float sqDistToPlayer = Helpers.SqDistance(playerPos.Value, transform.position);

        movement.SetMoveInput(CalculateMoveInput(sqDistToPlayer));

        if(sqDistToPlayer.IsInBetween(stats.SqRange.x, stats.SqRange.y))
            attackSystem.TryToAttack();
    }

    public void Init()
    {
        isActive = true;
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        if (isSpawning)
            yield break;

        isSpawning = true;

        isActive = false;

        float time = 0f;

        while (time < 1f)
        {
            movement._bottomH = Mathf.Lerp(bottomH, topH, time);
            time += Time.deltaTime / animDuration;
            yield return null;
        }

        yield return null; /// Skip 1 frame

        Debug.Log(transform.position);

        isActive = true; ;

        isSpawning = false;
    }

    IEnumerator DespawnEnemy()
    {
        if (isDespawning)
            yield break;

        isDespawning = true;

        isActive = false;

        float time = 0f;

        while (time < 1f)
        {
            movement._bottomH = Mathf.Lerp(topH, bottomH, time);

            time += Time.deltaTime / animDuration;
            yield return null;
        }

        CustomDestroy();

        isDespawning = false;
    }

    private Vector2 CalculateMoveInput(float sqDistToPlayer)
    {
        Vector2 moveInput = Vector2.zero;

        Vector3 directionToPlayer = playerPos.Value - transform.position;
        directionToPlayer.y = 0;

        /// calculate rotation

        float angle = Vector3.SignedAngle(transform.forward, directionToPlayer.normalized, Vector3.up);

        if (Mathf.Abs(angle) > 10f) /// threshold to avoid jitter
        {
            moveInput.x = Mathf.Sign(angle);
        }
        else
            moveInput.x = 0f;

        /// calculate movement forward/backwards

        if (sqDistToPlayer < stats.SqRange.x)
            moveInput.y = -1f; /// moves backwards
        else if (sqDistToPlayer < stats.SqRange.y)
            moveInput.y = 0f;  /// stays in place
        else
            moveInput.y = 1f;  /// moves forward

        return moveInput;
    }

    public void CustomDestroy()
    {
        if (hadProperDestroy)
            return;

        hadProperDestroy = true;

        spawnedEnemiesList.Remove(this);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CustomDestroy();
    }

    private void OnDrawGizmos()
    {
        if (stats == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 100f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(stats.SqRange.x));
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(stats.SqRange.y));
    }
}
