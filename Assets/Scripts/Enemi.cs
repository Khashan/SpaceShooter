using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemi : MonoBehaviour
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
    private GameObject m_Shot;
    [SerializeField]
	private Transform m_ShotSpawn;
    [SerializeField]
	private float m_FireRate = 1.5f;
    [SerializeField]
	private float m_Delay = 0.5f;

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

    }

	private void Start ()
	{
        m_RB.velocity = transform.forward * m_Speed;
        m_CurrentSpeed = m_RB.velocity.z;
		InvokeRepeating ("Fire", m_Delay, m_FireRate);
        StartCoroutine(Evade());
	}


    IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (m_StartWait.x, m_StartWait.y));
		while (true)
		{
			m_TargetManeuver = Random.Range (1, m_Dodge) * -Mathf.Sign (transform.position.x);
			yield return new WaitForSeconds (Random.Range (m_ManeuverTime.x, m_ManeuverTime.y));
			m_TargetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (m_ManeuverWait.x, m_ManeuverWait.y));
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
    }

    private void FixedUpdate()
    {        
        Evasive();

    }

    private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Destroy")
            {
                Destroy(gameObject);
            }
    }

    private void Evasive()
    {
            
		m_RB.rotation = Quaternion.Euler (0, 0, m_RB.velocity.x * -m_Tilt);
        float newManeuver = Mathf.MoveTowards (m_RB.velocity.x, m_TargetManeuver, m_Smoothing * Time.deltaTime);
		m_RB.velocity = new Vector3 (newManeuver, 0.0f, m_CurrentSpeed);
		
		m_RB.rotation = Quaternion.Euler (0, 0, m_RB.velocity.x * -m_Tilt);
    }

	private void Fire ()
	{
		Instantiate(m_Shot, m_ShotSpawn.position, m_ShotSpawn.rotation);
		GetComponent<AudioSource>().Play();
	}

}
