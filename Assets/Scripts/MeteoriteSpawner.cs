using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_AsteroidPrefabs = new List<GameObject>();

    [SerializeField]
    private Collider m_BoxCollider;

    [SerializeField]
    private float m_MinSpawnWaveRate = 0.5f;
    [SerializeField]
    private float m_MaxSpawnWaveRate = 1.50f;

    private float m_CurrentSpawnRate = 0;

    [SerializeField]
    private int m_MinSpawnPerWave = 1;
    [SerializeField]
    private int m_MaxSpawnPerWave = 5;

    private Vector3 m_RandomSpawnPoint;

    private void Awake()
    {
        m_CurrentSpawnRate = GetNextSpawnWaveRate();
    }

    private void Update()
    {
        m_CurrentSpawnRate -= Time.deltaTime;

        if (m_CurrentSpawnRate <= 0)
        {
            m_CurrentSpawnRate = GetNextSpawnWaveRate();
            SpawnRandomAsteroid();
        }
    }

    private float GetNextSpawnWaveRate()
    {
        return Random.Range(m_MinSpawnWaveRate, m_MaxSpawnPerWave);
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

    private void SetRandomSpawnPoint()
    {
        m_RandomSpawnPoint.x = Random.Range(m_BoxCollider.bounds.min.x, m_BoxCollider.bounds.max.x);
        m_RandomSpawnPoint.y = Random.Range(m_BoxCollider.bounds.min.y, m_BoxCollider.bounds.max.y);
        m_RandomSpawnPoint.z = Random.Range(m_BoxCollider.bounds.min.z, m_BoxCollider.bounds.max.z);
    }
}
