using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BossData")]
public class BossData : EnemyData
{
    [Header("Boss")]
    [SerializeField]
    private int m_MaxHealth;
    [SerializeField]
    private float m_AbilityRate;
    [SerializeField]
    private float m_ProtectionBetweenPhases;

    public int MaxHealth
    {
        get { return m_MaxHealth; }
    }

    public float AbilityRate
    {
        get { return m_AbilityRate; }
    }

    public float ProtectionBetweenPhases
    {
        get { return m_ProtectionBetweenPhases; }
    }

}
