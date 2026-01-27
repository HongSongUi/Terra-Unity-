using UnityEngine;

public class DragonEffectManager : MonoBehaviour
{
 
    [SerializeField]
    private Transform _stampEffectPoint;
    [SerializeField]
    private Transform _chargeEffectPoint;
    public void PlayStampAttackParticle()
    {
        GameObject obj = ObjectPoolManager.Instance.Get(PoolType.EarthShatter);
        Vector3 pos = _stampEffectPoint.transform.position;
        pos.y -= 0.5f;
        obj.transform.position = pos;
        obj.transform.rotation = _stampEffectPoint.localRotation;
        obj.GetComponent<EarthShatter>()?.OnSpawnFromPool();
    }
    public void PlayerChargeEffect()
    {
        GameObject obj = ObjectPoolManager.Instance.Get(PoolType.BreathChargeEffect);
        obj.transform.SetParent(_chargeEffectPoint);
        obj.transform.position = _chargeEffectPoint.transform.position;
        obj.transform.rotation = _chargeEffectPoint.transform.rotation;
        obj.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();

    }
}
