using UnityEngine;

public class BaseCharacterController : MonoBehaviour , IDamageable // 모든 Character들이 상속받아서 사용
{
    protected HealthComponent _healthComponent;

    protected virtual void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        if (_healthComponent == null)
        {
            Debug.Log("HealthComponent를 찾을 수 없습니다! ' " +
                           gameObject.name + " ' 오브젝트에 HealthComponent가 붙어 있는지 확인하세요.", this);
        }
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
