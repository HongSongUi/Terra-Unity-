using Unity.Behavior;
using UnityEngine;

public class DragonBehaviorManager : MonoBehaviour
{
    private BehaviorGraphAgent _behaviorAgent;

    [SerializeField]
    private BoxCollider _tackleCollider;

    private void Awake()
    {
        if(_behaviorAgent == null)
        {
            _behaviorAgent = GetComponent<BehaviorGraphAgent>();
        }
    }
    private void OnDestroy()
    {
        
    }
    public void SetAttackAnimationEnd()
    {
        _behaviorAgent.SetVariableValue("IsAttackEnd", true);
        _tackleCollider.enabled = false;
    }
    public void PhaseTwoEnterComplete()
    {
        _behaviorAgent.SetVariableValue("CurrentState", DragonState.Phase2);
        _behaviorAgent.SetVariableValue("SetNewPattern", true);
    }
    public void BackStepEnd()
    {
        _behaviorAgent.SetVariableValue("IsBackStepEnd", true);
        _tackleCollider.enabled = true;
    }
    public void PhaseTwoEnter(DragonState state)
    {
        _behaviorAgent.SetVariableValue("CurrentState", state);

    }
    public void PatternExecution()
    {
        _behaviorAgent.SetVariableValue("isFillingEnd", true);
    }
    public void OnBattle()
    {
        _behaviorAgent.SetVariableValue("IsOnFight", true);
    }
    public void ChangeDeathState()
    {
        _behaviorAgent.SetVariableValue("CurrentState", DragonState.Death);
    }
}
