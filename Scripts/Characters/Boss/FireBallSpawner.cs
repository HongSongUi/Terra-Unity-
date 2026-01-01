using UnityEngine;

public class FireBallSpawner : MonoBehaviour
{
    [SerializeField] 
    private Transform _spawnPoint;

    [SerializeField]
    private GameObject  _fireEffect;


    private GameObject _fireBreath;
    private GameObject _igniteFire;

    public void SpawnFireBall()
    {
        var fireball = ObjectPoolManager.Instance.Get(PoolType.FireBall);
        fireball.transform.position = _spawnPoint.position;
        fireball.transform.rotation = _spawnPoint.rotation;
    }
    public void SpawnChargeFireBall()
    {
        var chargeFireBall = ObjectPoolManager.Instance.Get(PoolType.ChargeFireBall);
        chargeFireBall.transform.position = _spawnPoint.position;
        chargeFireBall.transform.rotation = _spawnPoint.rotation;
    }

    public void SpawnFireBreath(bool state)
    {
        if(state)
        {
            _fireBreath = ObjectPoolManager.Instance.Get(PoolType.FireBreath);
            if(_fireBreath != null)
            {
                _fireBreath.transform.SetParent(_spawnPoint);
                _fireBreath.transform.position = _spawnPoint.position;
                _fireBreath.transform.rotation = _spawnPoint.rotation;
            }
        }
        else
        {
            DragonFireBreath breath = _fireBreath.GetComponent<DragonFireBreath>();
            if(breath)
            {
                breath.OnReturnToPool();
            }
        }
    }
    public void SetFireEffectState(bool state)
    {
        if (state)
        {
            if (_igniteFire != null) return;
            _igniteFire = ObjectPoolManager.Instance.Get(PoolType.IgniteEffect);
            _igniteFire.transform.SetParent(_spawnPoint);
            _igniteFire.transform.position = _spawnPoint.position;
            _igniteFire.GetComponent<LoopingEffectManager>()?.OnSpawnFromPool();
        }
        else
        {
            _igniteFire.GetComponent<LoopingEffectManager>()?.StopAndFadeOut();
            _igniteFire = null;
        }
    }
}
