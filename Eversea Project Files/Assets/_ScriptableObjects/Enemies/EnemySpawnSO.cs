using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new EnemySpawnSO", menuName = "ScriptableObjects/Enemies/EnemySpawn")]
public class EnemySpawnSO : ScriptableObject
{
    [field: SerializeField] public float Weight { get; private set; }
    [SerializeField] private List<Transform> EnemyPrefabList;

    public IEnumerable<Transform> GetAllEnemies() => EnemyPrefabList;
}
