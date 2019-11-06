using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData", fileName = "NewEnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private float m_FireRate;
    [SerializeField]
    private float m_Delay;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_Tilt;
    [SerializeField]
    private float m_Dodge;
    [SerializeField]
    private float m_Smoothing;

    public float GetFireRate()
    {
        return m_FireRate;
    }

    public float GetDelay()
    {
        return m_Delay;
    }

    public float GetSpeed()
    {
        return m_Speed;
    }

    public float GetTilt()
    {
        return m_Tilt;
    }

    public float GetDodge()
    {
        return m_Dodge;
    }

    public float GetSmoothing()
    {
        return m_Smoothing;
    }

}
