using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WariningArea", story: "[CurrentAttack] Fill [WariningAreaMananger]", category: "Action", id: "aa29c2d7f6dcec73f62f378474f8bb07")]
public partial class WariningAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<PhaseTwo> CurrentAttack;
    [SerializeReference] public BlackboardVariable<WarningAreaManager> WariningAreaMananger;
    protected override Status OnStart()
    {
        WariningAreaMananger.Value.CallFilling(CurrentAttack);
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

