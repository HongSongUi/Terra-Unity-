using UnityEngine;
using UnityEngine.Pool;


public class ProjectileBase : MonoBehaviour , IPoolable // 드래곤의 fireball base
{
    [SerializeField]
    private PoolType _explosionEffectType;
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private DamageInfo _damageInfo;

    private string _explosionSound;
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

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_explosionEffectType == PoolType.SmallExplosion)
        {
            _explosionSound = "SmallExplosion";
        }
        else if(_explosionEffectType == PoolType.BigExplosion)
        {
            _explosionSound = "BigExplosion";
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    protected virtual void Movement()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null && !other.CompareTag("Boss"))
        {   
            damageable.TakeDamage(_damageInfo);
        }

        if (other.CompareTag("Ground"))
        {
            OnHitGround();
        }
    }
    protected virtual void OnHitGround()
    {

        GameObject explosion = ObjectPoolManager.Instance.Get(_explosionEffectType);
        SoundManager.Instance.PlaySFX(_explosionSound);
        if (explosion != null)
        {

            ExplosionEffect effectComponent = explosion.GetComponent<ExplosionEffect>();

            if (effectComponent != null)
            {
                
                var explosionPool = ObjectPoolManager.Instance.GetPool(_explosionEffectType);
                effectComponent.SetPool(explosionPool);


                explosion.transform.position = transform.position;
                explosion.transform.rotation = Quaternion.identity;

                effectComponent.OnSpawnFromPool();
            }
        }

        // 풀로 반환
        Pool?.Release(gameObject);
    }
}
