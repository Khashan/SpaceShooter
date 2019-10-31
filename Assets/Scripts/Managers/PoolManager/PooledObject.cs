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

    public virtual void InitPooledObject(GameObject a_PoolOwner)
    {
        m_PoolOwner = a_PoolOwner;
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        PoolManager.Instance.ReturnedPooledObject(this, PoolOwner);
    }

}