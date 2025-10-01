using UnityEngine;

namespace PixelRivals.Player
{
    [CreateAssetMenu(menuName = "PixelRivals/Skin", fileName = "SkinDefinitionSO")]
    public class SkinDefinitionSO : ScriptableObject
    {
        [Header("Visual")]
        [Tooltip("Prefab opzionale che sostituisce la mesh del personaggio (child).")]
        public GameObject overrideMeshPrefab;

        [Tooltip("Materiali da applicare al renderer principale, se non si usa un override prefab.")]
        public Material[] materials;

        [Header("Meta")]
        public string displayName = "Default";
        public Sprite icon;
    }
}