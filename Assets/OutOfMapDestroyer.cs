using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfMapDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider aCol)
    {
        IDamageable target = aCol.GetComponent<IDamageable>();
        target?.DamageReceived(1000);
    }
}
