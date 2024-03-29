﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionShip : MonoBehaviour
{
    [System.Serializable]
    public struct ShipSocket
    {
        public GameObject m_ShipInScene;
        public GameObject m_ShipPrefabs;
    }

    [SerializeField]
    private TextMeshProUGUI m_TextPress;
    [SerializeField]
    private TextMeshProUGUI m_TextPress2;

    [SerializeField]
    private List<ShipSocket> m_ShipsStruct = new List<ShipSocket>();

    [SerializeField]
    private float m_Radius = 15f;
    [SerializeField]
    [Range(0.01f, 0.99f)]
    private float m_RotateSpeed = 0.2f;

    [SerializeField]
    private GameObject m_LockedShipIndcator;
    [SerializeField]
    private Collider m_LockedShipCollider;

    private PlayerID m_CurrentPlayer;
    private List<GameObject> m_SelectedShip = new List<GameObject>();

    private Dictionary<Transform, float> m_ShipsClock = new Dictionary<Transform, float>();

    private bool m_IsMoving = false;
    private List<Transform> m_CompletedRotation = new List<Transform>();

    private Vector3 m_GoToShip;
    private int m_Direction;

    //Camera for selection
    [SerializeField]
    private float m_LengthRaycast = 5.5f;

    private Camera m_Camera;
    private RaycastHit m_Hit;

    private int m_Players;



    private void Start()
    {
        m_TextPress.enabled = false;

        m_LockedShipIndcator.SetActive(!GameManager.Instance.BossDead);
        m_LockedShipCollider.enabled = GameManager.Instance.BossDead;

        m_Players = GameManager.Instance.PlayerCount;
        m_Camera = Camera.main;

        for (int i = 0; i < m_ShipsStruct.Count; i++)
        {
            m_ShipsClock.Add(m_ShipsStruct[i].m_ShipInScene.transform, i * 15 + 15);
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

            if (m_CompletedRotation.Count == m_ShipsStruct.Count)
            {
                m_IsMoving = false;
            }
        }
        else
        {
            m_Direction = (int)Input.GetAxisRaw("Horizontal" + m_CurrentPlayer);

            if (m_Direction != 0)
            {
                GetNextPosition();
            }
        }

        ShowText();
        PlayerSelectionShip();
    }

    private void MoveShipSlot(KeyValuePair<Transform, float> aShip)
    {
        if (m_CompletedRotation.Contains(aShip.Key))
        {
            return;
        }

        float shipTime = aShip.Value + (m_Direction > 0 ? -m_RotateSpeed : m_RotateSpeed);

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

        for (int i = 0; i < m_ShipsStruct.Count; i++)
        {
            Transform shipTransform = m_ShipsStruct[i].m_ShipInScene.transform;

            float time = m_ShipsClock[shipTransform] + (1 * -m_Direction);
            m_ShipsClock[shipTransform] = time;
        }
    }


    /// <summary >
    ///
    /// </summary>
    private void PlayerSelectionShip()
    {
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out m_Hit, m_LengthRaycast, LayerMask.GetMask("Ship")))
        {
            if (Input.GetButtonDown(m_CurrentPlayer + "Submit"))
            {
                GameObject tToutchShip = GetShipSocketPrefabs(m_Hit.transform.gameObject);   // contient se que le raycast touche 

                if (tToutchShip == null)
                {

                    return;
                }

                m_SelectedShip.Add(tToutchShip);

                GameManager.Instance.ListShipSelect = m_SelectedShip;

                if (m_SelectedShip.Count == GameManager.Instance.PlayerCount)
                {
                    LevelManager.Instance.ChangeLevel("Game", true);
                }
                else
                {
                    m_CurrentPlayer = PlayerID.PlayerTwo;
                }
            }
        }
    }

    private void ShowText()
    {
        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, m_LengthRaycast, LayerMask.GetMask("Ship")))
        {
            if (m_CurrentPlayer == PlayerID.PlayerOne)
            {
                m_TextPress.enabled = true;
            }
            else if (m_CurrentPlayer == PlayerID.PlayerTwo)
            {
                m_TextPress.enabled = false;
                m_TextPress2.enabled = true;
            }
        }
        else
        {
            m_TextPress.enabled = false;
            m_TextPress2.enabled = false;
        }
    }



    /// <summary >
    ///
    /// </summary>
    private GameObject GetShipSocketPrefabs(GameObject aShipSelected)
    {
        for (int i = 0; i < m_ShipsStruct.Count; i++)
        {
            if (m_ShipsStruct[i].m_ShipInScene.Equals(aShipSelected))
            {
                return m_ShipsStruct[i].m_ShipPrefabs;
            }
        }
        return null;
    }
}
