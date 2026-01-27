using UnityEngine;


public enum PotionType
{
    Healing,
    Buff,
}

[System.Serializable]
public struct PotionData
{
    public string Name;
    public float Cooldown;
    public ParticleSystem PotionParticle;

    [Header("Heal Potion")]
    public float HealAmount;

    [Header("Buff Potion")]
    public float BuffAmount;
    public float BuffDuration;

}