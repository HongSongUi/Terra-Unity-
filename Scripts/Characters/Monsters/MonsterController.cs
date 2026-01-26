using Unity.Behavior;
using UnityEngine;

// TODO : TestFunc 이름 바꾸기

public class MonsterController : BaseCharacterController
{
    [SerializeField]
    private BehaviorGraphAgent _behaviorAgent;
    [SerializeField]
    private SightComponent _sight;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Animator _animator;

    private HealthImageComponent _healthImg;

    private bool _isChase = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        base.Awake();
        _healthImg = GetComponent<HealthImageComponent>();
        _sight.OnPlayerDetected += TestFun;
    }
    private void OnDestroy()
    {
        _sight.OnPlayerDetected -= TestFun;
    }

    void Start()
    {
        if(_behaviorAgent == null)
        {
            _behaviorAgent = GetComponent<BehaviorGraphAgent>();
        }
        _behaviorAgent.SetVariableValue("Target", _target.gameObject);

        _healthComponent.OnHealthChange += _healthImg.SetHealthAmount;
        _healthComponent.OnCharacterDeath += DeathEvent;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void TestFun(bool state)
    {
        _isChase = state;
         _behaviorAgent.SetVariableValue("FindTarget", state);
    }

    private void HealthEvent(int health)
    {
        if(health<=0)
        {
            DeathEvent();
        }
       
    }
    private void DeathEvent()
    {
        GameManager.Instance.DecreaseMonsterCount();
        _behaviorAgent.SetVariableValue("IsDead", true);
    }
}
