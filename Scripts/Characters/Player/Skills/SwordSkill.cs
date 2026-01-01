using System.Collections;
using UnityEngine;
using UnityEngine.Pool;


public class SwordSkill : MonoBehaviour , IPoolable
{
    [SerializeField]
    private float _speed = 10.0f;
    [SerializeField]
    private Material _dissolveMat;
    [SerializeField]
    private float dissolveSpeed = 1f;
    [SerializeField]
    private SkillRangeAttack _rangeAttack;
  
    [SerializeField]
    private ParticleSystem _lightningParticle;



    private Renderer _rend;
    private bool _isMove = true;
    private bool _isDissolving = false;
    private float _dissolveAmount = 0f;

    private Material _dissolveInstance; 
    private Coroutine _dissolveCoroutine;
    public IObjectPool<GameObject> Pool { get; set; }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        Pool = pool;
    }
    public void OnSpawnFromPool()
    {
        _isMove = true;
        _isDissolving = false;
        _dissolveAmount = 0f;

        _rend.material = _dissolveMat;
     
        SoundManager.Instance.PlaySFX("SwordImpact");

        if (_dissolveCoroutine != null) StopCoroutine(_dissolveCoroutine);
    }
    public void OnReturnToPool()
    {
        StopAllCoroutines();

        _dissolveAmount = 0f;
        _isDissolving = false;
        _isMove = true;
        _lightningParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _dissolveInstance = Instantiate(_dissolveMat);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMove)
        {
            Move();
        }
    }
    private void Move()
    {
        float dist =  -1 * (_speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y + dist, transform.position.z);
    }
    private IEnumerator DissolveRoutine()
    {
        _isDissolving = true;
        Material mat = _dissolveInstance; // 인스턴스 1회 생성 후 캐싱

        while (_dissolveAmount < 1f)
        {
            _dissolveAmount += Time.deltaTime * dissolveSpeed;
            mat.SetFloat("_Dissolve", _dissolveAmount);
            yield return null;
        }
        OnReturnToPool();
        // 완전히 사라지면 오브젝트 제거
        Pool?.Release(gameObject);
    }
    public void OnHitGround()
    {
        _lightningParticle.Play();
        _rangeAttack.AreaAttack();


        SoundManager.Instance.PlaySFX("Thunder");
        if (_isDissolving) return;
        
        _isMove = false;
        _rend.material = _dissolveInstance;
        _dissolveAmount = 0f;
        _dissolveCoroutine = StartCoroutine(DissolveRoutine());
    }

}
