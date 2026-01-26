using System;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Transform _weaponAnchorRight;
    [SerializeField] private Transform _weaponAnchorLeft;
    [SerializeField] private Transform _slashContainer;
    [SerializeField] private GameObject _weapon;
    public event Action<bool> OnPlayerAttack;
    public event Action<bool> OnPlayerCombo;
    public event Action<bool> OnPlayerDodge;
    public event Action<bool> OnSwordTrigger;
    public event Action<bool> OnPlayerSkill;
    public event Action<bool> OnPlayerHitAnimation;
    public event Action<bool> OnPlayerGuardBreak;
    public event Action CreateSword;
    public event Action CreateSlash;
    public event Action OnStartSlashSkill;
    public event Action OnStartSwordSkill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _slashContainer.localRotation = _weaponAnchorRight.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AttackStart()
    {
        OnPlayerAttack?.Invoke(true);
    }
    void AttackEnd()
    {
        
        OnPlayerAttack?.Invoke(false);
    }
    void ComboStart()
    {
        OnPlayerCombo?.Invoke(true);
    }
    void ComboEnd()
    {
        OnPlayerCombo?.Invoke(false);
    }
    void DodgeStart()
    {
        OnPlayerDodge?.Invoke(true);
    }
    void DodgeEnd()
    {
        OnPlayerDodge?.Invoke(false);
    }
    void EnableSwordCollider()
    {
        OnSwordTrigger?.Invoke(true);
    }
    void DisableSwordCollider()
    {
        OnSwordTrigger?.Invoke(false);
    }
    void HitAnimationStart()
    {
        OnPlayerHitAnimation?.Invoke(true);
    }
    void HitAnimationEnd()
    {
        OnPlayerHitAnimation?.Invoke(false);
    }
    public void OnSwitchWeaponHandR()
    {

        _weapon.transform.SetParent(_weaponAnchorLeft, false);
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.identity;
       // _slashContainer.localRotation = weaponAnchorLeft.localRotation;


    }
    public void OnSwitchWeaponHandL()
    {
        _weapon.transform.SetParent(_weaponAnchorRight, false);
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.identity;
       _slashContainer.localRotation = _weaponAnchorRight.localRotation;
        
    }
    public void SkillStart()
    {
        OnPlayerSkill?.Invoke(true);
    }
    public void SkillEnd()
    {
        OnPlayerSkill?.Invoke(false);
    }
    public void CreateSkillSword()
    {
        CreateSword?.Invoke();
    }
    public void CreateSkillSlash()
    {
        CreateSlash?.Invoke();
    }
    public void GuardBreak()
    {
        OnPlayerGuardBreak?.Invoke(false);
    }
    public void SpawnSlashEffect()
    {
        OnStartSlashSkill?.Invoke();
    }   
    public void SpawnGreatSwordEffect()
    {
        OnStartSwordSkill?.Invoke();
    }

    public void PlaySwordSFXOne()
    {
        SoundManager.Instance.PlaySFX("Sword1");
    }
    public void PlaySwordSFXTwo()
    {
        SoundManager.Instance.PlaySFX("Sword2");
    }
    public void PlaySwordEnd()
    {
         SoundManager.Instance.PlaySFX("SwordEnd");
    }
    public void PlaySlashChargeSound()
    {
        SoundManager.Instance.PlaySFX("SlashCharge");
    }
    public void PlaySlashCahrgeEndSound()
    {
        SoundManager.Instance.PlaySFX("SlashChargeEnd");
    }
    public void PlayOnGuardSound()
    {
        SoundManager.Instance.PlaySFX("OnGuard");
    }
    public void PlayDashSound()
    {
        SoundManager.Instance.PlaySFX("Dash");
    }
    public void PlayFootStepSound()
    {
        SoundManager.Instance.PlaySFX("FootStep");
    }
    public void PlayArmorSound()
    {
        SoundManager.Instance.PlaySFX("Armor");
    }
    public void PlayJumpSound()
    {
        SoundManager.Instance.PlaySFX("Jump");
    }
}
