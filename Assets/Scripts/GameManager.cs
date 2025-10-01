using UnityEngine;

/// <summary>
/// Main game manager for spawning bots and managing the match
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private int botsPerTeam = 3;
    [SerializeField] private Transform[] playerTeamSpawnPoints;
    [SerializeField] private Transform[] enemyTeamSpawnPoints;

    [Header("Match Settings")]
    [SerializeField] private float matchDuration = 300f; // 5 minutes
    private float matchStartTime;

    private void Start()
    {
        matchStartTime = Time.time;
        SpawnBots();
    }

    private void SpawnBots()
    {
        if (botPrefab == null)
        {
            Debug.LogWarning("Bot prefab not assigned!");
            return;
        }

        // Spawn player team bots (if implemented)
        if (playerTeamSpawnPoints != null && playerTeamSpawnPoints.Length > 0)
        {
            for (int i = 0; i < Mathf.Min(botsPerTeam, playerTeamSpawnPoints.Length); i++)
            {
                if (playerTeamSpawnPoints[i] != null)
                {
                    GameObject bot = Instantiate(botPrefab, playerTeamSpawnPoints[i].position, Quaternion.identity);
                    // Configure as friendly bot
                }
            }
        }

        // Spawn enemy team bots
        if (enemyTeamSpawnPoints != null && enemyTeamSpawnPoints.Length > 0)
        {
            for (int i = 0; i < Mathf.Min(botsPerTeam, enemyTeamSpawnPoints.Length); i++)
            {
                if (enemyTeamSpawnPoints[i] != null)
                {
                    GameObject bot = Instantiate(botPrefab, enemyTeamSpawnPoints[i].position, Quaternion.identity);
                    bot.name = $"EnemyBot_{i}";
                }
            }
        }
    }

    private void Update()
    {
        float elapsedTime = Time.time - matchStartTime;
        if (elapsedTime >= matchDuration)
        {
            EndMatch();
        }
    }

    private void EndMatch()
    {
        Debug.Log("Match ended!");
        // Match end logic would go here
    }

    public float GetRemainingTime()
    {
        return Mathf.Max(0, matchDuration - (Time.time - matchStartTime));
    }
}
