using UnityEngine;

/// <summary>
/// Validates the scene setup and reports any missing components or configurations
/// This script can be attached to an empty GameObject to help debug setup issues
/// </summary>
public class SetupValidator : MonoBehaviour
{
    [Header("Validation Settings")]
    [SerializeField] private bool validateOnStart = true;
    [SerializeField] private bool logSuccessfulChecks = true;

    private void Start()
    {
        if (validateOnStart)
        {
            ValidateSetup();
        }
    }

    [ContextMenu("Validate Scene Setup")]
    public void ValidateSetup()
    {
        Debug.Log("=== PixelRivals Setup Validation ===");
        
        ValidatePlayer();
        ValidateBots();
        ValidateConquestZone();
        ValidateUI();
        ValidateGameManager();
        ValidateCamera();
        
        Debug.Log("=== Validation Complete ===");
    }

    private void ValidatePlayer()
    {
        Debug.Log("\n--- Player Validation ---");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("❌ No GameObject with 'Player' tag found!");
            return;
        }
        LogSuccess("✓ Player GameObject found");

        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller == null)
        {
            Debug.LogError("❌ PlayerController component missing on Player!");
        }
        else
        {
            LogSuccess("✓ PlayerController component present");
        }

        CharacterController charController = player.GetComponent<CharacterController>();
        if (charController == null)
        {
            Debug.LogWarning("⚠ CharacterController missing on Player (will be auto-added)");
        }
        else
        {
            LogSuccess("✓ CharacterController component present");
        }

        Transform firePoint = player.transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("❌ FirePoint child object missing on Player!");
        }
        else
        {
            LogSuccess("✓ FirePoint child object found");
        }
    }

    private void ValidateBots()
    {
        Debug.Log("\n--- Bot Validation ---");
        
        BotAI[] bots = FindObjectsOfType<BotAI>();
        if (bots.Length == 0)
        {
            Debug.LogWarning("⚠ No bots found in scene (they may be spawned at runtime)");
        }
        else
        {
            LogSuccess($"✓ Found {bots.Length} bot(s) in scene");
            
            foreach (BotAI bot in bots)
            {
                Transform firePoint = bot.transform.Find("FirePoint");
                if (firePoint == null)
                {
                    Debug.LogError($"❌ FirePoint missing on bot: {bot.gameObject.name}");
                }
            }
        }
    }

    private void ValidateConquestZone()
    {
        Debug.Log("\n--- Conquest Zone Validation ---");
        
        ConquestZone zone = FindObjectOfType<ConquestZone>();
        if (zone == null)
        {
            Debug.LogError("❌ No ConquestZone found in scene!");
            return;
        }
        LogSuccess("✓ ConquestZone found");

        Collider collider = zone.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("❌ ConquestZone missing Collider!");
        }
        else if (!collider.isTrigger)
        {
            Debug.LogError("❌ ConquestZone Collider is not set to 'Is Trigger'!");
        }
        else
        {
            LogSuccess("✓ ConquestZone has trigger collider");
        }
    }

    private void ValidateUI()
    {
        Debug.Log("\n--- UI Validation ---");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found in scene!");
            return;
        }
        LogSuccess("✓ Canvas found");

        VirtualJoystick joystick = FindObjectOfType<VirtualJoystick>();
        if (joystick == null)
        {
            Debug.LogError("❌ VirtualJoystick not found in scene!");
        }
        else
        {
            LogSuccess("✓ VirtualJoystick found");
        }

        GameHUD hud = FindObjectOfType<GameHUD>();
        if (hud == null)
        {
            Debug.LogError("❌ GameHUD not found in scene!");
        }
        else
        {
            LogSuccess("✓ GameHUD found");
        }

        UnityEngine.EventSystems.EventSystem eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError("❌ EventSystem missing! UI input will not work!");
        }
        else
        {
            LogSuccess("✓ EventSystem found");
        }
    }

    private void ValidateGameManager()
    {
        Debug.Log("\n--- Game Manager Validation ---");
        
        GameManager manager = FindObjectOfType<GameManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠ GameManager not found (bots may need manual spawning)");
        }
        else
        {
            LogSuccess("✓ GameManager found");
        }
    }

    private void ValidateCamera()
    {
        Debug.Log("\n--- Camera Validation ---");
        
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("❌ No main camera found!");
        }
        else
        {
            LogSuccess("✓ Main camera found");
        }
    }

    private void LogSuccess(string message)
    {
        if (logSuccessfulChecks)
        {
            Debug.Log(message);
        }
    }
}
