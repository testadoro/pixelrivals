using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Player Info")]
    public Image playerClassIcon;
    public TextMeshProUGUI playerClassName;
    public Slider playerHealthBar;
    public TextMeshProUGUI playerHealthText;
    
    [Header("Bot Icons")]
    public Image[] botClassIcons;
    public TextMeshProUGUI[] botClassNames;
    
    [Header("Cooldowns")]
    public Image basicAbilityCooldown;
    public Image ultimateAbilityCooldown;
    public TextMeshProUGUI basicCooldownText;
    public TextMeshProUGUI ultimateCooldownText;
    
    private PlayerController player;
    private BotController[] bots;
    
    private void Start()
    {
        FindPlayerAndBots();
        UpdatePlayerInfo();
        UpdateBotIcons();
    }
    
    private void Update()
    {
        if (player != null)
        {
            UpdateHealthBar();
        }
    }
    
    private void FindPlayerAndBots()
    {
        player = FindObjectOfType<PlayerController>();
        bots = FindObjectsOfType<BotController>();
    }
    
    public void SetPlayer(PlayerController playerController)
    {
        player = playerController;
        UpdatePlayerInfo();
    }
    
    public void SetBots(BotController[] botControllers)
    {
        bots = botControllers;
        UpdateBotIcons();
    }
    
    private void UpdatePlayerInfo()
    {
        if (player == null || player.PlayerClass == null)
            return;
        
        if (playerClassIcon != null && player.PlayerClass.classIcon != null)
        {
            playerClassIcon.sprite = player.PlayerClass.classIcon;
        }
        
        if (playerClassName != null)
        {
            playerClassName.text = player.PlayerClass.className;
        }
        
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        if (player == null || player.PlayerClass == null)
            return;
        
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = player.MaxHP;
            playerHealthBar.value = player.CurrentHP;
        }
        
        if (playerHealthText != null)
        {
            playerHealthText.text = $"{Mathf.RoundToInt(player.CurrentHP)} / {Mathf.RoundToInt(player.MaxHP)}";
        }
    }
    
    private void UpdateBotIcons()
    {
        if (bots == null)
            return;
        
        for (int i = 0; i < bots.Length && i < botClassIcons.Length; i++)
        {
            if (bots[i] != null && bots[i].BotClass != null)
            {
                if (botClassIcons[i] != null && bots[i].BotClass.classIcon != null)
                {
                    botClassIcons[i].sprite = bots[i].BotClass.classIcon;
                    botClassIcons[i].gameObject.SetActive(true);
                }
                
                if (botClassNames[i] != null)
                {
                    botClassNames[i].text = bots[i].BotClass.className;
                }
            }
        }
        
        // Hide unused bot icons
        for (int i = bots.Length; i < botClassIcons.Length; i++)
        {
            if (botClassIcons[i] != null)
            {
                botClassIcons[i].gameObject.SetActive(false);
            }
        }
    }
}
