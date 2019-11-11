using System.Collections;
using UnityEngine;

public class Enemy : PooledObject, IDamageable
{
    private float m_CurrentSpeed;
    private float m_TargetManeuver;

    [SerializeField]
    private float m_Speed = 5f;
    [SerializeField]
    private Rigidbody m_RB;
    [SerializeField]
    private EnemyData m_EnemyData;

    [Header("weapon")]
    [SerializeField]
    private GameObject m_BulletPrefab;
    [SerializeField]
    private Transform m_ShotSpawn;
    [SerializeField]
    private float m_FireRate = 1.5f;
    [SerializeField]
    private float m_Delay = 0.5f;
    [SerializeField]
    private float m_BulletSpeed = .5f;
    [SerializeField]
    private AudioClip m_AttackSound;
    [SerializeField]
    private GameObject m_ExplosionEffect;

    [Header("EvasiveManeuver")]
    [SerializeField]
    private float m_Tilt = 10f;
    [SerializeField]
    private float m_Dodge = 5f;
    [SerializeField]
    private float m_Smoothing = 7.5f;
    [SerializeField]
    private Vector2 m_StartWait = new Vector2();
    [SerializeField]
    private Vector2 m_ManeuverTime = new Vector2();
    [SerializeField]
    private Vector2 m_ManeuverWait = new Vector2();

    private void Awake()
    {
        SetData();
    }

    private void Start()
    {
        m_RB.velocity = transform.forward * m_Speed;
        m_CurrentSpeed = m_RB.velocity.z;
    }

    private void OnEnable()
    {
        InvokeRepeating("Fire", m_Delay, m_FireRate);
        StartCoroutine(Evade());
    }

    protected override void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();

        base.OnDisable();
    }


    private IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(m_StartWait.x, m_StartWait.y));
        while (true)
        {
            m_TargetManeuver = Random.Range(1, m_Dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(m_ManeuverTime.x, m_ManeuverTime.y));
            m_TargetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(m_ManeuverWait.x, m_ManeuverWait.y));
        }
    }

    private void SetData()
    {
        m_Speed = m_EnemyData.GetSpeed();
        m_FireRate = m_EnemyData.GetFireRate();
        m_Delay = m_EnemyData.GetDelay();
        m_Tilt = m_EnemyData.GetTilt();
        m_Dodge = m_EnemyData.GetDodge();
        m_Smoothing = m_EnemyData.GetSmoothing();
        m_BulletSpeed = m_EnemyData.GetBulletSpeed();
    }

    private void FixedUpdate()
    {
        Evasive();

    }

    private void Evasive()
    {

        m_RB.rotation = Quaternion.Euler(0, 0, m_RB.velocity.x * -m_Tilt);
        float newManeuver = Mathf.MoveTowards(m_RB.velocity.x, m_TargetManeuver, m_Smoothing * Time.deltaTime);
        m_RB.velocity = new Vector3(newManeuver, 0.0f, m_CurrentSpeed);

        m_RB.rotation = Quaternion.Euler(0, 0, m_RB.velocity.x * -m_Tilt);
    }

    private void Fire()
    {
        Bullets pooledBullet = PoolManager.Instance.UseObjectFromPool<Bullets>(m_BulletPrefab, m_ShotSpawn.position, m_ShotSpawn.rotation);
        pooledBullet.BulletInit(1, m_BulletSpeed);

        AudioManager.Instance.PlaySFX(m_AttackSound, transform.position);
    }

    public void DamageReceived(int aDamageReceived)
    {
        GameManager.Instance.AddScore(50);
        Death();
    }

    private void Death()
    {
        PoolManager.Instance.UseObjectFromPool(m_ExplosionEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public void HealReceived(int aHealReceived)
    {
    }

    private void OnCollisionEnter(Collision aCol)
    {
        IDamageable player = aCol.transform.GetComponent<IDamageable>();

        if (player != null)
        {
            player.DamageReceived(1);
            Death();
        }
    }
}
