using UnityEngine;

public class Bullets : PooledObject
{
    private int m_BulletDamage = 0;
    private float m_MoveSpeed = 0f;

    [SerializeField]
    private float m_Lifespan = 4f;
    private float m_CurrentLifeSpan = 0f;

    private void OnEnable()
    {
        m_CurrentLifeSpan = m_Lifespan;
    }

    public void BulletInit(int aBulletDamage, float aMoveSpeed)
    {
        m_BulletDamage = aBulletDamage;
        m_MoveSpeed = aMoveSpeed;
    }

    private void Update()
    {
        transform.position += transform.forward * m_MoveSpeed;

        m_CurrentLifeSpan -= Time.deltaTime;
        if(m_CurrentLifeSpan <= 0 )
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        m_BulletDamage = 0;
        m_MoveSpeed = 0f;
        base.OnDisable();
    }

    private void OnTriggerEnter(Collider aCol)
    {
        IDamageable iDmg = aCol.GetComponent<IDamageable>();

        if(iDmg != null)
        {
            iDmg.DamageReceived(m_BulletDamage);
            gameObject.SetActive(false);
        }
    }
}
