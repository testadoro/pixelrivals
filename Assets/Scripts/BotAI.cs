using UnityEngine;

/// <summary>
/// AI controller for enemy bots
/// </summary>
public class BotAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Combat")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float detectionRange = 15f;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 80;
    private int currentHealth;

    [Header("Team")]
    [SerializeField] private string botTeam = "Enemy";

    private CharacterController characterController;
    private Transform target;
    private float lastAttackTime;
    private Vector3 patrolTarget;
    private float patrolWaitTime;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
            characterController.radius = 0.5f;
            characterController.height = 2f;
        }
        currentHealth = maxHealth;
        SetNewPatrolTarget();
    }

    private void Update()
    {
        FindTarget();
        
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                AttackTarget();
            }
            else if (distanceToTarget <= detectionRange)
            {
                MoveTowardsTarget();
            }
            else
            {
                target = null;
                Patrol();
            }
        }
        else
        {
            Patrol();
        }

        // Apply gravity
        characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    private void FindTarget()
    {
        if (target != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                target = player.transform;
            }
        }
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        characterController.Move(movement);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void AttackTarget()
    {
        if (target == null) return;

        // Face the target
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Attack if cooldown is ready
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Projectile proj = projectile.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.Initialize(transform.forward, botTeam);
                }
            }
        }
    }

    private void Patrol()
    {
        if (patrolWaitTime > 0)
        {
            patrolWaitTime -= Time.deltaTime;
            return;
        }

        float distanceToPatrol = Vector3.Distance(transform.position, patrolTarget);
        
        if (distanceToPatrol < 1f)
        {
            SetNewPatrolTarget();
            patrolWaitTime = Random.Range(1f, 3f);
            return;
        }

        Vector3 direction = (patrolTarget - transform.position).normalized;
        direction.y = 0;

        Vector3 movement = direction * (moveSpeed * 0.5f) * Time.deltaTime;
        characterController.Move(movement);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SetNewPatrolTarget()
    {
        // Set a random patrol point within a radius
        Vector2 randomPoint = Random.insideUnitCircle * 15f;
        patrolTarget = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{botTeam} bot died!");
        Destroy(gameObject);
    }
}
