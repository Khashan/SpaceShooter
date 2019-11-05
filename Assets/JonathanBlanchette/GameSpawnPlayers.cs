using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawnPlayers : MonoBehaviour
{
    private void Start()
    {
        GameObject refship = Instantiate(GameManager.Instance.ListShipSelect[0]);
        Debug.Log(GameManager.Instance.ListShipSelect[0]);
        refship.transform.position = new Vector3(0,0,0);
    }
}
