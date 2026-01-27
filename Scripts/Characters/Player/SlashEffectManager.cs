using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SlashEffectManager : MonoBehaviour, IPoolable
{

    private float _duration = 2f; // 이펙트 재생 시간
    private ParticleSystem[] _particleSystems;
    private Coroutine _returnCoroutine; // 플레이 코루틴
    public IObjectPool<GameObject> Pool { get; set; }
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
        
        Pool?.Release(gameObject);
    }
    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _duration = 0f;
        foreach (var ps in _particleSystems) // 가장 긴 재생시간을 가져와서 저장
        {
            float psTime = ps.main.duration + ps.main.startLifetime.constantMax;
            if (psTime > _duration)
            {
                _duration = psTime; 
            }
        }
    }
    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 시간이 다 되면 정리 후 풀로 반환
        OnReturnToPool();

    }

}
