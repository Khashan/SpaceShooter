using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawnPlayers : MonoBehaviour
{
    private void Start()
    {
        Instantiate(GameManager.Instance.ListShipSelect[0]);
    }
}
