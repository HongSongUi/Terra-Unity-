using System;

using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    protected float _maxHealth = 100;

    protected float _currentHealth;

    [SerializeField] private float _damageCooldown = 0.5f;
    private float _lastDamageTime = -999f;

    private bool _isDead = false;
    private bool _isWarning = false;

    public bool IsDead => _isDead;
    public float Health => _currentHealth;
    public float MaxHealth => _maxHealth;
    public float HealthPercentage => _currentHealth / _maxHealth;

    public event Action<HealthComponent,float> OnHealthChange;
    public event Action OnCharacterDeath;
    public event Action<bool> OnHealthWarning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        _currentHealth = _maxHealth;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    public virtual bool ApplyDamage(float damage)
    {
        if (Time.time - _lastDamageTime < _damageCooldown)
        {            
            // damage 못 받음
            return false;
        }
        if(_isDead == false)
        {
            _lastDamageTime = Time.time;
            _currentHealth -= damage;
            HealthCheck();
            if (_currentHealth <= 0)
            {
                _isDead = true;
                OnDeath();
            }
            OnHealthChange?.Invoke(this, HealthPercentage);
            //Debug.Log($"{gameObject.name}이(가) {damage} 데미지를 입음. 남은 체력: {_currentHealth}");
        }
        return true;
    }
    private void HealthCheck()
    {
        if (_currentHealth < _maxHealth * 0.3f && !_isWarning)
        {
            _isWarning = true;
            OnHealthWarning?.Invoke(_isWarning); // 체력 경고 애니메이션 시작
        }
    }

    public virtual void Healing(float healAmount)
    {
        _currentHealth += healAmount;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        if(_isWarning && _currentHealth>=MaxHealth*0.3f)
        {
            _isWarning = false;
            OnHealthWarning?.Invoke(_isWarning);
        }
        OnHealthChange?.Invoke(this, HealthPercentage); // 체력바 업데이트

    }

    public void OnDeath()
    {
        OnCharacterDeath?.Invoke();
    }

}
