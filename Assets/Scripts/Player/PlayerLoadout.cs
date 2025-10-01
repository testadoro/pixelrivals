using UnityEngine;

namespace PixelRivals.Player
{
    // Connettore tra SO (class/skin) e componenti runtime
    [DisallowMultipleComponent]
    public class PlayerLoadout : MonoBehaviour
    {
        [Header("Loadout")]
        public PlayerClassSO playerClass;
        public SkinDefinitionSO skin;

        [Header("References (auto-find)")]
        public StatsRuntime statsRuntime;
        public SkinApplier skinApplier;

        private void Reset()
        {
            statsRuntime = GetComponent<StatsRuntime>();
            skinApplier = GetComponentInChildren<SkinApplier>();
        }

        private void Awake()
        {
            if (statsRuntime == null) statsRuntime = GetComponent<StatsRuntime>();
            if (statsRuntime != null && playerClass != null)
                statsRuntime.SetClass(playerClass);

            if (skinApplier != null && skin != null)
                skinApplier.SetSkin(skin);
        }
    }
}