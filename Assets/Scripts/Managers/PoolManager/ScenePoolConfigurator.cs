using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePoolConfigurator : MonoBehaviour
{
    [SerializeField]
    private List<PoolManager.PoolStruct> m_PoolsPrefabs = new List<PoolManager.PoolStruct>();

    private void Awake()
    {
        PoolManager.Instance.InitalizePools(m_PoolsPrefabs);
        Destroy(gameObject);
    }
}
