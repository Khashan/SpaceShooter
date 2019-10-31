using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolStruct
{
    public GameObject m_Prefab;
    public int m_Size;
    [Tooltip("If the the pool already used all its GameObject, Re-Use the first active one")]
    public bool m_LimitReachUseActiveObject;
}

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    private List<PoolStruct> m_Pools = new List<PoolStruct>();

    private Dictionary<GameObject, List<GameObject>> m_PoolsObjects = new Dictionary<GameObject, List<GameObject>>();
    private Dictionary<GameObject, List<GameObject>> m_PoolsActiveObjects = new Dictionary<GameObject, List<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
        CreatePools();
    }

    //Create Pools
    private void CreatePools()
    {
        for (int i = 0; i < m_Pools.Count; i++)
        {
            PoolStruct pool = m_Pools[i];
            if (IsValidPoolStruct(pool))
            {
                m_PoolsObjects.Add(pool.m_Prefab, new List<GameObject>());
                m_PoolsActiveObjects.Add(pool.m_Prefab, new List<GameObject>());
                GrowPool(m_Pools[i]);
            }
        }
    }

    //Verify if the struct does have a prefab otherwise, it's a invalid struct
    private bool IsValidPoolStruct(PoolStruct a_Pool)
    {
        return a_Pool.m_Prefab != null;
    }

    //Create Pool's Objects
    private void GrowPool(PoolStruct a_Pool)
    {
        for (int i = 0; i < a_Pool.m_Size; i++)
        {
            CreatePooledObject(a_Pool.m_Prefab);
        }
    }

    //Create Object for the pool and config
    private void CreatePooledObject(GameObject a_PoolPrefab)
    {
        GameObject pooledGameObject = Instantiate(a_PoolPrefab, transform);
        PooledObject pooledScript = pooledGameObject.GetComponent<PooledObject>();

        if (pooledScript == null)
        {
            Debug.LogErrorFormat("Invalid Prefab for PoolManager: The prefab {0} should be derived from PooledObject!", a_PoolPrefab.name);
            Destroy(pooledGameObject);
        }
        else
        {
            pooledScript.InitPooledObject(a_PoolPrefab);
            pooledGameObject.SetActive(false);

            m_PoolsObjects[a_PoolPrefab].Add(pooledGameObject);
        }
    }

    public T UseObjectFromPool<T>(GameObject a_Prefab, Vector3 a_Position, Quaternion a_Rotation)
    {
        GameObject pooledObject = UseObjectFromPool(a_Prefab, a_Position, a_Rotation);
        return pooledObject.GetComponent<T>();
    }

    public GameObject UseObjectFromPool(GameObject a_Prefab, Vector3 a_Position, Quaternion a_Rotation)
    {
        GameObject pooledObject = GetObjectFromPool(a_Prefab);

        if (pooledObject != null)
        {
            pooledObject.transform.position = a_Position;
            pooledObject.transform.rotation = a_Rotation;
            pooledObject.SetActive(true);

            m_PoolsActiveObjects[a_Prefab].Add(pooledObject);
        }

        return pooledObject;
    }

    //Get the first inactive poolObject found
    private GameObject GetObjectFromPool(GameObject a_Prefab)
    {
        List<GameObject> pooledObject = new List<GameObject>();
        m_PoolsObjects.TryGetValue(a_Prefab, out pooledObject);

        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            }
        }

        return NotEnoughPooledObject(a_Prefab);
    }

    //If there's no inactive poolObject -> Catch Action : Get oldest active pooledObject or DynamicSafeGrowPool 
    private GameObject NotEnoughPooledObject(GameObject a_Prefab)
    {
        PoolStruct pool = GetPoolStruct(a_Prefab);

        if (IsValidPoolStruct(pool))
        {
            if (pool.m_LimitReachUseActiveObject)
            {
                return UseFirstActivePooledObject(pool);
            }
            else
            {
                return DynamicGrowPool(pool);
            }
        }

        return null;
    }

    //Get a Pool with a prefab
    private PoolStruct GetPoolStruct(GameObject a_PoolPrefab)
    {
        for (int i = 0; i < m_Pools.Count; i++)
        {
            if (m_Pools[i].m_Prefab.Equals(a_PoolPrefab))
            {
                return m_Pools[i];
            }
        }

        return new PoolStruct() { m_Prefab = null };
    }

    //Get the oldest pooledobject to re-use it for a new one
    private GameObject UseFirstActivePooledObject(PoolStruct a_Pool)
    {
        List<GameObject> activeObjects = new List<GameObject>();
        m_PoolsActiveObjects.TryGetValue(a_Pool.m_Prefab, out activeObjects);

        if (activeObjects.Count != 0)
        {
            GameObject firstElement = activeObjects[0];
            activeObjects.RemoveAt(0);
            return firstElement;
        }

        return null;
    }

    //create other poolobject if the pool doesn't have any free ones. 
    //This is just for safety of the runtime, but will require to change it in editor mode
    private GameObject DynamicGrowPool(PoolStruct a_Pool)
    {
        Debug.LogErrorFormat("Pool Too Small: The Pool {0} is not big enough, Creating {1} more PooledObject! Increments the pool size!", a_Pool.m_Prefab.name, a_Pool.m_Size);

        GrowPool(a_Pool);
        return GetObjectFromPool(a_Pool.m_Prefab);
    }

    //Manually return a PooledObject.
    public void ReturnedPooledObject(PooledObject a_PooledObject, GameObject a_PoolOwner)
    {
        PoolStruct pool = GetPoolStruct(a_PooledObject.gameObject);

        if (m_PoolsActiveObjects.ContainsKey(a_PooledObject.PoolOwner))
        {
            List<GameObject> activeObjects = m_PoolsActiveObjects[a_PoolOwner];

            if (activeObjects.Count != 0)
            {
                activeObjects.Remove(a_PooledObject.gameObject);
            }
        }
    }
}