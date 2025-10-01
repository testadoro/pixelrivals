using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Class Configuration")]
    public PlayerClassSO playerClass;
    public int selectedSkinIndex = 0;
    
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
    
    public float CurrentHP => currentHP;
    public float MaxHP => playerClass.baseHP;
    public PlayerClassSO PlayerClass => playerClass;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    
    private void Start()
    {
        if (playerClass != null)
        {
            InitializeStats();
            ApplySkin();
        }
    }
    
    public void SetPlayerClass(PlayerClassSO classData, int skinIndex = 0)
    {
        playerClass = classData;
        selectedSkinIndex = skinIndex;
        InitializeStats();
        ApplySkin();
    }
    
    private void InitializeStats()
    {
        currentHP = playerClass.baseHP;
        currentSpeed = playerClass.baseSpeed;
        attackDamage = playerClass.baseAttack;
        attackCooldownTimer = 0f;
        ultimateCooldownTimer = 0f;
    }
    
    private void ApplySkin()
    {
        if (playerClass.availableSkins != null && 
            selectedSkinIndex >= 0 && 
            selectedSkinIndex < playerClass.availableSkins.Length)
        {
            var skin = playerClass.availableSkins[selectedSkinIndex];
            
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
        HandleMovement();
        UpdateCooldowns();
        HandleAbilities();
    }
    
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 move = new Vector3(horizontal, 0, vertical) * currentSpeed * Time.deltaTime;
        
        if (characterController != null)
        {
            characterController.Move(move);
        }
        else
        {
            transform.Translate(move, Space.World);
        }
    }
    
    private void UpdateCooldowns()
    {
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        
        if (ultimateCooldownTimer > 0)
            ultimateCooldownTimer -= Time.deltaTime;
    }
    
    private void HandleAbilities()
    {
        // Basic ability (left click or button)
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
        {
            UseBasicAbility();
            attackCooldownTimer = playerClass.attackCooldown;
        }
        
        // Ultimate ability (right click or button)
        if (Input.GetMouseButtonDown(1) && ultimateCooldownTimer <= 0)
        {
            UseUltimateAbility();
            ultimateCooldownTimer = playerClass.ultimateCooldown;
        }
    }
    
    private void UseBasicAbility()
    {
        switch (playerClass.basicAbilityType)
        {
            case AbilityType.MeleeAttack:
                PerformMeleeAttack();
                break;
            case AbilityType.RangedAttack:
                PerformRangedAttack();
                break;
            case AbilityType.HealCone:
                PerformHealCone();
                break;
        }
    }
    
    private void UseUltimateAbility()
    {
        switch (playerClass.ultimateAbilityType)
        {
            case AbilityType.KnockbackAOE:
                PerformKnockbackAOE();
                break;
            case AbilityType.HealAOE:
                PerformHealAOE();
                break;
            case AbilityType.RangedAttack:
                PerformRangedAttack();
                break;
        }
    }
    
    private void PerformMeleeAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 2f, 1.5f);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<BotController>();
            if (enemy != null && enemy != this)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
        Debug.Log($"{playerClass.className} performed melee attack!");
    }
    
    private void PerformRangedAttack()
    {
        Debug.Log($"{playerClass.className} performed ranged attack!");
        // Implement projectile spawning here
    }
    
    private void PerformHealCone()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * playerClass.abilityRange * 0.5f, playerClass.abilityRange);
        foreach (var hit in hits)
        {
            var ally = hit.GetComponent<PlayerController>();
            var botAlly = hit.GetComponent<BotController>();
            
            if (ally != null)
            {
                ally.Heal(playerClass.abilityValue);
            }
            else if (botAlly != null)
            {
                botAlly.Heal(playerClass.abilityValue);
            }
        }
        Debug.Log($"{playerClass.className} healed allies in cone!");
    }
    
    private void PerformHealAOE()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, playerClass.abilityRange);
        foreach (var hit in hits)
        {
            var ally = hit.GetComponent<PlayerController>();
            var botAlly = hit.GetComponent<BotController>();
            
            if (ally != null)
            {
                ally.Heal(playerClass.abilityValue);
            }
            else if (botAlly != null)
            {
                botAlly.Heal(playerClass.abilityValue);
            }
        }
        Debug.Log($"{playerClass.className} healed all allies in AoE!");
    }
    
    private void PerformKnockbackAOE()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, playerClass.abilityRange);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<BotController>();
            if (enemy != null)
            {
                Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                enemy.ApplyKnockback(knockbackDir * 10f);
            }
        }
        Debug.Log($"{playerClass.className} knocked back enemies!");
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
        currentHP = Mathf.Min(currentHP, playerClass.baseHP);
    }
    
    private void Die()
    {
        Debug.Log($"{playerClass.className} has been defeated!");
        // Handle death logic
    }
}
