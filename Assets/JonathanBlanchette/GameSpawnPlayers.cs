using UnityEngine;

public class GameSpawnPlayers : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private int m_IdPlayerToSpawn;

    private void Start()
    {
        if (m_IdPlayerToSpawn < GameManager.Instance.ListShipSelect.Count)
        {
            PlayerController refship = Instantiate(GameManager.Instance.ListShipSelect[m_IdPlayerToSpawn], transform.position, Quaternion.identity).GetComponent<PlayerController>();

            if (refship == null)
            {
                Destroy(refship);
            }
            else
            {
                refship.SetUpPlayer((PlayerID)m_IdPlayerToSpawn);
            }
        }

        Destroy(gameObject);
    }
}
