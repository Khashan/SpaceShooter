using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    private enum BossState
    {
        PHASE_01,
        PHASE_02,
        PHASE_03,
        Count
    }

    private StateMachine m_StateMachine;
    
    [SerializeField]
    private GameObject m_BulletPrefab;

    [SerializeField]
    private List<int> m_PhaseHealthMax = new List<int> { 70, 30 };


    [SerializeField]
    private float m_ProtectionBetweenPhases = 1.25f;
    private float m_CurrentProtectionTimer = 0f;

    [SerializeField]
    private int m_MaxHealth = 100;
    private int m_CurrentHealth = 0;

    [SerializeField]
    private float m_AttackRate = 0.75f;
    private float m_CurrentAttackTime = 0f;

    [SerializeField]
    private float m_AbilityRate = 3.5f;
    private float m_CurrentAbilityTime = 0;

    [SerializeField]
    private int m_BulletDamage = 1;
    [SerializeField]
    private float m_BulletSpeed = 2f;

    private bool m_IsGodMode = false;
    private BossState m_CurrentStage;
    private Coroutine m_Ability;


    private void Awake()
    {
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        m_StateMachine = new StateMachine(BossState.Count.ToInt());
        m_StateMachine.AddState(BossState.PHASE_01.ToInt(), Phase01Enter, Phase01Update, Phase01Exit);
        m_StateMachine.AddState(BossState.PHASE_02.ToInt(), Phase02Enter, Phase02Update, Phase02Exit);
        m_StateMachine.AddState(BossState.PHASE_02.ToInt(), Phase03Enter, Phase03Update, Phase03Exit);

        m_StateMachine.ChangeState(BossState.PHASE_01.ToInt());
    }

    private void Update()
    {
        m_StateMachine.Update();

        if (m_CurrentProtectionTimer > 0)
        {
            ProtectionTimer();
        }
        else if(m_Ability == null)
        {
            AutoAttackTimer();
        }

    }

    private void ProtectionTimer()
    {
        m_CurrentProtectionTimer -= Time.deltaTime;

        if (m_CurrentProtectionTimer <= 0)
        {
            m_IsGodMode = false;
        }
    }

    private void AutoAttackTimer()
    {
        m_CurrentAttackTime -= Time.deltaTime;

        if(m_CurrentAttackTime <= 0)
        {
            m_CurrentAttackTime = m_AttackRate;
            AutoAttack();
        }
    }

    private void AutoAttack()
    {
        Bullets projectile = PoolManager.Instance.UseObjectFromPool<Bullets>(m_BulletPrefab, transform.position, transform.rotation);
        projectile.BulletInit(m_BulletDamage, m_BulletSpeed);
    }


    #region Phases

    //Phase 01
    private void Phase01Enter()
    {
        ChangeBossStage(BossState.PHASE_01);
        m_CurrentAbilityTime = m_AbilityRate;
        m_CurrentAttackTime = 0f;
    }

    private void Phase01Update()
    {
        if(!m_IsGodMode && m_Ability == null)
        {
            if(m_CurrentAbilityTime <= 0f)
            {
                m_CurrentAbilityTime = m_AbilityRate;
                m_Ability = StartCoroutine(Phase01Ability());
            }
            else
            {
                m_CurrentAbilityTime -= Time.deltaTime;
            }
        }
    }

    private void Phase01Exit()
    {

    }

    //Phase 02
    private void Phase02Enter()
    {
        ChangeBossStage(BossState.PHASE_02);
    }

    private void Phase02Update()
    {

    }

    private void Phase02Exit()
    {

    }

    //Phase 03
    private void Phase03Enter()
    {
        ChangeBossStage(BossState.PHASE_03);
    }

    private void Phase03Update()
    {

    }

    private void Phase03Exit()
    {

    }

    private void ChangeBossStage(BossState aBossState)
    {
        m_IsGodMode = true;
        m_CurrentProtectionTimer = m_ProtectionBetweenPhases;
        m_CurrentStage = aBossState;
    }


    private IEnumerator Phase01Ability()
    {
        int rotationGaps = 15;
        int rotation = rotationGaps;

        while(rotation <= 360)
        {
            transform.Rotate(Vector3.up * rotationGaps);
            rotation += rotationGaps;
            AutoAttack();
        }
        yield return null;

        m_Ability = null;
    }

    public void DamageReceived(int aDamageReceived)
    {
        if (!m_IsGodMode)
        {
            m_CurrentHealth -= aDamageReceived;

            int m_NextPhaseHealth = 0;
            int m_NextStage = -1;

            if (m_StateMachine.GetCurrentState() != BossState.PHASE_03.ToInt())
            {
                m_NextPhaseHealth = m_PhaseHealthMax[m_CurrentStage.ToInt() +1];
                m_NextStage = m_CurrentStage.ToInt() + 1; ;
            }

            if (m_CurrentHealth <= m_NextPhaseHealth)
            {
                m_CurrentHealth = m_NextPhaseHealth;

                if (m_NextStage != -1)
                {
                    m_StateMachine.ChangeState((m_CurrentStage.ToInt() + 1));
                }
            }

            if (m_CurrentHealth <= 0)
            {
                Death();
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

    }
    #endregion

}

