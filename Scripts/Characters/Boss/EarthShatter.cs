using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Pool;

public class EarthShatter : MonoBehaviour , IPoolable
{
    [SerializeField]
    private float _lifeTime = 4.75f;
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private DamageInfo _damageInfo;



    private float _stretchSpeed = 8f;
    private float _moveSpeed = 4f;

    private bool _hasHit = false;


   // private Coroutine _timerCoroutine;
    public IObjectPool<GameObject> Pool { get; set; }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }

    public void OnSpawnFromPool()
    {
        _boxCollider.size = new Vector3(5, 2, 0);
        _boxCollider.center = Vector3.zero;
        _boxCollider.enabled = true;
        _hasHit = false;
        StartCoroutine(LifeCycleRoutine());
    }

    public void OnReturnToPool()
    {
        StopAllCoroutines();
        _boxCollider.enabled = false;
        Pool?.Release(gameObject);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private IEnumerator LifeCycleRoutine()
    {
        float expansionTime = _lifeTime - 2.5f;
        float timer = 0f;

        while (timer < expansionTime)
        {
            UpdateBoxColliderSize();
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);
        OnReturnToPool();
    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
    }
    
    private void UpdateBoxColliderSize()
    {
        float _zSize = _boxCollider.size.z;
        _zSize += _stretchSpeed * Time.deltaTime;
        _boxCollider.size = new Vector3(5, 2, _zSize);

        float _zPos = _boxCollider.center.z;
        _zPos += _moveSpeed * Time.deltaTime;
        _boxCollider.center = new Vector3(0, 0, _zPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasHit) return;

        var damageable = other.GetComponent<IDamageable>();

        if(damageable != null&& other.CompareTag("Player"))
        {
            _hasHit = true; // 연속 히트 방지
            damageable.TakeDamage(_damageInfo); 
            Debug.Log("Attack on: " + other.name);
        }
    }
}
