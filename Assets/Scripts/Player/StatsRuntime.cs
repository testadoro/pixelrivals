using UnityEngine;

namespace PixelRivals.Player
{
    // Stats runtime (supporta modifiche dinamiche)
    public class StatsRuntime : MonoBehaviour
    {
        [SerializeField] private PlayerClassSO playerClass;

        public int MaxHP { get; private set; } = 100;
        public float MoveSpeed { get; private set; } = 8f;

        public float AttackDamage { get; private set; } = 25f;
        public float AttackRate { get; private set; } = 0.3f;
        public float AttackRange { get; private set; } = 10f;
        public float ProjectileSpeed { get; private set; } = 18f;

        public float UltimateDamage { get; private set; } = 40f;
        public float UltimateRadius { get; private set; } = 4f;
        public float UltimateCooldown { get; private set; } = 6f;

        public PlayerClassSO ClassAsset => playerClass;

        public void SetClass(PlayerClassSO newClass)
        {
            playerClass = newClass;
            ApplyFromClass();
        }

        private void Awake()
        {
            ApplyFromClass();
        }

        private void ApplyFromClass()
        {
            if (playerClass == null) return;

            MaxHP = playerClass.maxHP;
            MoveSpeed = playerClass.moveSpeed;

            AttackDamage = playerClass.attackDamage;
            AttackRate = playerClass.attackRate;
            AttackRange = playerClass.attackRange;
            ProjectileSpeed = playerClass.projectileSpeed;

            UltimateDamage = playerClass.ultimateDamage;
            UltimateRadius = playerClass.ultimateRadius;
            UltimateCooldown = playerClass.ultimateCooldown;

            SendMessage("OnStatsChanged", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}