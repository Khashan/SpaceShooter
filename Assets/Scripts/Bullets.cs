using UnityEngine;

public class Bullets : PooledObject
{
    private int m_BulletDamage = 0;
    private float m_MoveSpeed = 0f;

    private void BulletInit(int aBulletDamage, float aMoveSpeed)
    {
        m_BulletDamage = aBulletDamage;
        m_MoveSpeed = aMoveSpeed;
    }

    private void Update()
    {
        transform.position += transform.forward * m_MoveSpeed;
    }

    override protected void OnDisable()
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
