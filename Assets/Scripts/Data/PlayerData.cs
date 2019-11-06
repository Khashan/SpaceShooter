using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "NewPlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private int m_MaxHp;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_Tilt;
    [SerializeField]
    private int m_DamageBullet;
    [SerializeField]
    private float m_BulletSpeed;
    [SerializeField]
    private float m_ShootTimer;


    public int GetMaxHp()
    {
        return m_MaxHp;
    }

    public float GetShootTimer()
    {
        return m_ShootTimer;
    }

    public float GetSpeed()
    {
        return m_Speed;
    }
    public float GetTilt()
    {
        return m_Tilt;
    }
    public int GetDamageBullet()
    {
        return m_DamageBullet;
    }
    public float GetBulletSpeed()
    {
        return m_BulletSpeed;
    }



}
