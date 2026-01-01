using UnityEngine;
using UnityEngine.Pool;

public class ExplosionEffect : MonoBehaviour , IPoolable // 폭발 이펙트
{

    [SerializeField]
    private DamageInfo _damageInfo;
    [SerializeField]
    private float _explosionRadius = 5.0f;
    [SerializeField]
    private float _explosionForce = 10.0f;
    [SerializeField]
    private LayerMask targetMask;


    private float _duration = 2f; // 이펙트 재생 시간
    private float _timer;
    private bool _hasExploded = false;
    private ParticleSystem[] _particleSystems;
    public IObjectPool<GameObject> Pool { get; set; }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }
    public void OnSpawnFromPool()
    {
        _timer = _duration;
        _hasExploded = false;
        foreach (var ps in _particleSystems)
        {
            ps.Play();
        }
        Explode();
    }
    public void OnReturnToPool()
    {

    }
    private void OnEnable()
    {
       
    }
    private void Awake()
    {
        // 자식 포함 모든 파티클 시스템 가져오기
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _duration = 0f;
        foreach (var ps in _particleSystems)
        {
            float psTime = ps.main.duration + ps.main.startLifetime.constantMax;
            if (psTime > _duration)
            {
                _duration = psTime;
            }
        }
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, targetMask);
        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent<IDamageable>(out var target))
            {
                if (target != null)
                {
                    target.TakeDamage(_damageInfo);
                }
            }
        }
    }

 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer <=0)
        {
            Pool?.Release(gameObject);
        }
    }
}
