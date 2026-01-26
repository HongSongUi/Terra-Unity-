using MaykerStudio.VFX;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SlashSkill : MonoBehaviour , IPoolable
{
    [SerializeField]
    private float _lifeCycle = 2.5f;


    [SerializeField]
    private DamageInfo _damageInfo;



    private Coroutine _returnCoroutine;
    private float _speed = 15.0f;
    public IObjectPool<GameObject> Pool { get; set; }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }

    public void OnSpawnFromPool()
    {
        if (_returnCoroutine != null) StopCoroutine(_returnCoroutine); // 안전장치
        _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay(_lifeCycle));
    }
    public void OnReturnToPool()
    {
        StopAllCoroutines();


        Pool?.Release(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponentInParent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(_damageInfo);
        }
    }
    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 시간이 다 되면 정리 후 풀로 반환
        OnReturnToPool();
    }
    private void Move()
    {
        Vector3 dir = transform.forward * _speed;
        transform.position += dir * Time.deltaTime;
    }
}
