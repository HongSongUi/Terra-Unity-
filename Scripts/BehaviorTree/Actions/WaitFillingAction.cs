using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitFilling", story: "Wait [State] is true", category: "Action", id: "15422c9a1e9258331f7a519d6c119539")]
public partial class WaitFillingAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> State;

    protected override Status OnStart()
    {
        if(State)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (State)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

