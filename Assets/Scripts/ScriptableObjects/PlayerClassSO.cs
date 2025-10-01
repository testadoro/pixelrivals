using UnityEngine;

[CreateAssetMenu(fileName = "New Player Class", menuName = "PixelRivals/Player Class")]
public class PlayerClassSO : ScriptableObject
{
    [Header("Class Info")]
    public string className;
    public Sprite classIcon;
    public ClassType classType;
    
    [Header("Base Stats")]
    public float baseHP = 100f;
    public float baseAttack = 20f;
    public float baseSpeed = 5f;
    public float attackCooldown = 1f;
    public float ultimateCooldown = 10f;
    
    [Header("Ability Info")]
    public AbilityType basicAbilityType;
    public AbilityType ultimateAbilityType;
    public float abilityRange = 10f;
    public float abilityValue = 20f; // Damage for attacks, heal for support
    
    [Header("Available Skins")]
    public SkinData[] availableSkins;
}

[System.Serializable]
public class SkinData
{
    public string skinName;
    public Material skinMaterial;
    public Mesh skinMesh;
}

public enum ClassType
{
    Tank,
    DPS,
    Support
}

public enum AbilityType
{
    MeleeAttack,
    RangedAttack,
    HealCone,
    HealAOE,
    KnockbackAOE
}
