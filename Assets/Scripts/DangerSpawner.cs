using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_AsteroidPrefabs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> m_EnemyPrefabs = new List<GameObject>();

    [SerializeField]
    private Collider m_BoxCollider;

    [Header("Asteroid")]
    [SerializeField]
    private float m_MinSpawnWaveRate = 0.5f;
    [SerializeField]
    private float m_MaxSpawnWaveRate = 1.50f;
    [SerializeField]
    private int m_MinSpawnPerWave = 1;
    [SerializeField]
    private int m_MaxSpawnPerWave = 5;

    [Header("Enemies")]
    [SerializeField]
    private float m_MinSpawnWaveEnemyRate = 1.25f;
    [SerializeField]
    private float m_MaxSpawnWaveEnemyRate = 3f;
    [SerializeField]
    private int m_MinEnemySpawnPerWave = 1;
    [SerializeField]
    private int m_MaxEnemySpawnPerWave = 5;

    private float m_CurrentSpawnRate = 0;
    private float m_CurrentEnemySpawnRate = 0;
    private Vector3 m_RandomSpawnPoint;

    private void Awake()
    {
        m_CurrentSpawnRate = GetNextSpawnWaveRate();
    }

    private void Update()
    {
        if (!GameManager.Instance.AllowEnemyToSpawn)
        {
            return;
        }

        if (m_AsteroidPrefabs.Count > 0)
        {
            m_CurrentSpawnRate -= Time.deltaTime;

            if (m_CurrentSpawnRate <= 0)
            {
                m_CurrentSpawnRate = GetNextSpawnWaveRate();
                SpawnRandomAsteroid();
            }
        }

        if (m_EnemyPrefabs.Count > 0)
        {
            m_CurrentEnemySpawnRate -= Time.deltaTime;

            if (m_CurrentEnemySpawnRate <= 0)
            {
                m_CurrentEnemySpawnRate = GetNextEnemySpawnWaveRate();
                SpawnRandomEnemy();
            }
        }
    }

    private float GetNextSpawnWaveRate()
    {
        return Random.Range(m_MinSpawnWaveRate, m_MaxSpawnPerWave);
    }

    private float GetNextEnemySpawnWaveRate()
    {
        return Random.Range(m_MinSpawnWaveEnemyRate, m_MaxSpawnWaveEnemyRate);
    }

    private void SpawnRandomAsteroid()
    {
        int qtyToSpawn = Random.Range(m_MinSpawnPerWave, m_MaxSpawnPerWave + 1);

        for (int i = 0; i < qtyToSpawn; i++)
        {
            SetRandomSpawnPoint();
            GameObject randomPrfab = m_AsteroidPrefabs[Random.Range(0, m_AsteroidPrefabs.Count)];
            PoolManager.Instance.UseObjectFromPool(randomPrfab, m_RandomSpawnPoint, transform.rotation);
        }
    }

    private void SpawnRandomEnemy()
    {
        int qtyToSpawn = Random.Range(m_MinEnemySpawnPerWave, m_MaxEnemySpawnPerWave + 1);

        for (int i = 0; i < qtyToSpawn; i++)
        {
            SetRandomSpawnPoint();
            GameObject randomPrfab = m_EnemyPrefabs[Random.Range(0, m_EnemyPrefabs.Count)];
            PoolManager.Instance.UseObjectFromPool(randomPrfab, m_RandomSpawnPoint, transform.rotation);
        }
    }

    private void SetRandomSpawnPoint()
    {
        m_RandomSpawnPoint.x = Random.Range(m_BoxCollider.bounds.min.x, m_BoxCollider.bounds.max.x);
        m_RandomSpawnPoint.y = Random.Range(m_BoxCollider.bounds.min.y, m_BoxCollider.bounds.max.y);
        m_RandomSpawnPoint.z = Random.Range(m_BoxCollider.bounds.min.z, m_BoxCollider.bounds.max.z);
    }
}
