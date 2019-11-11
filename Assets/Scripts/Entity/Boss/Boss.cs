using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss : MonoBehaviour, IDamageable
{
    [System.Serializable]
    public struct BossPhase
    {
        public string m_NamePhase;
        public BossData m_BossPhaseData;
        public int m_EndPhaseHealth;
        public ScriptableBossAbility m_AbilityCast;
        public bool m_AllowEnemyToSpawn;
    }

    [SerializeField]
    private Rigidbody m_RB;

    [SerializeField]
    private GameObject m_BulletPrefab;
    [SerializeField]
    private AudioClip m_AutoAttackAudio;
    [SerializeField]
    private AudioClip m_SwitchPhaseSFX;

    [SerializeField]
    private BossData m_Data;

    [SerializeField]
    private List<BossPhase> m_Phases = new List<BossPhase>();

    private float m_ProtectionBetweenPhases = 1.25f;
    private float m_CurrentProtectionTimer = 0f;

    private int m_MaxHealth = 100;
    private int m_CurrentHealth = 0;

    private float m_AttackRate = 0.75f;
    private float m_CurrentAttackTime = 0f;

    private float m_AbilityRate = 3.5f;
    private float m_CurrentAbilityTime = 0;

    private int m_BulletDamage = 1;
    private float m_BulletSpeed = 2f;

    private float m_Tilt = 10f;
    private float m_Dodge = 5f;
    private float m_Smoothing = 7.5f;

    [Header("Moves Paterns")]
    [SerializeField]
    private Vector2 m_StartWait = new Vector2();
    [SerializeField]
    private Vector2 m_ManeuverTime = new Vector2();
    [SerializeField]
    private Vector2 m_ManeuverWait = new Vector2();

    private bool m_IsGodMode = false;
    private float m_TargetManeuver;

    private int m_CurrentPhase = -1;

    private void Awake()
    {
        InitData();
        ChangeBossStage();
    }

    private void InitData()
    {
        m_MaxHealth = m_Data.MaxHealth;
        m_CurrentHealth = m_MaxHealth;

        m_Tilt = m_Data.GetTilt();
        m_Dodge = m_Data.GetDodge();
        m_Smoothing = m_Data.GetSmoothing();
        m_BulletSpeed = m_Data.GetBulletSpeed();
        m_AttackRate = m_Data.GetFireRate();

        m_AbilityRate = m_Data.AbilityRate;
        m_ProtectionBetweenPhases = m_Data.ProtectionBetweenPhases;
    }

    private void OnEnable()
    {
        StartCoroutine(Evade());
    }

    private void Update()
    {
        if (m_CurrentProtectionTimer > 0)
        {
            ProtectionTimer();
        }

        AttackTimer();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        Evasive();
    }


    private void ProtectionTimer()
    {
        m_CurrentProtectionTimer -= Time.deltaTime;

        if (m_CurrentProtectionTimer <= 0)
        {
            m_IsGodMode = false;
        }
    }

    private void AttackTimer()
    {
        m_CurrentAttackTime -= Time.deltaTime;

        if (m_CurrentAttackTime <= 0)
        {
            m_CurrentAttackTime = m_AttackRate;
            AutoAttack();
        }
        else if (!m_IsGodMode)
        {
            if (m_CurrentAbilityTime <= 0f)
            {
                AbilityAttack();
            }
            else
            {
                m_CurrentAbilityTime -= Time.deltaTime;
            }
        }
    }

    private void AutoAttack()
    {
        AudioManager.Instance.PlaySFX(m_AutoAttackAudio, transform.position);
        Bullets projectile = PoolManager.Instance.UseObjectFromPool<Bullets>(m_BulletPrefab, transform.position, Quaternion.identity);
        projectile.BulletInit(m_BulletDamage, m_BulletSpeed);
    }

    private void AbilityAttack()
    {
        m_CurrentAttackTime = m_AttackRate;
        m_CurrentAbilityTime = m_AbilityRate;
        StartCoroutine(m_Phases[m_CurrentPhase].m_AbilityCast.CastAbility(transform));
    }

    private void ChangeBossStage()
    {
        m_IsGodMode = true;

        AudioManager.Instance.PlaySFX(m_SwitchPhaseSFX, transform.position);

        m_CurrentProtectionTimer = m_ProtectionBetweenPhases;
        m_CurrentAbilityTime = m_AbilityRate;
        m_CurrentAttackTime = m_AttackRate;

        m_CurrentPhase++;

        if (m_CurrentPhase >= m_Phases.Count)
        {
            m_CurrentPhase--;
            Debug.LogError("OutOfBounds Boss Phases! Be sure than the last boss phase has the endhealth set lower or equals of 0");
        }

        m_Data = m_Phases[m_CurrentPhase].m_BossPhaseData;
        GameManager.Instance.AllowEnemyToSpawn = m_Phases[m_CurrentPhase].m_AllowEnemyToSpawn;
        InitData();
    }

    public void DamageReceived(int aDamageReceived)
    {
        if (!m_IsGodMode)
        {
            m_CurrentHealth -= aDamageReceived;

            int endPhaseHealth = m_Phases[m_CurrentPhase].m_EndPhaseHealth;

            if (m_CurrentHealth <= 0)
            {
                Death();
            }
            else if (m_CurrentHealth <= endPhaseHealth)
            {
                ChangeBossStage();
            }
        }
    }

    public void HealReceived(int aHealReceived)
    {
        m_CurrentHealth += aHealReceived;

        if (m_CurrentHealth > m_MaxHealth)
        {
            m_CurrentHealth = m_MaxHealth;
        }
    }

    private void Death()
    {
        //Security if, for some reason, people overcome the damage.
        if (m_CurrentPhase + 1 < m_Phases.Count)
        {
            ChangeBossStage();
        }
        else
        {
            GameManager.Instance.BossDied();
            GameManager.Instance.AllowEnemyToSpawn = true;
            GameManager.Instance.BossDead = true;
            Destroy(gameObject);
        }
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

    private void Evasive()
    {
        m_RB.rotation = Quaternion.Euler(0, 0, m_RB.velocity.x * -m_Tilt);
        float newManeuver = Mathf.MoveTowards(m_RB.velocity.x, m_TargetManeuver, m_Smoothing * Time.deltaTime);
        float speed = m_Data.GetSpeed() * (newManeuver > 0 ? 1 : -1) * Time.deltaTime;
        m_RB.velocity = transform.right * newManeuver + (Vector3.right * (newManeuver == 0 ? 0 : speed));
    }
}

