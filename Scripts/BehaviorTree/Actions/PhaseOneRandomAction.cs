using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PhaseOne Random", story: "[PhaseOne] WeightRandom", category: "Action", id: "c9a9917b8e30abd72b1ab3498fdd97b4")]
public partial class PhaseOneRandomAction : BaseWeightRandomAction
{
    [SerializeReference] public BlackboardVariable<PhaseOneType> PhaseOne;
    protected override Status OnStart()
    {
        
        PhaseOne.Value = (PhaseOneType)RollIndex();

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

