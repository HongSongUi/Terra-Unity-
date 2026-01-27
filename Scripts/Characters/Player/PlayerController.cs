using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class PlayerController : BaseCharacterController
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _effectPoint;
    [SerializeField]
    private Transform _jumpEffectPoint;
    private bool _isOnTeleport;

    private Vector3 _spawnPoint;

    private PlayerInputManager _inputManager;

    private PlayerAnimationEventHandler _animController;

    private MovementComponent   _movementComponent;
    private CombatComponent _combatComponent;
    private PlayerInventoryComponent _inventoryComponent;
    private SkillManager _skillManager;
    private ParticleAnimationEvent _particleController;




    private Dictionary<PlayerSkillType, CooldownComponent> _cooldowns;

    protected override void Awake()
    {
        base.Awake();
        _movementComponent = GetComponent<MovementComponent>();
        _combatComponent = GetComponent<CombatComponent>();
        _inputManager = GetComponent<PlayerInputManager>();
        _animController = GetComponent<PlayerAnimationEventHandler>();
        _inventoryComponent = GetComponent<PlayerInventoryComponent>();
        _skillManager = GetComponent<SkillManager>();
        _particleController = GetComponent<ParticleAnimationEvent>();

        _combatComponent.SetMovementComponent(_movementComponent);
        _inventoryComponent.SetComponent(_healthComponent, _combatComponent);

        _particleController.SetEffectPoint(_effectPoint, _jumpEffectPoint);

        
    }

    void Start()
    {
        EventSubScribe();
        InitSkillCoolDown();

        UIManager.Instance.FillPlayerHealth(_healthComponent.HealthPercentage);

      
        foreach (var data in _cooldowns)
        {
            UIManager.Instance.SubscribeSkill(data.Value);
        }

        UIManager.Instance.SubscribePotion(_inventoryComponent);
       
    }
    private void OnDestroy()
    {

        EventUnSubScribe();
    }
    // Update is called once per frame
    void Update()
    {
        if(!_healthComponent.IsDead)
        {
            if (!_combatComponent.BlockMovementState())
            {
                _movementComponent.HandlePlayerMovement(_inputManager.Move, _inputManager.IsSprint);
            }
            _combatComponent.HandleBufferedInput();
        }
    }
    private void FixedUpdate()
    {

    }
    private void InitSkillCoolDown()
    {
        _cooldowns = new Dictionary<PlayerSkillType, CooldownComponent>()
        {
            { PlayerSkillType.Dash, new CooldownComponent( PlayerSkillType.Dash, 3.0f) },
            { PlayerSkillType.Skill_One, new CooldownComponent(PlayerSkillType.Skill_One, 8.0f) },
            { PlayerSkillType.Skill_Two, new CooldownComponent(PlayerSkillType.Skill_Two, 10.0f) }
        };
    }
    private void EventSubScribe()
    {
        _healthComponent.OnHealthWarning += PlayWarningAni;
        _animController.OnPlayerDodge += _movementComponent.SetPlayerDodgeState;

        _inputManager.OnPlayerAttack += _combatComponent.NormalAttack;
        _inputManager.OnPlayerSkill_1 += TryUseSkillOne;
        _inputManager.OnPlayerSkill_2 += TryUseSkillTwo;
        _inputManager.OnPlayerTeleport += TryTeleport;

        _animController.OnPlayerAttack += _combatComponent.SetPlayerAttackState;
        _animController.OnPlayerSkill += _combatComponent.SetSkillState;
        _animController.OnPlayerGuardBreak += _combatComponent.SetGuardState;
        _animController.OnPlayerHitAnimation += _combatComponent.OnPlayerHit;
        _animController.CreateSword += _skillManager.CreateSword;
        _animController.CreateSlash += _skillManager.CreateSlashSkill;
        _animController.OnStartSwordSkill += _particleController.PlayGreatSwordEffect;
        _animController.OnStartSlashSkill += _particleController.PlaySkillEffect;

        _inputManager.OnPlayerRolling += TryUseDash;
        _inputManager.OnPlayerGuard += _combatComponent.SetGuardState;
        _inputManager.OnPlayerHealing += UseHealingPotion;
        _inputManager.OnPlayerPowerUp += UsePowerUpPotion;

        _healthComponent.OnCharacterDeath += DeathEvent;
    }

    private void EventUnSubScribe()
    {
        _healthComponent.OnHealthWarning -= PlayWarningAni;
        _healthComponent.OnCharacterDeath -= DeathEvent;
        _inputManager.OnPlayerAttack -= _combatComponent.NormalAttack;
        _inputManager.OnPlayerRolling -= TryUseDash;

        _inputManager.OnPlayerSkill_1 -= TryUseSkillOne;
        _inputManager.OnPlayerSkill_2 -= TryUseSkillTwo;
        _inputManager.OnPlayerGuard -= _combatComponent.SetGuardState;
        _inputManager.OnPlayerTeleport -= TryTeleport;
        _inputManager.OnPlayerHealing -= UseHealingPotion;
        _inputManager.OnPlayerPowerUp -= UsePowerUpPotion;

        _animController.OnPlayerAttack -= _combatComponent.SetPlayerAttackState;
        _animController.OnPlayerDodge -= _movementComponent.SetPlayerDodgeState;
        _animController.OnPlayerSkill -= _combatComponent.SetSkillState;
        _animController.OnPlayerHitAnimation -= _combatComponent.OnPlayerHit;
        _animController.OnPlayerGuardBreak -= _combatComponent.SetGuardState;
        _animController.OnStartSwordSkill -= _particleController.PlayGreatSwordEffect;
        _animController.OnStartSlashSkill -= _particleController.PlaySkillEffect;

        _animController.CreateSword -= _skillManager.CreateSword;
        _animController.CreateSlash -= _skillManager.CreateSlashSkill;
    }
    public override void TakeDamage(DamageInfo damageInfo)
    {
        _combatComponent.HandleIncomingDamage(damageInfo, _healthComponent);
    }

    private void SetRollingAnimation() // È¸ÇÇ±â
    {
        if (_combatComponent.IsSkilled) return;
        if (_combatComponent.IsAttack) _combatComponent.NormalAttackEnd();
        _movementComponent.SetDodgeAnimation(_inputManager.Move);
    }

    private bool CheckSkillCooldown(CooldownComponent cooldown)
    {
        if (cooldown == null)
        {
            return false;
        }
        if (cooldown.IsOnCooldown)
        {
            return false; 
        }
        return true;
    }
    private void TryUseDash()
    {
        CooldownComponent dashCoolDown = _cooldowns[PlayerSkillType.Dash];
        if(CheckSkillCooldown(dashCoolDown) && !_combatComponent.BlockDash())
        {
            if (_inputManager.Move == Vector2.zero) return;
            SetRollingAnimation();
            dashCoolDown.StartCooldown();
        }
      
    }

    private void TryUseSkillOne()
    {
        CooldownComponent skillCoolDown = _cooldowns[PlayerSkillType.Skill_One];
        if (CheckSkillCooldown(skillCoolDown) && !_combatComponent.HitState && !_combatComponent.IsSkilled)
        {
            _combatComponent.OnGreateSwordSkill();
            skillCoolDown.StartCooldown();
            
        }
    }
    
    private void TryUseSkillTwo()
    {
        CooldownComponent skillCoolDown = _cooldowns[PlayerSkillType.Skill_Two];
        if (CheckSkillCooldown(skillCoolDown) && !_combatComponent.HitState && !_combatComponent.IsSkilled)
        {
            _combatComponent.OnSwordWave();
            skillCoolDown.StartCooldown();
        }
    }
    private void TryTeleport()
    {
        if(_isOnTeleport)
        {
            _movementComponent.PlayerTeleport(_spawnPoint);
            UIManager.Instance.DisableNoticeText();
            UIManager.Instance.ShowGameText();
            _isOnTeleport = false;
        }
    }
    public void SetPlayerTP(Vector3 pos, bool state)
    {
        _spawnPoint = pos;
        _isOnTeleport = state;
    }
    public void DiablePlayerInput()
    {
        _inputManager.DisablePlayerInput();
    }
    public void EnablePlayerInput()
    {
        _inputManager.EnablePlayerInput();
    }
    private void UseHealingPotion()
    {
        _inventoryComponent.UseHealingPotion(_effectPoint);
    }
    private void UsePowerUpPotion()
    {
       
        _inventoryComponent.UseAttackBuffPotion(_effectPoint);
    }
    private void DeathEvent()
    {
        _combatComponent.SetDeadState();
        _movementComponent.SetDeadState();

        
        _animator.SetTrigger("Death");
        GameManager.Instance.GameEnd(false);
    }
    private void PlayWarningAni(bool state)
    {
        UIManager.Instance.PlayWarningAnimation(state);
    }
}
