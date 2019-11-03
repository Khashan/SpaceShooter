using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Destroy")
            {
                Destroy(gameObject);
                Destroy(this);
            }
    }
}
