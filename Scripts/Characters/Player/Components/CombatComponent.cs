using System;
using System.Collections;
using Unity.AppUI.Core;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    [SerializeField]
    private int _maxComboCount = 4;
    [SerializeField]
    private float inputBufferTime = 0.5f;
    [SerializeField]
    private PoolType _slashEffect;
    [SerializeField]
    private PoolType _slashFireEffect;
    [SerializeField]
    private Transform _effectPoint;
    [SerializeField]
    private SwordAttack _sword;
    [SerializeField]
    private float _baseDamage;


    private Animator _animator;



    private MovementComponent _movementComponent;
   


    private bool _isAttack = false;
    private bool _isSkilled = false;
    private bool _guardState = false;


    private bool _comboState = false;
    private bool _nextAttackQueued = false;
    private bool _hitState = false;
    private bool _isDead = false;


    private float _lastInputTime;
    private int _currentComboCount = 0;

    private float _currentDamageMultiplier = 1f; 
    private Coroutine _buffCoroutine;

    public bool IsAttack => _isAttack;
    public bool IsSkilled => _isSkilled;
    public bool GuardState => _guardState;
    public bool HitState => _hitState;
    public float FinalDamage => _baseDamage * _currentDamageMultiplier;
   
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if(_sword != null)
        {
            _sword.SetSwordDamage(FinalDamage);
        }

    
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMovementComponent(MovementComponent component)
    {
        _movementComponent = component;
    }
    public void HandleBufferedInput()
    {
        if (_nextAttackQueued && _comboState && _currentComboCount < _maxComboCount)
        {
            // 버퍼 시간 내에 있는지 확인
            if (Time.time - _lastInputTime <= inputBufferTime)
            {

                ExecuteNextCombo();
            }
            else
            {
                // 시간 초과로 버퍼 클리어
                _nextAttackQueued = false;
            }
        }

    }
    public void NormalAttack()
    {
        if (_isDead) return;
      
        _movementComponent.RotationToCameraDirection();
        if (!_isAttack)
        {
            StartFirstAttack();
        }
        else
        {
            QueueNextAttack();
        }
    }
    private void StartFirstAttack()
    {
        _isAttack = true;
        _currentComboCount = 1;
        _nextAttackQueued = false; // 버퍼 초기화
        _animator.SetTrigger("Attack");
    }
    private void QueueNextAttack()
    {
        if (_currentComboCount >= _maxComboCount)
        {
            return;
        }
        _nextAttackQueued = true;
        _lastInputTime = Time.time;
    }

    private void ExecuteNextCombo()
    {
        _currentComboCount++;
        _comboState = false; // 콤보 윈도우 닫기
        _nextAttackQueued = false; // 버퍼 사용됨

        switch (_currentComboCount)
        {
            case 2:
                _animator.SetTrigger("Attack2Trigger");
                break;

            case 3:
                _animator.SetTrigger("Attack3Trigger");
                break;

            case 4:
                _animator.SetTrigger("Attack4Trigger");
                break;

            default:
                Debug.LogWarning($"예상치 못한 콤보 카운트: {_currentComboCount}");
                break;
        }
    }
    public void SetDeadState()
    {
        _isDead = true;
    }
    public void SetGuardState(bool state)
    {
        if (_isAttack) NormalAttackEnd();
          
        _animator.SetBool("OnGuard", state);

        _guardState = state;
    }
    private void SetHitAnimation(bool isKnockdown)
    {
        if (!isKnockdown)
        {
            if (!_isAttack)
            {
                _animator.SetTrigger("HitTrigger");
                NormalAttackEnd();
            }
        }
        else if (isKnockdown)
        {
            NormalAttackEnd();
            if (!_guardState)
            {
                _animator.SetTrigger("DropTrigger");
                Vector3 dir = transform.forward * -1;
                _movementComponent.StartKnockback(dir, 15, 1);
            }
            else if (_guardState)
            {
                _animator.SetTrigger("GuardBreak");
            }

        }
    }
    public void HandleIncomingDamage(DamageInfo damageInfo, HealthComponent healthComponent)
    {
      
        if (_isSkilled)
        {
            return;
        }
        float dmg = damageInfo.DamageAmount;
        if (_guardState)
        {
            dmg *= 0.9f;
        }
        if (!healthComponent.ApplyDamage(dmg))
        {
            return;
        }
        if (!damageInfo.IsKnockbackAttack)
        {
            CameraManager.Instance.ShakeCameraOnHit(1, 0.7f);
        }
        else
        {
            CameraManager.Instance.ShakeCameraOnExplosion(1, 0.7f);
        }
        SetHitAnimation(damageInfo.IsKnockbackAttack);
        SoundManager.Instance.PlaySFX("Hit");
        UIManager.Instance.PlayHitImageAnimation();
    }
    public bool BlockMovementState()
    {
        return _isAttack || _guardState || _isSkilled || _hitState;
    }
    public bool BlockDash()
    {
        return _guardState || _isSkilled || _hitState; 
    }
    public bool IsHitState()
    {
        return _hitState;
    }
    public void SetPlayerAttackState(bool state) // 애니메이션 컨트롤러 구독
    {
        _isAttack = state; // true -> 이동 제한
    }
    public void EnableComboWindow()
    {
        _comboState = true;
    }

    public void DisableComboWindow()
    {
        _comboState = false;
    }
    public void NormalAttackEnd()
    {
        _isAttack = false;
        _comboState = false;
        _nextAttackQueued = false;
        _currentComboCount = 0;
    }

    public void SetSkillState(bool state)
    {
        _isSkilled = state;
    }

    public void OnGreateSwordSkill()
    {
        if (_isSkilled) return;
        _movementComponent.RotationToCameraDirection();
        _animator.SetTrigger("Skill01");
    }
    public void OnSwordWave()
    {
        if (_isSkilled) return;
        _movementComponent.RotationToCameraDirection();
        _animator.SetTrigger("Skill02");
    }
    public void OnPlayerHit(bool state)
    {
        _hitState = state;
    }
    public void SwordSlashEffect()
    {
        GameObject effect = ObjectPoolManager.Instance.Get(_slashEffect);
        SpawnEffect(effect);
    }
    public void SkillSlashEffect()
    {
        GameObject effect = ObjectPoolManager.Instance.Get(_slashFireEffect);
        SpawnEffect(effect);
    }
    private void SpawnEffect(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.position = _effectPoint.position;
            obj.transform.rotation = _effectPoint.rotation;
            obj.transform.Rotate(Vector3.up, 180f, Space.Self);
            obj.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();
        }
    }
    public void ApplyAttackBuff(float buffAmount, float duration)
    {
        if (_buffCoroutine != null)
        {
            StopCoroutine(_buffCoroutine);
        }

        _buffCoroutine = StartCoroutine(AttackBuffRoutine(buffAmount, duration));
    }
    private IEnumerator AttackBuffRoutine(float buffAmount, float duration)
    {
        // 버프 적용 
        _currentDamageMultiplier += buffAmount;
        _sword.SetSwordDamage(FinalDamage);


        //지정된 시간만큼 대기
        yield return new WaitForSeconds(duration);

        // 버프 해제
        _currentDamageMultiplier -= buffAmount;
        _sword.SetSwordDamage(FinalDamage);

        _buffCoroutine = null;
    }
}
