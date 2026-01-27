using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class LoopingEffectManager : MonoBehaviour , IPoolable // loop 이펙트 
{
    private ParticleSystem[] _particleSystems;
    private Coroutine _returnCoroutine;
    private float _maxParticleLifetime = 0f;
    public IObjectPool<GameObject> Pool { get; set; }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }

    public void OnSpawnFromPool()
    {
        if (_returnCoroutine != null) StopCoroutine(_returnCoroutine);

        foreach (var ps in _particleSystems)
        {
            var mainModule = ps.main;
            mainModule.loop = true;

            ps.Play();
        }
    }

    public void OnReturnToPool()
    {

        transform.SetParent(null);

        StopAllCoroutines();

        foreach (var ps in _particleSystems)
        {
            var mainModule = ps.main;
            mainModule.loop = true; 
            ps.Clear(true);         
        }

        Pool?.Release(gameObject);
    }
    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in _particleSystems)
        {
            float lifetime = ps.main.startLifetime.constantMax;
            if (lifetime > _maxParticleLifetime)
            {
                _maxParticleLifetime = lifetime;
            }
        }
    }
    public void StopAndFadeOut()
    {
        if (_returnCoroutine != null) StopCoroutine(_returnCoroutine);
        foreach (var ps in _particleSystems)
        {
            var mainModule = ps.main;
            mainModule.loop = false;

        }

        _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay(_maxParticleLifetime));
    }
    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        // 시간이 다 되면 풀로 돌아감
        OnReturnToPool();
    }

}
