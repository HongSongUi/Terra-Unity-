using UnityEngine;

public class SkillManager : MonoBehaviour
{

    [SerializeField]
    private Transform _swordSpawnPoint;
    [SerializeField]
    private Transform _slashSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateSword()
    {
        var sword = ObjectPoolManager.Instance.Get(PoolType.GreatSwordSkill);
        sword.transform.position = _swordSpawnPoint.position;
        sword.transform.rotation = Quaternion.Euler(_swordSpawnPoint.eulerAngles.x, _swordSpawnPoint.eulerAngles.y, -180f);
        sword.GetComponent<SwordSkill>()?.OnSpawnFromPool();
       
    }

    public void CreateSlashSkill()
    {
        var proj = ObjectPoolManager.Instance.Get(PoolType.SlashProjectile);
        proj.transform.position = _slashSpawnPoint.position;
        proj.transform.rotation = _slashSpawnPoint.rotation;

        proj.GetComponent<SlashSkill>()?.OnSpawnFromPool();

    }
}
