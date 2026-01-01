using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PhaseTwo WeightRandom", story: "[PhaseTwo] WeightRandom", category: "Action", id: "b8de436f982a41c5e58741520e787441")]
public partial class PhaseTwoWeightRandomAction : BaseWeightRandomAction
{
    [SerializeReference] public BlackboardVariable<PhaseTwo> PhaseTwo;

    protected override Status OnStart()
    {

        PhaseTwo.Value = (PhaseTwo)RollIndex();
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

