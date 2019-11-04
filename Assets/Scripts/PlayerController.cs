using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerID
{
    PlayerOne,
    PlayerTwo  
}
public class PlayerController : MonoBehaviour, IDamageable
{
    private int m_Hp;
    private float m_Speed;
    private float m_Tilt;

    private int m_Damage;
    private float m_BulletSpeed;

    [SerializeField]
    private GameObject m_BulletPrefab;

    [SerializeField]
    private PlayerData m_Data;

    [SerializeField]
    private Rigidbody m_Rb;

    private Vector3 m_Input = new Vector3();

    private Vector3 m_MoveDir = new Vector3();


    private void Awake()
    {
        SetupData();
    }

    private void SetupData()
    {
        m_Hp =  m_Data.GetHp();
        m_Speed = m_Data.GetSpeed();
        m_Tilt = m_Data.GetTilt();
        m_BulletSpeed = m_Data.GetBulletSpeed();
        m_Damage = m_Data.GetDamageBullet();
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
        m_Rb.velocity= m_MoveDir;
        SetTilt();
    }

    private void GetInputs()
    {
        m_Input.x = Input.GetAxisRaw("Horizontal");
        m_Input.z = Input.GetAxisRaw("Vertical");
        Debug.Log(m_Input);
        if(Input.GetButton("Fire1"))
        {
            Bullets pooledBullet = PoolManager.Instance.UseObjectFromPool<Bullets>(m_BulletPrefab,transform.position, transform.rotation);
            pooledBullet.BulletInit(m_Damage,m_BulletSpeed);
        }
    }
    private void SetTilt()
    {
        m_Rb.rotation = Quaternion.Euler (0.0f, 0.0f, m_Rb.velocity.x * -m_Tilt);
    }

    public void DamageReceived(int aDamageReceived)
    {
        
    }

    public void HealReceived(int aHealReceived)
    {

    }


    
}
