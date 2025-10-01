using UnityEngine;

namespace PixelRivals.Player
{
    // Applica skin come override prefab o materiali
    public class SkinApplier : MonoBehaviour
    {
        [SerializeField] private Transform meshRoot;
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private SkinDefinitionSO skin;

        private GameObject _spawnedOverride;

        public void SetSkin(SkinDefinitionSO newSkin)
        {
            skin = newSkin;
            Apply();
        }

        private void Awake()
        {
            Apply();
        }

        private void Apply()
        {
            if (skin == null) return;

            if (_spawnedOverride != null)
            {
                Destroy(_spawnedOverride);
                _spawnedOverride = null;
            }

            if (skin.overrideMeshPrefab != null && meshRoot != null)
            {
                _spawnedOverride = Instantiate(skin.overrideMeshPrefab, meshRoot);
                if (targetRenderer != null) targetRenderer.enabled = false;
            }
            else if (skin.materials != null && skin.materials.Length > 0 && targetRenderer != null)
            {
                targetRenderer.enabled = true;
                targetRenderer.sharedMaterials = skin.materials;
            }
        }
    }
}