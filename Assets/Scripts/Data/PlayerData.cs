using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName ="NewPlayerData", order =1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private int m_Hp;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_Tilt;
    [SerializeField]
    private int m_DamageBullet;
    [SerializeField]
    private float m_BulletSpeed;
    

    public int GetHp()
    {
        return m_Hp;
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
