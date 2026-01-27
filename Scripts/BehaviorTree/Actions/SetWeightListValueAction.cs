using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set WeightList Value", story: "Set [WeightList] Value Use [Distance]", category: "Action", id: "487bfa006ff22303f760730cd3d7328e")]
public partial class SetWeightListValueAction : Action
{
    [SerializeReference] public BlackboardVariable<List<int>> WeightList;
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<AttackWeightSet> weightSet;


    protected override Status OnStart()
    {
        if (Distance.Value < 7f)
        {
            WeightList.Value = weightSet.Value.nearWeights;
        }
        else
        {
            WeightList.Value = weightSet.Value.midWeights;
        }

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

