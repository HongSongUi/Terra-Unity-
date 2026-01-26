using UnityEngine;

[System.Serializable]

public struct DamageInfo
{
    public float DamageAmount;
    public bool IsKnockbackAttack;

    public DamageInfo(float damage, bool knockback)
    {
        DamageAmount = damage;
        IsKnockbackAttack = knockback;
    }
}