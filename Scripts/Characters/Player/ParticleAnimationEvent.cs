using UnityEngine;

public class ParticleAnimationEvent : MonoBehaviour
{
    private Transform _SlashEffectPoint;
    private Transform _JumpEffectPoint;
    public void SetEffectPoint(Transform slashPoint, Transform jumpPoint)
    {
        _SlashEffectPoint = slashPoint;
        _JumpEffectPoint = jumpPoint;
    }

    public void PlaySkillEffect()
    {
        GameObject obj = ObjectPoolManager.Instance.Get(PoolType.SkillSlashEffect);
        if (obj != null) 
        {
            obj.transform.SetParent(_SlashEffectPoint);
            obj.transform.position = _SlashEffectPoint.position;
            obj.transform.rotation = _SlashEffectPoint.rotation;
            obj.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();
        }

    }
    public void PlayGreatSwordEffect()
    {
        PlayJumpEffect();
        PlayWindEffect();
    }
    private void PlayJumpEffect()
    {
        GameObject obj = ObjectPoolManager.Instance.Get(PoolType.JumpEffect);
        if (obj != null)
        {
            Vector3 pos = _JumpEffectPoint.position;
            pos.y += 2;
            obj.transform.position = pos;
            obj.transform.rotation = _JumpEffectPoint.rotation;
            obj.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();
        }
    }
    private void PlayWindEffect() 
    {
        GameObject obj = ObjectPoolManager.Instance.Get(PoolType.WindEffect);
        if (obj != null)
        {
            obj.transform.position = _JumpEffectPoint.position;
            obj.GetComponent<UnitEffectManager>()?.OnSpawnFromPool();
        }
    }

}
