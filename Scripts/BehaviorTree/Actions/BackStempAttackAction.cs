using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BackStempAttack", story: "[Animator] BackStep Flow [State]", category: "Action", id: "2f4d5db630c71d73b7286ed124d30a27")]
public partial class BackStempAttackAction : Action 
{
    [SerializeReference] public BlackboardVariable<Animator> _Animator;
    [SerializeReference] public BlackboardVariable<bool> State;
    protected override Status OnStart()
    {
        _Animator.Value.SetBool("WarningAreaFill", true);
        _Animator.Value.SetBool("BackStepBreath", State.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
       
        return Status.Success;
    }

    protected override void OnEnd()
    {
        
    }
}

