﻿using UnityEngine;
using System.Collections;

public class Done_RandomRotator : MonoBehaviour 
{
	public float tumble;
	
	void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}

	private void OnCollisionEnter(Collision aCol)
    {
        Destroy(this);
		if(aCol != null)
		{
			Destroy(gameObject);

		}

    }
}