using UnityEngine;

public enum PlayerID
{
    PlayerOne,
    PlayerTwo
}

public class PlayerController : MonoBehaviour, IDamageable
{
    private int m_CurrentHp;
    private int m_MaxHp;
    private int m_MinHp;
    private float m_Speed;
    private float m_Tilt;
    private float m_ShootTimer;

    private float m_CurrentTime;

    private PlayerID m_PlayerID;
    private int m_Damage;
    private float m_BulletSpeed;

    [SerializeField]
    private AudioClip m_SoundBullet;

    [SerializeField]
    private GameObject m_ExplosionSFX;

    [SerializeField]
    private GameObject m_BulletPrefab;

    [SerializeField]
    private PlayerData m_Data;

    [SerializeField]
    private Rigidbody m_Rb;

    private Vector3 m_Input = new Vector3();

    private Vector3 m_MoveDir = new Vector3();

    private bool m_CanShoot = true;

    private void Awake()
    {
        SetupData();
    }

    public void SetUpPlayer(PlayerID a_ID)
    {
        m_PlayerID = a_ID;
    }

    private void SetupData()
    {
        m_MaxHp = m_Data.GetMaxHp();
        m_Speed = m_Data.GetSpeed();
        m_Tilt = m_Data.GetTilt();
        m_BulletSpeed = m_Data.GetBulletSpeed();
        m_Damage = m_Data.GetDamageBullet();
        m_ShootTimer = m_Data.GetShootTimer();
    }
    private void Start()
    {
        m_CurrentHp = m_MaxHp;
        m_CurrentTime = 0f;
    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        m_MoveDir = transform.forward * m_Input.z;
        m_MoveDir += transform.right * m_Input.x;
        m_MoveDir *= m_Speed;
        m_Rb.velocity = m_MoveDir;
        SetTilt();
    }

    private void GetInputs()
    {

        m_Input.x = Input.GetAxisRaw("Horizontal" + m_PlayerID);
        m_Input.z = Input.GetAxisRaw("Vertical" + m_PlayerID);
        if (Input.GetButton("Fire1" + m_PlayerID) & m_CanShoot)
        {
            Bullets pooledBullet = PoolManager.Instance.UseObjectFromPool<Bullets>(m_BulletPrefab, transform.position, transform.rotation);
            pooledBullet.BulletInit(m_Damage, m_BulletSpeed);

            AudioManager.Instance.PlaySFX(m_SoundBullet, transform.position);
            m_CanShoot = false;
        }
        Cooldown();
    }
    private void SetTilt()
    {
        m_Rb.rotation = Quaternion.Euler(0.0f, 0.0f, m_Rb.velocity.x * -m_Tilt);
    }
    private void Cooldown()
    {
        if (!m_CanShoot)
        {
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime >= m_ShootTimer)
            {
                m_CanShoot = true;
                m_CurrentTime = 0f;
            }
        }
    }

    public void DamageReceived(int aDamageReceived)
    {
        m_CurrentHp -= aDamageReceived;
        if (m_CurrentHp <= 0)
        {
            m_CurrentHp = 0;
            Death();
        }
    }

    private void Death()
    {
        Instantiate(m_ExplosionSFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void HealReceived(int aHealReceived)
    {
        m_CurrentHp += aHealReceived;
        if (m_CurrentHp >= m_MaxHp)
        {
            m_CurrentHp = m_MaxHp;
        }
    }



}
