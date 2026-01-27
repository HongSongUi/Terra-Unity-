using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField]
    PlayerAnimationEventHandler _animController;
    [SerializeField]
    private DamageInfo _damageInfo;



    private float _swordDamage = 10;

    BoxCollider _boxCollider;

    private void Awake()
    {
        _animController.OnSwordTrigger += SetSwordTrigger;
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void OnDestroy()
    {
        _animController.OnSwordTrigger -= SetSwordTrigger;
    }

    void Start()
    {
        
    }

    public void SetSwordDamage(float damage)
    {
        _swordDamage = damage;

    }
    // Update is called once per frame

    private void SetSwordTrigger(bool state)
    {
        _boxCollider.enabled = state;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag( "Player")) return;

        var damageable = other.GetComponentInParent<IDamageable>();
        if(damageable != null)
        {
            DamageInfo damageInfo = new DamageInfo(_swordDamage, false);
            damageable.TakeDamage(damageInfo);
        }
    }
   
}
