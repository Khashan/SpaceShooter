using UnityEngine;

public class Meteorite : PooledObject, IDamageable
{
    [Header("References")]
    [SerializeField]
    private Rigidbody m_Rb;
    [SerializeField]
    private GameObject m_ExplosionFX;

    [Header("Configuration")]
    [SerializeField]
    private float m_Tumble = 4f;
    [SerializeField]
    private float m_Speed = 4f;
    [SerializeField]
    private int m_Damage = 1;

    private void Awake()
    {
        m_Rb.angularVelocity = Random.insideUnitSphere * m_Tumble;
    }

    private void FixedUpdate()
    {
        m_Rb.velocity = Vector3.forward * -m_Speed;
    }

    private void OnTriggerEnter(Collider aCol)
    {
        IDamageable target = aCol.GetComponent<IDamageable>();
        target?.DamageReceived(m_Damage);

        OnHit();
    }

    private void OnHit()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.UseObjectFromPool(m_ExplosionFX, transform.position, transform.rotation);
    }

    public void DamageReceived(int aDamageReceived)
    {
        GameManager.Instance.AddScore(10);
        OnHit();
    }

    public void HealReceived(int aHealReceived)
    {
    }
}
