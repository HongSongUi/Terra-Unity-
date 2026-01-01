using UnityEngine;

public class SkillRangeAttack : MonoBehaviour
{
    [SerializeField]
    private float _checkRadius = 5f;
    [SerializeField]
    private LayerMask _targetLayer;
    [SerializeField]
    private DamageInfo _damageInfo;
    private Collider[] _hitColliders = new Collider[10];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AreaAttack()
    {
        int numHits = Physics.OverlapSphereNonAlloc(
            transform.position,
            _checkRadius,
            _hitColliders,
            _targetLayer
        );
        Debug.Log( numHits );
        for (int i = 0; i < numHits; i++)
        {
            Collider targetCollider = _hitColliders[i];
            if ( targetCollider != null ) 
            {
                var damageable = targetCollider.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage( _damageInfo );
               
            }
           
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
