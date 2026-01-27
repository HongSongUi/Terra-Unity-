using UnityEngine;

[System.Serializable]

public struct DamageInfo
{
    public float DamageAmount;
    public bool IsKnockbackAttack; // player가 맞았을 때 true -> 날라감, false -> 움찔

    public DamageInfo(float damage, bool knockback)
    {
        DamageAmount = damage;
        IsKnockbackAttack = knockback;
    }
}