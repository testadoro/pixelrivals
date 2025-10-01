using UnityEngine;

namespace PixelRivals.Player
{
    [CreateAssetMenu(menuName = "PixelRivals/Player Class", fileName = "PlayerClassSO")]
    public class PlayerClassSO : ScriptableObject
    {
        [Header("Stats base")]
        [Min(1)] public int maxHP = 100;

        [Min(0f)] public float moveSpeed = 8f;

        [Header("Attacco Base")]
        [Min(0f)] public float attackDamage = 25f;
        [Min(0.05f)] public float attackRate = 0.3f; // secondi tra colpi
        [Min(0f)] public float attackRange = 10f;
        [Min(0f)] public float projectileSpeed = 18f;

        [Header("Ultimate")]
        [Min(0f)] public float ultimateDamage = 40f;
        [Min(0f)] public float ultimateRadius = 4f;
        [Min(0f)] public float ultimateCooldown = 6f;
    }
}