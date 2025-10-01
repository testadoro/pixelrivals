using UnityEngine;

/// <summary>
/// Simple test script to verify the class system is working correctly.
/// Attach this to an empty GameObject in your scene to run tests.
/// </summary>
public class ClassSystemTest : MonoBehaviour
{
    [Header("Test Configuration")]
    public bool runTestsOnStart = true;
    
    private void Start()
    {
        if (runTestsOnStart)
        {
            RunAllTests();
        }
    }
    
    [ContextMenu("Run All Tests")]
    public void RunAllTests()
    {
        Debug.Log("=== Starting Class System Tests ===");
        
        TestLoadClasses();
        TestClassStats();
        TestSkinData();
        
        Debug.Log("=== All Tests Complete ===");
    }
    
    private void TestLoadClasses()
    {
        Debug.Log("--- Test: Load Classes from Resources ---");
        
        PlayerClassSO tank = Resources.Load<PlayerClassSO>("Classes/Tank");
        PlayerClassSO dps = Resources.Load<PlayerClassSO>("Classes/DPS");
        PlayerClassSO support = Resources.Load<PlayerClassSO>("Classes/Support");
        
        if (tank != null)
            Debug.Log($"✓ Tank loaded successfully: {tank.className}");
        else
            Debug.LogError("✗ Failed to load Tank");
        
        if (dps != null)
            Debug.Log($"✓ DPS loaded successfully: {dps.className}");
        else
            Debug.LogError("✗ Failed to load DPS");
        
        if (support != null)
            Debug.Log($"✓ Support loaded successfully: {support.className}");
        else
            Debug.LogError("✗ Failed to load Support");
    }
    
    private void TestClassStats()
    {
        Debug.Log("--- Test: Verify Class Stats ---");
        
        PlayerClassSO tank = Resources.Load<PlayerClassSO>("Classes/Tank");
        if (tank != null)
        {
            Debug.Log($"Tank Stats - HP: {tank.baseHP}, Attack: {tank.baseAttack}, Speed: {tank.baseSpeed}");
            
            // Verify Tank requirements
            if (tank.baseHP == 150f)
                Debug.Log("✓ Tank HP correct (150)");
            else
                Debug.LogWarning($"✗ Tank HP incorrect. Expected 150, got {tank.baseHP}");
            
            if (tank.baseSpeed == 4.25f)
                Debug.Log("✓ Tank speed correct (4.25, -15% from base 5)");
            else
                Debug.LogWarning($"✗ Tank speed incorrect. Expected 4.25, got {tank.baseSpeed}");
            
            if (tank.ultimateAbilityType == AbilityType.KnockbackAOE)
                Debug.Log("✓ Tank ultimate correct (Knockback AoE)");
            else
                Debug.LogWarning($"✗ Tank ultimate incorrect. Expected KnockbackAOE, got {tank.ultimateAbilityType}");
        }
        
        PlayerClassSO dps = Resources.Load<PlayerClassSO>("Classes/DPS");
        if (dps != null)
        {
            Debug.Log($"DPS Stats - HP: {dps.baseHP}, Attack: {dps.baseAttack}, Speed: {dps.baseSpeed}");
            
            // Verify DPS requirements
            if (dps.baseAttack == 30f)
                Debug.Log("✓ DPS attack correct (30)");
            else
                Debug.LogWarning($"✗ DPS attack incorrect. Expected 30, got {dps.baseAttack}");
            
            if (dps.attackCooldown == 0.8f)
                Debug.Log("✓ DPS cooldown correct (0.8s, -20% from base 1.0)");
            else
                Debug.LogWarning($"✗ DPS cooldown incorrect. Expected 0.8, got {dps.attackCooldown}");
        }
        
        PlayerClassSO support = Resources.Load<PlayerClassSO>("Classes/Support");
        if (support != null)
        {
            Debug.Log($"Support Stats - HP: {support.baseHP}, Attack: {support.baseAttack}, Speed: {support.baseSpeed}");
            
            // Verify Support requirements
            if (support.basicAbilityType == AbilityType.HealCone)
                Debug.Log("✓ Support basic ability correct (Heal Cone)");
            else
                Debug.LogWarning($"✗ Support basic ability incorrect. Expected HealCone, got {support.basicAbilityType}");
            
            if (support.ultimateAbilityType == AbilityType.HealAOE)
                Debug.Log("✓ Support ultimate correct (Heal AoE)");
            else
                Debug.LogWarning($"✗ Support ultimate incorrect. Expected HealAOE, got {support.ultimateAbilityType}");
            
            if (support.abilityValue == 20f)
                Debug.Log("✓ Support heal amount correct (20 HP)");
            else
                Debug.LogWarning($"✗ Support heal amount incorrect. Expected 20, got {support.abilityValue}");
        }
    }
    
    private void TestSkinData()
    {
        Debug.Log("--- Test: Verify Skin Data ---");
        
        PlayerClassSO[] classes = new PlayerClassSO[]
        {
            Resources.Load<PlayerClassSO>("Classes/Tank"),
            Resources.Load<PlayerClassSO>("Classes/DPS"),
            Resources.Load<PlayerClassSO>("Classes/Support")
        };
        
        foreach (var classData in classes)
        {
            if (classData == null) continue;
            
            if (classData.availableSkins != null && classData.availableSkins.Length >= 2)
            {
                Debug.Log($"✓ {classData.className} has {classData.availableSkins.Length} skins:");
                foreach (var skin in classData.availableSkins)
                {
                    Debug.Log($"  - {skin.skinName}");
                }
            }
            else
            {
                Debug.LogWarning($"✗ {classData.className} should have 2-3 skins");
            }
        }
    }
}
