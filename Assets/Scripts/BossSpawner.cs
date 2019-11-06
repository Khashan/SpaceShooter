using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private float m_BossTimerToSpawn = 45;
    private float m_CurrentTimerToSpawn = 45;

    [SerializeField]
    private GameObject m_BossPrefab;

    private void Awake()
    {
        m_CurrentTimerToSpawn = m_BossTimerToSpawn;
    }

    private void Update()
    {
        m_CurrentTimerToSpawn -= Time.deltaTime;

        if (m_CurrentTimerToSpawn <= 0)
        {
            Instantiate(m_BossPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
