using System;
using UnityEngine;

public class PlayerInventoryComponent : MonoBehaviour
{
    [SerializeField]
    private PotionData _healingPotion;
    [SerializeField]
    private PotionData _attackBuffPotion;

    private float _lastHealingUseTime = 0;
    private float _lastBuffUseTime = 0;

    private HealthComponent _healthComponent;
    private CombatComponent _combatComponent;


    public event Action<PotionType, float> OnStartPotionCooldown;
    private void Awake()
    {
        _lastHealingUseTime = Time.time - _healingPotion.Cooldown;
        _lastBuffUseTime = Time.time - _attackBuffPotion.Cooldown;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetComponent(HealthComponent health, CombatComponent combat)
    {
        _healthComponent = health;
        _combatComponent = combat;
    }
    public void UseHealingPotion(Transform tr)
    {
        if(Time.time < _lastHealingUseTime + _healingPotion.Cooldown)
        {
            return;
        }
    
        _lastHealingUseTime = Time.time;
        _healthComponent.Healing(_healingPotion.HealAmount);
        SoundManager.Instance.PlaySFX("UsePotion");
        GameObject effect = ObjectPoolManager.Instance.Get(PoolType.HealingEffect);
        if (effect != null)
        { 
            effect.transform.SetParent(tr);
            Vector3 spawnPosition = tr.position;
            spawnPosition.y -= 1.5f;
            effect.transform.position = spawnPosition;
            effect.transform.rotation = tr.rotation;
            effect.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();
        }
        OnStartPotionCooldown?.Invoke(PotionType.Healing , _healingPotion.Cooldown);
    }
    public void UseAttackBuffPotion(Transform tr)
    {
        if (Time.time < _lastBuffUseTime + _attackBuffPotion.Cooldown)
        {
            return;
        }
       
        _lastBuffUseTime = Time.time;
        _combatComponent.ApplyAttackBuff(_attackBuffPotion.BuffAmount, _attackBuffPotion.BuffDuration);
        SoundManager.Instance.PlaySFX("UsePotion");
        GameObject effect = ObjectPoolManager.Instance.Get(PoolType.BuffEffect);
        if (effect != null)
        {
            effect.transform.SetParent(tr);
            Vector3 spawnPosition = tr.position;
            spawnPosition.y -= 1.5f;
            effect.transform.position = spawnPosition;
            effect.transform.rotation = tr.rotation;
            effect.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();
        }
        OnStartPotionCooldown?.Invoke(PotionType.Buff, _attackBuffPotion.Cooldown);
    }
}
