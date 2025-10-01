using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    
    [Header("Selected Class")]
    private PlayerClassSO selectedPlayerClass;
    private int selectedSkinIndex;
    
    [Header("Game State")]
    public PlayerController playerPrefab;
    public BotController botPrefab;
    public Transform playerSpawnPoint;
    public Transform[] botSpawnPoints;
    
    private PlayerController currentPlayer;
    private BotController[] currentBots;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void SetPlayerClass(PlayerClassSO classData, int skinIndex)
    {
        selectedPlayerClass = classData;
        selectedSkinIndex = skinIndex;
    }
    
    public void InitializeMatch(PlayerClassSO[] botClasses = null)
    {
        // Spawn player
        if (playerPrefab != null && playerSpawnPoint != null && selectedPlayerClass != null)
        {
            currentPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            currentPlayer.SetPlayerClass(selectedPlayerClass, selectedSkinIndex);
        }
        
        // Spawn bots
        if (botPrefab != null && botSpawnPoints != null && botClasses != null)
        {
            currentBots = new BotController[Mathf.Min(botSpawnPoints.Length, botClasses.Length)];
            
            for (int i = 0; i < currentBots.Length; i++)
            {
                currentBots[i] = Instantiate(botPrefab, botSpawnPoints[i].position, Quaternion.identity);
                currentBots[i].SetBotClass(botClasses[i], Random.Range(0, botClasses[i].availableSkins.Length));
            }
        }
        
        // Update HUD
        var hud = FindObjectOfType<GameHUD>();
        if (hud != null)
        {
            hud.SetPlayer(currentPlayer);
            hud.SetBots(currentBots);
        }
    }
    
    public PlayerClassSO GetSelectedPlayerClass()
    {
        return selectedPlayerClass;
    }
    
    public int GetSelectedSkinIndex()
    {
        return selectedSkinIndex;
    }
}
