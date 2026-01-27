using UnityEngine;

public class MonsterAttackHandler : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private MonsterAnimationEventHandler _animationEventHandler;

    private void OnEnable()
    {
        _animationEventHandler.OnMonsterAttack += SetCollider;
    }
    private void OnDisable()
    {
        _animationEventHandler.OnMonsterAttack -= SetCollider;
    }

    private void SetCollider(bool state)
    {
        _boxCollider.enabled = state;
    }
}
