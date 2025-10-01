using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Main HUD controller with health bar and ability cooldown display
/// </summary>
public class GameHUD : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerController player;

    [Header("UI Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image specialAbilityCooldownImage;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button specialAbilityButton;

    [Header("Conquest Zone")]
    [SerializeField] private ConquestZone conquestZone;
    [SerializeField] private Slider conquestBar;
    [SerializeField] private TextMeshProUGUI conquestText;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        // Setup button listeners
        if (attackButton != null)
        {
            attackButton.onClick.AddListener(() => player?.Attack());
        }

        if (specialAbilityButton != null)
        {
            specialAbilityButton.onClick.AddListener(() => player?.UseSpecialAbility());
        }

        if (conquestZone == null)
        {
            conquestZone = FindObjectOfType<ConquestZone>();
        }
    }

    private void Update()
    {
        UpdatePlayerHealth();
        UpdateSpecialAbilityCooldown();
        UpdateConquestZone();
    }

    private void UpdatePlayerHealth()
    {
        if (player == null || healthBar == null) return;

        float healthPercent = (float)player.CurrentHealth / player.MaxHealth;
        healthBar.value = healthPercent;

        if (healthText != null)
        {
            healthText.text = $"{player.CurrentHealth} / {player.MaxHealth}";
        }
    }

    private void UpdateSpecialAbilityCooldown()
    {
        if (player == null || specialAbilityCooldownImage == null) return;

        float cooldownProgress = player.SpecialAbilityCooldownProgress;
        specialAbilityCooldownImage.fillAmount = cooldownProgress;

        // Enable/disable button based on cooldown
        if (specialAbilityButton != null)
        {
            specialAbilityButton.interactable = (cooldownProgress >= 1f);
        }
    }

    private void UpdateConquestZone()
    {
        if (conquestZone == null || conquestBar == null) return;

        // Map -100 to 100 range to 0 to 1 for the slider
        float normalizedProgress = (conquestZone.CaptureProgress + 100f) / 200f;
        conquestBar.value = normalizedProgress;

        if (conquestText != null)
        {
            string team = conquestZone.ControllingTeam;
            conquestText.text = $"Zone: {team}";
        }
    }
}
