using UnityEngine;

/// <summary>
/// Projectile script for attacks
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    private Vector3 direction;
    private string ownerTeam;
    private float spawnTime;

    public void Initialize(Vector3 dir, string team)
    {
        direction = dir.normalized;
        ownerTeam = team;
        spawnTime = Time.time;
    }

    private void Update()
    {
        // Move projectile
        transform.position += direction * speed * Time.deltaTime;

        // Destroy after lifetime
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't hit the owner
        if (ownerTeam == "Player" && other.CompareTag("Player"))
            return;

        if (ownerTeam != "Player" && other.CompareTag("Player"))
        {
            // Hit player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }

        // Hit bot
        BotAI bot = other.GetComponent<BotAI>();
        if (bot != null && ownerTeam == "Player")
        {
            bot.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Hit environment
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
