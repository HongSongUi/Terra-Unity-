using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StartRoar", story: "[Animator] Start Roar Animation", category: "Action", id: "2435b463b786c966053473861052c5aa")]
public partial class StartRoarAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    protected override Status OnStart()
    {
        if(!Animator.Value.GetCurrentAnimatorStateInfo(0).IsName("Roar")&& !Animator.Value.GetNextAnimatorStateInfo(0).IsName("Roar"))
        {// 애니메이션 중복 실행 방지
            Animator.Value.SetTrigger("Roar");
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

