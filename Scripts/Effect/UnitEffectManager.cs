using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class UnitEffectManager : MonoBehaviour, IPoolable // 한 번만 재생되는 이펙트
{
    private Coroutine _returnCoroutine; // 플레이 코루틴

    private float _duration; // 이펙트 재생 시간
    private ParticleSystem[] _particleSystems;
    public IObjectPool<GameObject> Pool { get; set; }
    private void Awake()
    {
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
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }
    public void OnSpawnFromPool()
    {
        if (_returnCoroutine != null) StopCoroutine(_returnCoroutine);
        _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay(_duration));

        foreach (var ps in _particleSystems)
        {
            ps.Play();
        }
    }
    public void OnReturnToPool()
    {

        transform.SetParent(null);

        Pool?.Release(gameObject);
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 시간이 다 되면 정리 후 풀로 반환
        OnReturnToPool();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
