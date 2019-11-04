using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float m_Speed;
    private float m_Tilt;

    [SerializeField]
    private Rigidbody m_Rb;

    private Vector3 m_Input = new Vector3();


    private void Start()
    {

    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        
    }

    private void GetInputs()
    {
        m_Input.x = Input.GetAxisRaw("Horizontal");
        m_Input.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Fire1"))
        {
            
        }
    }

    
}
