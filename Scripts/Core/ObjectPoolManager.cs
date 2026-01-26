using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public enum PoolType
{
    FireBall,
    ChargeFireBall,
    SmallExplosion,
    BigExplosion,
    SlashEffect,
    SlashFireEffect,
    FireBreath,
    SlashProjectile,
    GreatSwordSkill,
    HealingEffect,
    BuffEffect,
    SkillSlashEffect,
    JumpEffect,
    WindEffect,
    EarthShatter,
    BreathChargeEffect,
    IgniteEffect,
    // 여기에 추가하면 됨
}

[System.Serializable]
public class PoolConfig
{
    public PoolType poolType;
    public GameObject prefab;
    public int defaultCapacity = 10;
    public int maxPoolSize = 15;
}

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;

    [SerializeField]
    private List<PoolConfig> _poolConfigs = new List<PoolConfig>();

    private Dictionary<PoolType, IObjectPool<GameObject>> _pools = new Dictionary<PoolType, IObjectPool<GameObject>>();

    public static ObjectPoolManager Instance
    {
        get { return _instance; }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        InitPools();
    }

    private void InitPools()
    {
        foreach (var config in _poolConfigs)
        {
            var pool = new ObjectPool<GameObject>(
                () => CreatePooledItem(config.prefab),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true,
                config.defaultCapacity,
                config.maxPoolSize
            );

            _pools.Add(config.poolType, pool);

            // 미리 오브젝트 생성
            for (int i = 0; i < config.defaultCapacity; i++)
            {
                GameObject obj = CreatePooledItem(config.prefab);
                var poolable = obj.GetComponent<IPoolable>();
                if (poolable != null)
                {
                    poolable.SetPool(pool);
                }
                pool.Release(obj);
            }
        }
    }

    private GameObject CreatePooledItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    // Public API
    public GameObject Get(PoolType poolType)
    {
        if (_pools.TryGetValue(poolType, out var pool))
        {
            GameObject obj = pool.Get();
            var poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.SetPool(pool);
            }
            return obj;
        }

        Debug.LogError($"Pool '{poolType}' not found!");
        return null;
    }

    public void Release(PoolType poolType, GameObject obj)
    {
        if (_pools.TryGetValue(poolType, out var pool))
        {
            pool.Release(obj);
        }
        else
        {
            Debug.LogError($"Pool '{poolType}' not found!");
        }
    }

    public IObjectPool<GameObject> GetPool(PoolType poolType)
    {
        if (_pools.TryGetValue(poolType, out var pool))
        {
            return pool;
        }

        Debug.LogError($"Pool '{poolType}' not found!");
        return null;
    }
}
