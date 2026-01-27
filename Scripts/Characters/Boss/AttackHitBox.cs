using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField]
    private DamageInfo _damageInfo;
    
    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(_damageInfo);
        }
    }
}
