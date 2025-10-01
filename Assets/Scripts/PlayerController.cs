using UnityEngine;

/// <summary>
/// Controls the player character with virtual joystick input
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Combat")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float specialAbilityCooldown = 5f;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float lastAttackTime;
    private float lastSpecialAbilityTime;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public float SpecialAbilityCooldownProgress => Mathf.Clamp01((Time.time - lastSpecialAbilityTime) / specialAbilityCooldown);

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
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            // Move character
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            characterController.Move(movement);

            // Rotate character to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Apply gravity
        characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
    }

    /// <summary>
    /// Called by virtual joystick to set movement direction
    /// </summary>
    public void SetMoveDirection(Vector2 input)
    {
        moveDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    /// <summary>
    /// Called by attack button
    /// </summary>
    public void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        if (projectilePrefab == null || firePoint == null) return;

        lastAttackTime = Time.time;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Initialize(transform.forward, "Player");
        }
    }

    /// <summary>
    /// Called by special ability button
    /// </summary>
    public void UseSpecialAbility()
    {
        if (Time.time - lastSpecialAbilityTime < specialAbilityCooldown) return;

        lastSpecialAbilityTime = Time.time;

        // Special ability: fire 3 projectiles in a spread
        if (projectilePrefab == null || firePoint == null) return;

        for (int i = -1; i <= 1; i++)
        {
            Vector3 direction = Quaternion.Euler(0, i * 15f, 0) * transform.forward;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.Initialize(direction, "Player");
            }
        }
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
        Debug.Log("Player died!");
        // Respawn or game over logic would go here
        currentHealth = maxHealth;
        transform.position = Vector3.zero;
    }
}
