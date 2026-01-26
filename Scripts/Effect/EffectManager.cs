using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EffectManager : MonoBehaviour, IPoolable // 폭발 이펙트
{
    [SerializeField]
    private DamageInfo _damageInfo;
    [SerializeField]
    private float _explosionRadius = 5.0f;
    [SerializeField]
    private float _explosionForce = 10.0f;
    [SerializeField]
    private LayerMask targetMask;
    private bool _hasExploded = false;


    [SerializeField] private float _duration = 2f; // 이펙트 재생 시간
    private ParticleSystem[] _particleSystems;
    public IObjectPool<GameObject> Pool { get; set; }
    private void OnEnable()
    {
       
    }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
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
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSpawnFromPool()
    {
        foreach (var ps in _particleSystems)
        {
            ps.Play();
        }
        Explode();
        // 일정 시간 후 자동 반환
        Invoke(nameof(ReturnToPool), _duration);
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, targetMask);
        foreach(Collider hit in colliders)
        {
            if(hit.TryGetComponent<IDamageable>(out var target))
            {
                if(target != null)
                {
                    target.TakeDamage(_damageInfo);
                }
            }
        }
    }

    public void OnReturnToPool()
    {
        CancelInvoke();

        // 모든 파티클 정리
        foreach (var ps in _particleSystems)
        {
            ps.Stop();
            ps.Clear();
        }
    }

    private void ReturnToPool()
    {
        Pool?.Release(gameObject);
    }


}
