using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTesting : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform pos;

    void Start()
    {
        Invoke("SpwanEnemy", 1f);
    }

    public void SpwanEnemy()
    {
        Instantiate(enemyPrefab, pos.position, Quaternion.identity);
    }
}
