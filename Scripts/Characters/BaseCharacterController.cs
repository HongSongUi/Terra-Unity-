using UnityEngine;

public class BaseCharacterController : MonoBehaviour , IDamageable
{
    protected HealthComponent _healthComponent;

    protected virtual void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        if (_healthComponent == null)
        {
            Debug.Log("오류: HealthComponent를 찾을 수 없습니다! ' " +
                           gameObject.name + " ' 오브젝트에 HealthComponent가 붙어 있는지 확인하세요.", this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void TakeDamage(DamageInfo damageInfo)
    {
        float damage = damageInfo.DamageAmount;
        if(_healthComponent)
        {
            _healthComponent.ApplyDamage(damage);
        }
        else
        {
            Debug.Log("healthcomponent가 null");
        }
    }
}
