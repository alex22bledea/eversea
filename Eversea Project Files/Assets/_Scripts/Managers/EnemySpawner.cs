
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Header("Listening to channels")]
    [SerializeField] private VoidEventChannel NightStartedEventChannel;
    [SerializeField] private VoidEventChannel NightEndedEventChannel;

    [Space(12), Header("Enemy Vars")]
    [SerializeField] private EnemySpawnList enemySpawnList;
    [SerializeField] private EnemyRuntimeList spawnedEnemiesList;

    [Space(12)]
    [SerializeField] private float playerViewRange;
    [SerializeField] private Vector3VariableSO playerPos;

    [SerializeField] private FloatVariableSO startingWeight;
    [SerializeField] private FloatVariableSO dailyWeightMultiplier;

    private double lastSpawnTime;

    private Queue<EnemySpawnSO> enemiesToSpawn = new Queue<EnemySpawnSO>();

    private float crTotalWeight;
    double timeBetweenSpawns;

    [SerializeField] private Transform[] spawnPositions;

    protected override void Awake()
    {
        base.Awake();

        crTotalWeight = startingWeight.Value;
    }

    private void OnEnable()
    {
        NightStartedEventChannel.OnEventRaised += GenerateWave;
        NightEndedEventChannel.OnEventRaised += EndWave;
    }

    private void OnDisable()
    {
        NightStartedEventChannel.OnEventRaised -= GenerateWave;
        NightEndedEventChannel.OnEventRaised -= EndWave;
    }

    private void FixedUpdate()
    {
        while (enemiesToSpawn.Count > 0 && lastSpawnTime < TimeSystem.Instance.GetTotalTimePassed() + timeBetweenSpawns)
        {
            SpawnNextEnemy();
            lastSpawnTime += timeBetweenSpawns;
        }
    }

    private void EndWave()
    {
        foreach (EnemyController enemy in spawnedEnemiesList.Items)
            enemy.CustomDestroy();

        enemiesToSpawn.Clear();

        crTotalWeight *= dailyWeightMultiplier.Value; // waves get harder each night
    }

    private void GenerateWave()
    {
        float crWeight = 0f;

        int count = enemySpawnList.Items.Count;

        enemiesToSpawn.Clear();

        while (crWeight < crTotalWeight)
        {
            int index = UnityEngine.Random.Range(0, count);
            EnemySpawnSO enemySpawn = enemySpawnList.Items[index];
            crWeight += enemySpawn.Weight;
            enemiesToSpawn.Enqueue(enemySpawn);
        }

        timeBetweenSpawns = TimeSystem.Instance.GetNightDuration() / enemiesToSpawn.Count;

        lastSpawnTime = TimeSystem.Instance.GetTotalTimePassed();
    }

    private void SpawnNextEnemy()
    {
        if (enemiesToSpawn.Count == 0)
            return;

        EnemySpawnSO enemySpawn = enemiesToSpawn.Dequeue();

        foreach (Transform enemyTr in enemySpawn.GetAllEnemies())
        {
            Transform spawnPosition = spawnPositions.GetRandomElement();

            Transform spawnedEnemyTr = Instantiate(enemyTr, spawnPosition.position, Quaternion.identity);
            spawnedEnemyTr.GetComponent<EnemyController>().Init();
        }
    }
}
