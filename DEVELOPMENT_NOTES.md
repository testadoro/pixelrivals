# Development Notes

## Quick Start for Developers

### Prerequisites
- Unity 2022.3.10f1 or later
- Basic knowledge of Unity Editor
- Understanding of C# scripting
- Familiarity with mobile game development

### First Time Setup
1. Clone the repository
2. Open Unity Hub
3. Add project from disk
4. Open project (Unity will generate .meta files)
5. Follow SETUP_GUIDE.txt to create the scene
6. Press Play to test

### Development Workflow

#### Testing in Editor
1. **Keyboard Controls** (for testing without mobile device):
   - Arrow keys / WASD: Movement (if you add keyboard input to PlayerController)
   - Mouse clicks: Can click UI buttons
   - Current setup requires UI interaction

2. **Mobile Testing**:
   - Use Unity Remote for quick mobile testing
   - Build to device for full testing
   - Use touch simulation in editor (Unity > Project Settings > Input System Package)

3. **Debug Tools**:
   - Add SetupValidator script to scene to validate configuration
   - Use Debug.Log statements in scripts
   - Unity Console shows all logs and errors

#### Making Changes

##### Adding New Abilities
```csharp
// In PlayerController.cs
public void NewAbility()
{
    if (Time.time - lastAbilityTime < cooldownTime) return;
    lastAbilityTime = Time.time;
    
    // Your ability code here
}

// Wire to UI button in GameHUD
newAbilityButton.onClick.AddListener(() => player?.NewAbility());
```

##### Modifying Bot Behavior
```csharp
// In BotAI.cs Update() method
// Add new behavior states in the decision tree
if (someCondition)
{
    NewBehavior();
}
```

##### Adding Power-ups
```csharp
// Create new PowerUp.cs script
public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType type;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            ApplyPowerUp(player);
            Destroy(gameObject);
        }
    }
}
```

### Code Style Guidelines

1. **Naming Conventions**:
   - Classes: PascalCase (PlayerController)
   - Methods: PascalCase (TakeDamage)
   - Private fields: camelCase (currentHealth)
   - Serialized fields: camelCase with [SerializeField]
   - Constants: PascalCase or UPPER_CASE

2. **Comments**:
   - Use XML documentation for public methods
   - Comment complex logic
   - Keep comments up to date

3. **Organization**:
   - Group related fields with [Header("Group Name")]
   - Order: Serialized fields → Private fields → Properties → Unity methods → Public methods → Private methods

### Performance Optimization Tips

#### For Mobile
1. **Reduce Draw Calls**:
   - Use static batching for static objects
   - Use dynamic batching for small moving objects
   - Combine meshes where possible

2. **Optimize Physics**:
   - Use layers to reduce collision checks
   - Keep Rigidbody count low
   - Use trigger colliders instead of collision where possible

3. **Script Optimization**:
   - Cache component references in Awake/Start
   - Avoid GetComponent in Update
   - Use object pooling for frequently spawned objects
   - Minimize allocations in Update loop

4. **UI Optimization**:
   - Minimize overdraw
   - Use Canvas groups for batch enabling/disabling
   - Separate static and dynamic UI elements

#### Object Pooling Example
```csharp
// ProjectilePool.cs
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 20;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
    public GameObject GetProjectile()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(projectilePrefab);
    }
    
    public void ReturnProjectile(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

### Testing Checklist

#### Functional Testing
- [ ] Player movement works with joystick
- [ ] Attack button spawns projectiles correctly
- [ ] Special ability fires 3 projectiles
- [ ] Cooldowns prevent spam
- [ ] Player takes damage from bot projectiles
- [ ] Player health bar updates correctly
- [ ] Bots patrol when no target visible
- [ ] Bots chase player when in range
- [ ] Bots attack player at close range
- [ ] Conquest zone captures for player
- [ ] Conquest zone captures for bots
- [ ] Zone visual changes based on control
- [ ] Match timer counts down correctly

#### Performance Testing
- [ ] Frame rate stable at 60 FPS on target device
- [ ] No significant frame drops during combat
- [ ] Memory usage remains stable over time
- [ ] No memory leaks detected
- [ ] Battery drain is acceptable
- [ ] Device doesn't overheat

#### Mobile-Specific Testing
- [ ] Touch controls responsive
- [ ] Joystick works smoothly
- [ ] Buttons are easy to tap
- [ ] UI scales properly on different resolutions
- [ ] UI readable on small screens
- [ ] Portrait and landscape modes work (if supported)
- [ ] Works on various screen aspect ratios
- [ ] Pause/resume works correctly
- [ ] Handles interruptions (calls, notifications)

### Common Issues and Solutions

#### Issue: Player doesn't move
**Solutions**:
- Check VirtualJoystick is connected to PlayerController
- Verify CharacterController component exists
- Check ground collision (player may be falling)
- Ensure Canvas has EventSystem

#### Issue: Projectiles don't hit anything
**Solutions**:
- Verify projectile has trigger collider
- Check projectile speed isn't too high
- Verify target has collider
- Check layer collision matrix

#### Issue: Bots don't spawn
**Solutions**:
- Verify GameManager has bot prefab assigned
- Check spawn points are assigned
- Look for errors in console
- Verify bot prefab has all required components

#### Issue: UI doesn't respond
**Solutions**:
- Check EventSystem exists in scene
- Verify Canvas raycaster is present
- Check UI elements are on top (higher in hierarchy)
- Verify buttons have correct interactable settings

#### Issue: Performance is poor
**Solutions**:
- Profile with Unity Profiler
- Check for excessive Draw Calls
- Reduce polygon count on models
- Optimize physics calculations
- Implement object pooling
- Reduce shadow quality
- Lower texture resolution

### Git Workflow

1. **Branch Naming**:
   - feature/description
   - bugfix/description
   - hotfix/description

2. **Commit Messages**:
   - Use descriptive messages
   - Reference issue numbers when applicable
   - Format: "Type: Description"
   - Example: "Feature: Add dash ability to player"

3. **Before Committing**:
   - Test your changes
   - Remove debug code
   - Update documentation if needed
   - Check .gitignore catches Unity temp files

### Build Settings

#### Android Build
1. File > Build Settings
2. Select Android
3. Switch Platform
4. Player Settings:
   - Set Company Name
   - Set Product Name
   - Set Bundle Identifier (com.company.pixelrivals)
   - Set Minimum API Level: 22 (Android 5.1)
   - Set Target API Level: 30+
   - Graphics API: OpenGL ES 3.0 and Vulkan
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64

#### iOS Build
1. File > Build Settings
2. Select iOS
3. Switch Platform
4. Player Settings:
   - Set Bundle Identifier
   - Set Target Minimum iOS Version: 11.0
   - Set Camera Usage Description (if needed)
   - Set Location Usage Description (if needed)
   - Enable Requires Persistent WiFi: No
   - Set Target Device: iPhone + iPad or iPhone only

### Useful Unity Commands

```csharp
// Finding objects efficiently
GameObject player = GameObject.FindGameObjectWithTag("Player");
PlayerController controller = FindObjectOfType<PlayerController>();

// Instantiating with parent
GameObject obj = Instantiate(prefab, parent);

// Delayed destruction
Destroy(gameObject, 2f);

// Coroutine for delays
StartCoroutine(DelayedAction());

IEnumerator DelayedAction()
{
    yield return new WaitForSeconds(1f);
    // Code here runs after 1 second
}

// Screen space to world space
Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

// Distance check
float distance = Vector3.Distance(pointA, pointB);

// Lerp for smooth transitions
transform.position = Vector3.Lerp(start, end, t);
```

### Resources

- Unity Documentation: https://docs.unity3d.com/
- URP Documentation: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest
- Mobile Optimization Guide: Unity Manual > Mobile Optimization
- Input System: Unity Manual > Input System Package

### Future Enhancements

Priority list for expanding the prototype:

1. **Immediate**:
   - Add object pooling for projectiles
   - Implement respawn system
   - Add basic audio feedback
   - Create simple particle effects

2. **Short Term**:
   - Add team system for 3v3
   - Implement scoring system
   - Add match victory conditions
   - Create main menu and HUD polish

3. **Medium Term**:
   - Add character selection
   - Implement multiple arenas
   - Add power-ups system
   - Create progression system

4. **Long Term**:
   - Add networked multiplayer
   - Implement matchmaking
   - Add cosmetic customization
   - Create seasonal content system

### Contact and Support

For questions or issues:
- Check SETUP_GUIDE.txt for scene setup
- Review ARCHITECTURE.md for system design
- Check Unity Console for error messages
- Use SetupValidator script to check configuration
- Consult Unity documentation for Unity-specific issues
