using System;
using UnityEngine;

public class DragonAnimationManager : MonoBehaviour
{
    //애니메이션 이벤트 등록


    public event Action OnFireBallAttack;
    public event Action OnRoarAnimEnd;
    public event Action OnAttackAnimEnd;
   
    public event Action<bool> OnFlyState;
   
    public event Action<bool> OnFireBreath;
    public event Action<bool> OnPlayFireEffect;

    public event Action<bool> RightArmHitBoxTrigger;
    public event Action<bool> TailAttackTrigger;
    public event Action OnEarthShatter;
    public event Action IsBackStepEnd;
    public event Action OnChargeFireBallAttack;
    public event Action OnPlayFireChargeEffect;
    public void EnableRightArmHitBox()
    {
        RightArmHitBoxTrigger?.Invoke(true);
    }
    public void DisableRightArmHitBox() 
    {
        RightArmHitBoxTrigger?.Invoke(false);
    }

    public void OnFireBall()
    {
        OnFireBallAttack?.Invoke();
    }
    public void OnChargeFireBall()
    {
        OnChargeFireBallAttack?.Invoke();
    }
    public void StartFireBreath()
    {
        OnFireBreath?.Invoke(true);
    }
    public void EndFireBreath()
    {
        OnFireBreath?.Invoke(false);
    }

    public void OnRoarEnd()
    {
        OnRoarAnimEnd?.Invoke();
    }
    public void OnAttackEnd()
    {
        OnAttackAnimEnd?.Invoke();
    }
    public void OnFlyStart()
    {

        OnFlyState?.Invoke(true);
    }
    public void OnFlyEnd()
    {
        OnFlyState?.Invoke(false);
    }
    private void OnBackStepEnd()
    {
        IsBackStepEnd?.Invoke();
    }
    public void PlayFireEffect()
    {
        OnPlayFireEffect?.Invoke(true);
    }
    public void StopFireEffect()
    {
        OnPlayFireEffect?.Invoke(false);
    }
    public void PlayFireChargeEffect()
    {
        OnPlayFireChargeEffect?.Invoke();
    }

    public void TailAttackEnable()
    {
        TailAttackTrigger?.Invoke(true);
    }
    public void TailAttackDisable()
    {
        TailAttackTrigger?.Invoke(false);
    
    }
    public void PlayEarthShatter()
    {
        OnEarthShatter?.Invoke();
    }
    public void PlayScratchSound()
    {
        SoundManager.Instance.PlaySFX("Scratch");
    }
    public void PlayNailSound()
    {
        SoundManager.Instance.PlaySFX("Nail");
    }
    public void PlayEarthImpactSound()
    {
        SoundManager.Instance.PlaySFX("EarthImpact");
    }
    public void PlayStampSound()
    {
        SoundManager.Instance.PlaySFX("StampSound");
    }
    public void PlayBreathSound()
    {
        SoundManager.Instance.PlaySFX("DragonBreath");
    }
    public void PlayGrowlSound()
    {
        SoundManager.Instance.PlaySFX("DragonGrowl");
    }
    public void PlayPreAttackGrowlSound()
    {
        SoundManager.Instance.PlaySFX("PreAttackGrowl");
    }
    public void PlayTailSwingSound()
    {
        SoundManager.Instance.PlaySFX("TailSwing");
    }
    public void PlayDragonBreathTwo()
    {
        SoundManager.Instance.PlaySFX("DragonBreathTwo");
    }
    public void PlayDragonGrowlTwo()
    {
        SoundManager.Instance.PlaySFX("DragonGrowlTwo");
    }
    public void PlayWingSound()
    {
        SoundManager.Instance.PlaySFX("DragonWing");
    }
    public void FireBallSound()
    {
        SoundManager.Instance.PlaySFX("SpawnFireBall");
    }
    public void FireBallGrowling()
    {
        SoundManager.Instance.PlaySFX("FireBallGrowling");
    }
    public void PlayChargingSound()
    {
        SoundManager.Instance.PlaySFX("Charging");
    }
    public void PlayFlyUpSound()
    {
        SoundManager.Instance.PlaySFX("FlyUp");
    }
    public void PlayTackleSound()
    {
        SoundManager.Instance.PlaySFX("Tackle");
    }
    public void PlayBackStepSound()
    {
        SoundManager.Instance.PlaySFX("BackStep");
    }
    public void PlayFootStepSound()
    {
        SoundManager.Instance.PlaySFX("DragonFootStep");
    }
    public void PlayRoarSound()
    {
        SoundManager.Instance.PlaySFX("DragonRoar");
    }
    public void PlayDeathSound()
    {
        SoundManager.Instance.PlaySFX("DragonDeath");
    }
    public void PlayDeathGrowl()
    {
        SoundManager.Instance.PlaySFX("DeathGrowl");
    }
    public void PlayGiantFall()
    {
        SoundManager.Instance.PlaySFX("GiantFall");
    }
    public void PlayChargerBallLaunch()
    {
        SoundManager.Instance.PlaySFX("ChargeFireBallLaunch");
    }
}
