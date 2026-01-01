using UnityEngine;
using UnityEngine.Pool;

public class DragonFireBreath : MonoBehaviour , IPoolable
{
    public IObjectPool<GameObject> Pool { get; set; }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }
    public void OnSpawnFromPool()
    {

    }
    public void OnReturnToPool()
    {
        transform.SetParent(null);
        Pool?.Release(gameObject);
    }

}
