using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class DragonController : BaseCharacterController
{

    [SerializeField]
    private BoxCollider _hitBox;
    [SerializeField]
    private BoxCollider _tailAttackCollider_1;
    [SerializeField]
    private BoxCollider _tailAttackCollider_2;
    

    private DragonAnimationManager  _animationManager;
    private DragonBehaviorManager   _behaivorManager;
    private FireBallSpawner         _fireBallSpawner;
    private WarningAreaManager      _waringAreaManager;
    private DragonEffectManager     _effecManager;
    private Animator                _animator;

    private DragonState _currentPhase = DragonState.Phase1;


    public event Action<DragonState> OnPhaseTransition;
    protected override void Awake()
    {
        base.Awake();
        if(_animationManager == null) _animationManager = GetComponent<DragonAnimationManager>();
        if (_behaivorManager == null) _behaivorManager = GetComponent<DragonBehaviorManager>();
        if(_fireBallSpawner == null) _fireBallSpawner = GetComponent<FireBallSpawner>();
        if (_waringAreaManager == null) _waringAreaManager = GetComponent<WarningAreaManager>();
        if(_effecManager == null) _effecManager = GetComponent<DragonEffectManager>();
        if(_animator == null) _animator = GetComponent<Animator>();


    }
    private void OnDestroy()
    {
        EventUnsubscribe();
    }
    private void OnDisable()
    {

    }
    private void Start()
    {

        EventSubScribe();

    }
    private void EventSubScribe()
    {
        _animationManager.OnRoarAnimEnd += _behaivorManager.PhaseTwoEnterComplete;
        _animationManager.OnAttackAnimEnd += _behaivorManager.SetAttackAnimationEnd;
        _animationManager.IsBackStepEnd += _behaivorManager.BackStepEnd;

        _animationManager.OnFireBreath += _fireBallSpawner.SpawnFireBreath;
        _animationManager.OnChargeFireBallAttack += _fireBallSpawner.SpawnChargeFireBall;
        _animationManager.OnFireBallAttack += _fireBallSpawner.SpawnFireBall;
        _animationManager.OnPlayFireEffect += _fireBallSpawner.SetFireEffectState;
        _animationManager.OnPlayFireChargeEffect += _effecManager.PlayerChargeEffect;
        _animationManager.RightArmHitBoxTrigger += SetFootAttackHitbox;
        _animationManager.TailAttackTrigger += SetTailAttackState;
        _animationManager.OnEarthShatter += _effecManager.PlayStampAttackParticle;
        _healthComponent.OnCharacterDeath += OnDeathEvent;
        _waringAreaManager.OnFillCompleted += _behaivorManager.PatternExecution;
       
        OnPhaseTransition += _behaivorManager.PhaseTwoEnter;

    }
 
    private void EventUnsubscribe()
    {
        _animationManager.OnRoarAnimEnd -= _behaivorManager.PhaseTwoEnterComplete;
        _animationManager.OnAttackAnimEnd -= _behaivorManager.SetAttackAnimationEnd;
        _animationManager.IsBackStepEnd -= _behaivorManager.BackStepEnd;

        _animationManager.OnFireBallAttack -= _fireBallSpawner.SpawnFireBall;
        _animationManager.OnFireBreath -= _fireBallSpawner.SpawnFireBreath;
        _animationManager.OnChargeFireBallAttack -= _fireBallSpawner.SpawnChargeFireBall;
        _animationManager.OnPlayFireEffect -= _fireBallSpawner.SetFireEffectState;
        
        _animationManager.OnPlayFireChargeEffect -= _effecManager.PlayerChargeEffect;
        _animationManager.RightArmHitBoxTrigger -= SetFootAttackHitbox;
        _animationManager.TailAttackTrigger -= SetTailAttackState;
        _animationManager.OnEarthShatter -= _effecManager.PlayStampAttackParticle;
        _healthComponent.OnCharacterDeath -= OnDeathEvent;
        _waringAreaManager.OnFillCompleted -= _behaivorManager.PatternExecution;

        OnPhaseTransition -= _behaivorManager.PhaseTwoEnter;
  
    }
    
    public override void TakeDamage(DamageInfo damageInfo)
    {
        base.TakeDamage(damageInfo);

        CheckPhaseTransition();


    }
    private void CheckPhaseTransition()
    {
        if (_healthComponent == null)
        {
            return;
        }

        float healthRatio = (float)_healthComponent.Health / _healthComponent.MaxHealth;
        if (healthRatio <= 0.5f && _currentPhase == DragonState.Phase1) // 체력 절반 이하라면 phase2로 변경
        {
            _currentPhase = DragonState.Phase2;
            OnPhaseTransition?.Invoke(DragonState.Phase2Enter);
        }
    }

    private void SetTailAttackState(bool state)
    {
        _tailAttackCollider_1.enabled = state;
        _tailAttackCollider_2.enabled = state;
    }

    private void SetFootAttackHitbox(bool state)
    {
        _hitBox.isTrigger = state;
    }

    public void CutsceneEndSignalReceiver()
    {
        _behaivorManager.OnBattle();
    }
    private void OnDeathEvent()
    {
        _behaivorManager.ChangeDeathState();
        _animator.SetTrigger("Death");
        GameManager.Instance.GameEnd(true);
    }
}
