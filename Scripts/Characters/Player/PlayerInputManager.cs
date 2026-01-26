using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    private Vector2 _move;
    private Vector2 _look;
    private bool _isPlayerAttack = false;
    private bool _isSprint;
    private bool _isRolling;
    public Vector2 Move => _move;
    public Vector2 Look => _look;
    public bool IsSprint => _isSprint;
    public bool IsPlayerAttack => _isPlayerAttack;

    public bool IsRolling => _isRolling;
    public event Action OnPlayerAttack;
    public event Action OnPlayerRolling;
    public event Action IsOpenItemBox;
    public event Action OnPlayerSkill_1;
    public event Action OnPlayerSkill_2;
    public event Action OnPlayerTeleport;
    public event Action<bool> OnPlayerGuard;
    public event Action OnPlayerHealing;
    public event Action OnPlayerPowerUp;
   
    public void OnMove(InputAction.CallbackContext context)
    {

        SetMoveInput(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        SetLookInput(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SetSprintState(context.performed || context.started);
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        SetRollingState(context.performed || context.started);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        SetAttackState(context.performed);
    }
    public void OnTeleport(InputAction.CallbackContext context)
    {
        PlayerTeleport(context.performed || context.started);
    }
    public void OnItemBox(InputAction.CallbackContext context)
    {
        OpenItemBox(context.performed || context.started);
    }

    public void OnSkill_1(InputAction.CallbackContext context)
    {
        SetPlayerSkill01(context.performed || context.started);
    }

    public void OnSkill_2(InputAction.CallbackContext context)
    {
        SetPlayerSkill02(context.performed || context.started);
    }

    public void OnGuard(InputAction.CallbackContext context)
    {
        // context.started -> 누른 순간
        // context.performed -> 누르고 있는 동안
        // context.canceled -> 뗐을 때
        SetPlayerGuard(context.performed || context.started); // 가드 유지
                                                              
        if (context.canceled) // context.canceled 시 원상복귀 처리
        {
            SetPlayerGuard(false);
        }    
    }
    public void OnHealing(InputAction.CallbackContext context)
    {
        if(context.performed || context.started)
        {
            UseHealingPotion();
        }
        
    }
    public void OnPowerUp(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            UsePowerPotion();
        }
    }
    void SetMoveInput(Vector2 vec)
    {
        _move = vec;
    }
    void SetLookInput(Vector2 vec)
    {
        _look = vec;
    }
    void SetSprintState(bool newState)
    {
        _isSprint = newState;
    }
    void SetAttackState(bool newState)
    {
        if (newState)
        {
            OnPlayerAttack?.Invoke();
        }

    }
    void SetRollingState(bool newState)
    {
        if (newState)
        {
            OnPlayerRolling?.Invoke();
        }
    }
    void OpenItemBox(bool newState)
    {
        if (newState)
        {
            IsOpenItemBox?.Invoke();
        }
    }
    void SetPlayerSkill01(bool state)
    {
        if (state)
        {
            OnPlayerSkill_1?.Invoke();
        }
    }
    void SetPlayerSkill02(bool state)
    {
        if (state)
        {
            OnPlayerSkill_2?.Invoke();
        }
    }
    void SetPlayerGuard(bool state)
    {
        
        OnPlayerGuard?.Invoke(state);
    }

    void PlayerTeleport(bool state)
    {
        OnPlayerTeleport?.Invoke();
    }
    private void UseHealingPotion()
    {
        OnPlayerHealing?.Invoke();
    }
    private void UsePowerPotion()
    {
        OnPlayerPowerUp?.Invoke();
    }
    public void DisablePlayerInput()
    {
       _playerInput.actions.FindActionMap("Player").Disable();

    }
    public void EnablePlayerInput()
    {
        _playerInput.actions.FindActionMap("Player").Enable();
    }
}
