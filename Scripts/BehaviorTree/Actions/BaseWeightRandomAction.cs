using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BaseWeightRandom", story: "[WeightList]", category: "Action", id: "a8eaf92151ff46105859b52e8a10389b")]
public partial class BaseWeightRandomAction : Action
{
    [SerializeReference] public BlackboardVariable<List<int>> WeightList;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }
    protected int RollIndex()
    {
        int total = WeightList.Value.Sum();
        int roll = UnityEngine.Random.Range(0, total);
        int acc = 0;
        for (int i = 0; i < WeightList.Value.Count; i++)
        {
            acc += WeightList.Value[i];
            if (roll < acc)
            {
                return i;
            }
        }
        return 0;
    }
}

