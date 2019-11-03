using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
    TODO:
    Make so what in front of the camera is the Selected Ship
    Make so if there's an empy space to the direction the player wants -> Do not rotate or Rotate 2x
 */

public class TestSelectorBehaviourFUCKYOUJO : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_Ships = new List<GameObject>();

    [SerializeField]
    private float m_Radius = 15f;
    [SerializeField]
    [Range(0.01f, 0.99f)]
    private float m_RotateSpeed = 0.2f;


    private Dictionary<Transform, float> m_ShipsClock = new Dictionary<Transform, float>();

    private bool m_IsMoving = false;
    private List<Transform> m_CompletedRotation = new List<Transform>();

    private Vector3 m_GoToShip;
    private int m_Direction;


    private void Start()
    {
        for (int i = 0; i < m_Ships.Count; i++)
        {
            m_ShipsClock.Add(m_Ships[i].transform, i * 15);
        }
    }

    private void Update()
    {
        if (m_IsMoving)
        {
            for (int i = 0; i < m_ShipsClock.Count; i++)
            {
                MoveShipSlot(m_ShipsClock.ElementAt(i));
            }

            if (m_CompletedRotation.Count == m_Ships.Count)
            {
                m_IsMoving = false;
            }
        }
        else
        {
            m_Direction = (int)Input.GetAxisRaw("Horizontal");

            if (m_Direction != 0)
            {
                GetNextPosition();
            }
        }
    }

    private void MoveShipSlot(KeyValuePair<Transform, float> aShip)
    {
        if (m_CompletedRotation.Contains(aShip.Key))
        {
            return;
        }

        float shipTime = aShip.Value + (m_Direction > 0 ? m_RotateSpeed : -m_RotateSpeed);

        GetShipNextTickPosition(aShip.Key, shipTime);

        if ((int)shipTime % 15 == 0)
        {
            m_CompletedRotation.Add(aShip.Key);
            GetShipNextTickPosition(aShip.Key, (int)shipTime);
        }

        aShip.Key.position = m_GoToShip;
    }

    private void GetShipNextTickPosition(Transform aShip, float aTime)
    {
        m_ShipsClock[aShip] = aTime;

        float angle = (aTime * 6) * Mathf.Deg2Rad;

        float x = Mathf.Cos(angle) * m_Radius;
        float z = Mathf.Sin(angle) * m_Radius;

        m_GoToShip.x = x;
        m_GoToShip.z = z;
    }

    private void GetNextPosition()
    {
        m_CompletedRotation.Clear();
        m_IsMoving = true;

        for (int i = 0; i < m_Ships.Count; i++)
        {
            Transform shipTransform = m_Ships[i].transform;

            float time = m_ShipsClock[shipTransform] + (1 * m_Direction);
            m_ShipsClock[shipTransform] = time;
        }
    }
}
