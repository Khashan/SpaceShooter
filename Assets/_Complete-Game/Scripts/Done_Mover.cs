using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;

	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Destroy")
            {
                Destroy(gameObject);
                Destroy(this);
            }
    }
}