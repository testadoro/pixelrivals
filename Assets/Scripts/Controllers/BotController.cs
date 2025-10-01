using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BotController : MonoBehaviour
{
    [Header("Class Configuration")]
    public PlayerClassSO botClass;
    public int selectedSkinIndex = 0;
    
    [Header("AI Settings")]
    public float detectionRange = 15f;
    public float attackRange = 5f;
    
    [Header("Runtime Stats")]
    private float currentHP;
    private float currentSpeed;
    private float attackDamage;
    private float attackCooldownTimer;
    private float ultimateCooldownTimer;
    
    [Header("Components")]
    private CharacterController characterController;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    [Header("AI State")]
    private Transform currentTarget;
    private Vector3 wanderTarget;
    private float wanderTimer;
    
    public float CurrentHP => currentHP;
    public float MaxHP => botClass.baseHP;
    public PlayerClassSO BotClass => botClass;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    
    private void Start()
    {
        if (botClass != null)
        {
            InitializeStats();
            ApplySkin();
        }
        SetNewWanderTarget();
    }
    
    public void SetBotClass(PlayerClassSO classData, int skinIndex = 0)
    {
        botClass = classData;
        selectedSkinIndex = skinIndex;
        InitializeStats();
        ApplySkin();
    }
    
    private void InitializeStats()
    {
        currentHP = botClass.baseHP;
        currentSpeed = botClass.baseSpeed;
        attackDamage = botClass.baseAttack;
        attackCooldownTimer = 0f;
        ultimateCooldownTimer = 0f;
    }
    
    private void ApplySkin()
    {
        if (botClass.availableSkins != null && 
            selectedSkinIndex >= 0 && 
            selectedSkinIndex < botClass.availableSkins.Length)
        {
            var skin = botClass.availableSkins[selectedSkinIndex];
            
            if (meshFilter != null && skin.skinMesh != null)
            {
                meshFilter.mesh = skin.skinMesh;
            }
            
            if (meshRenderer != null && skin.skinMaterial != null)
            {
                meshRenderer.material = skin.skinMaterial;
            }
        }
    }
    
    private void Update()
    {
        UpdateCooldowns();
        UpdateAI();
    }
    
    private void UpdateCooldowns()
    {
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        
        if (ultimateCooldownTimer > 0)
            ultimateCooldownTimer -= Time.deltaTime;
    }
    
    private void UpdateAI()
    {
        FindTarget();
        
        if (currentTarget != null)
        {
            EngageTarget();
        }
        else
        {
            Wander();
        }
    }
    
    private void FindTarget()
    {
        // Find nearest enemy player or bot
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRange);
        float nearestDistance = float.MaxValue;
        Transform nearestEnemy = null;
        
        foreach (var obj in nearbyObjects)
        {
            // Check if it's an enemy (simple team check - can be expanded)
            if (obj.gameObject != gameObject && obj.GetComponent<PlayerController>() != null)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = obj.transform;
                }
            }
        }
        
        currentTarget = nearestEnemy;
    }
    
    private void EngageTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        
        if (distanceToTarget > attackRange)
        {
            // Move towards target
            MoveTowards(currentTarget.position);
        }
        else
        {
            // Attack target based on class
            if (botClass.classType == ClassType.Support)
            {
                // Support bots prioritize healing allies
                HealNearbyAllies();
            }
            else
            {
                // Attack the target
                if (attackCooldownTimer <= 0)
                {
                    AttackTarget();
                    attackCooldownTimer = botClass.attackCooldown;
                }
            }
            
            // Use ultimate when available
            if (ultimateCooldownTimer <= 0)
            {
                UseUltimateAbility();
                ultimateCooldownTimer = botClass.ultimateCooldown;
            }
        }
    }
    
    private void Wander()
    {
        wanderTimer -= Time.deltaTime;
        
        if (wanderTimer <= 0)
        {
            SetNewWanderTarget();
        }
        
        MoveTowards(wanderTarget);
    }
    
    private void SetNewWanderTarget()
    {
        wanderTarget = transform.position + new Vector3(
            Random.Range(-10f, 10f),
            0,
            Random.Range(-10f, 10f)
        );
        wanderTimer = Random.Range(3f, 6f);
    }
    
    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 move = direction * currentSpeed * Time.deltaTime;
        
        if (characterController != null)
        {
            characterController.Move(move);
        }
        else
        {
            transform.Translate(move, Space.World);
        }
        
        // Face target
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    private void AttackTarget()
    {
        switch (botClass.basicAbilityType)
        {
            case AbilityType.MeleeAttack:
                PerformMeleeAttack();
                break;
            case AbilityType.RangedAttack:
                PerformRangedAttack();
                break;
        }
    }
    
    private void PerformMeleeAttack()
    {
        if (currentTarget != null)
        {
            var player = currentTarget.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
            }
        }
    }
    
    private void PerformRangedAttack()
    {
        Debug.Log($"Bot {botClass.className} performed ranged attack!");
        // Implement projectile spawning here
    }
    
    private void HealNearbyAllies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, botClass.abilityRange);
        foreach (var hit in hits)
        {
            var botAlly = hit.GetComponent<BotController>();
            if (botAlly != null && botAlly != this && botAlly.CurrentHP < botAlly.MaxHP)
            {
                botAlly.Heal(botClass.abilityValue);
                break; // Heal one at a time
            }
        }
    }
    
    private void UseUltimateAbility()
    {
        switch (botClass.ultimateAbilityType)
        {
            case AbilityType.KnockbackAOE:
                PerformKnockbackAOE();
                break;
            case AbilityType.HealAOE:
                PerformHealAOE();
                break;
        }
    }
    
    private void PerformHealAOE()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, botClass.abilityRange);
        foreach (var hit in hits)
        {
            var botAlly = hit.GetComponent<BotController>();
            if (botAlly != null)
            {
                botAlly.Heal(botClass.abilityValue);
            }
        }
        Debug.Log($"Bot {botClass.className} healed all allies!");
    }
    
    private void PerformKnockbackAOE()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, botClass.abilityRange);
        foreach (var hit in hits)
        {
            var player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                Vector3 knockbackDir = (player.transform.position - transform.position).normalized;
                // Apply knockback to player (player needs to handle this)
            }
        }
        Debug.Log($"Bot {botClass.className} knocked back enemies!");
    }
    
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        
        if (currentHP <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, botClass.baseHP);
    }
    
    public void ApplyKnockback(Vector3 knockbackForce)
    {
        if (characterController != null)
        {
            characterController.Move(knockbackForce * Time.deltaTime);
        }
        else
        {
            transform.position += knockbackForce * Time.deltaTime;
        }
    }
    
    private void Die()
    {
        Debug.Log($"Bot {botClass.className} has been defeated!");
        gameObject.SetActive(false);
    }
}
