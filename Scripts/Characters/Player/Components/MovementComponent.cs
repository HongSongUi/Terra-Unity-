using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2.0f;
    [SerializeField]
    private float _sprintSpeed = 6.0f;
    [SerializeField]
    private GameObject _mainCamera;

    private Animator _animator;
    private CharacterController _characterController;

    [Range(0.0f, 0.3f)]
    private float _rotationSmoothTime = 0.12f;
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _speedChangeRate = 10.0f;
    private float _dodgeSpeed = 10.0f;

    private float _verticalVelocity;
    private bool _isDead = false;
    private bool _isDodge = false;

    private Vector2 _dashDir;
    private Coroutine _knockbackRoutine;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePlayerMovement(Vector2 moveVec, bool sprintState)
    {
        if (_isDodge)
        {
            PlayerDash();
        }
        else
        {
            PlayerMove(moveVec, sprintState);
        }
        
    }
    public void HandlePlayerDash()
    {
      
    }
    private void PlayerDash()
    {
        Vector3 dir = new Vector3(_dashDir.x, 0, _dashDir.y).normalized;
        _targetRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            _rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _characterController.Move(targetDirection.normalized * (_dodgeSpeed * Time.deltaTime)
             + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }
    private void PlayerMove(Vector2 moveVec, bool sprintState)
    {
        float targetSpeed = sprintState ? _sprintSpeed : _moveSpeed;
        Vector3 dir = new Vector3(moveVec.x, 0, moveVec.y).normalized;

        if (moveVec.magnitude < 0.1f)
        {
            targetSpeed = 0.0f;
        }


        float inputMagnitude = 1f;

        _speed = Mathf.Lerp(_speed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);
        _speed = Mathf.Round(_speed * 1000f) / 1000f;


        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        if (moveVec != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;


        _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime)
            + new Vector3(0.0f, _rotationSmoothTime, 0.0f) * Time.deltaTime);


        // move the player

        _animator.SetFloat("Speed", _animationBlend);
    }
    public void RotationToCameraDirection()
    {
        if (_isDead) return;

        float cameraYAngle = _mainCamera.transform.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0.0f, cameraYAngle, 0.0f);
    }
    public void SetDodgeAnimation(Vector2 inputVec)
    {
        if (inputVec == Vector2.zero) return;
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge") && !_animator.GetNextAnimatorStateInfo(0).IsName("Dodge"))
        { // 구르기 계속 누르기 방지
            _dashDir = inputVec;
            _animator.SetTrigger("Dodge");
        }
    }
    public void StartKnockback(Vector3 direction, float force, float duration = 0.5f)
    {
        if (_knockbackRoutine != null)
        {
            StopCoroutine(_knockbackRoutine);
        }

        // 넉백 코루틴 시작
        _knockbackRoutine = StartCoroutine(KnockbackRoutine(direction.normalized, force, duration));
    }
    private IEnumerator KnockbackRoutine(Vector3 direction, float initialForce, float duration)
    {
        float startTime = Time.time;
        float currentForce = initialForce;

        // 넉백이 진행되는 동안 반복
        while (Time.time < startTime + duration)
        {
            //  시간에 따른 감속 (Smooth Damping)
            float t = (Time.time - startTime) / duration; // 0에서 1까지 증가
            currentForce = Mathf.Lerp(initialForce, 0f, t);


            Vector3 horizontalMove = direction * currentForce;
            Vector3 finalMove = horizontalMove + new Vector3(0, _verticalVelocity, 0);

            _characterController.Move(finalMove * Time.deltaTime);

            yield return null; // 다음 프레임까지 대기
        }

        _knockbackRoutine = null;
    }
    public void SetPlayerDodgeState(bool state)
    {
        _isDodge = state;
    }
    public void PlayerTeleport(Vector3 pos)
    {
        _characterController.enabled = false;

        _characterController.transform.position = pos;
        _characterController.enabled = true;
    }
    public void SetDeadState()
    {
        _isDead = true;
    }
}
