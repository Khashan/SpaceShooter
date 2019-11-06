using UnityEngine;

public class PooledFX : PooledObject
{
    [SerializeField]
    private float m_LifeTime = 2f;
    private float m_CurrentLifeTime;

    [SerializeField]
    private AudioClip m_SFX;

    private void OnEnable()
    {
        if (m_HasBeendInitialized)
        {
            AudioManager.Instance.PlaySFX(m_SFX, transform.position);
        }

        m_CurrentLifeTime = m_LifeTime;
    }

    private void Update()
    {
        m_CurrentLifeTime -= Time.deltaTime;

        if (m_CurrentLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
