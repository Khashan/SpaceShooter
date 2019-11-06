using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawnPlayers : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private int m_IdPlayerToSpawn;

    private void Start()
    {
        if (m_IdPlayerToSpawn >= GameManager.Instance.ListShipSelect.Count)
        {
            return;
        }

        GameObject refship = Instantiate(GameManager.Instance.ListShipSelect[m_IdPlayerToSpawn],transform.position,Quaternion.identity);
    }
}
