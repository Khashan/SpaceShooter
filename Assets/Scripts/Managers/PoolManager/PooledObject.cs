using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    private GameObject m_PoolOwner;
    public GameObject PoolOwner
    {
        get { return m_PoolOwner; }
    }

    protected bool m_HasBeendInitialized = false;

    public virtual void InitPooledObject(GameObject a_PoolOwner)
    {
        m_PoolOwner = a_PoolOwner;
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        m_HasBeendInitialized = true;

        PoolManager.Instance.ReturnedPooledObject(this);
    }

}