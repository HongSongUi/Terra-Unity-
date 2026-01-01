using System;
using System.Collections;
using UnityEngine;

public class CooldownComponent
{
    public PlayerSkillType Type { get; private set; }
    public float Duration { get; private set; }

    // 쿨타임이 끝나는 시점 (월드 시간 기준)
    private float _cooldownEndTime;

    // 현재 쿨타임 상태를 확인하는 프로퍼티
    public bool IsOnCooldown => Time.time < _cooldownEndTime;

    // 남은 쿨타임 시간
    public float RemainingTime => IsOnCooldown ? _cooldownEndTime - Time.time : 0f;

    public event Action<PlayerSkillType, float> OnStartCooldownEvent;
    public CooldownComponent(PlayerSkillType type,float duration)
    {
        Type = type;
        Duration = duration;
        _cooldownEndTime = 0f;
    }
    private void Update()
    {

    }

    public void StartCooldown()
    {
        // 현재 시간 + 쿨타임 시간 = 쿨타임 종료 시간
        _cooldownEndTime = Time.time + Duration;

        OnStartCooldownEvent?.Invoke(Type, Duration);
    }


}
