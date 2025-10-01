# PixelRivals - Quick Reference Card

## 🎮 Project Overview
Unity URP mobile 3v3 arena game prototype with conquest zone mechanics, AI bots, and touch controls.

## 📁 Project Structure
```
pixelrivals/
├── Assets/
│   ├── Scripts/          # All C# game scripts
│   │   ├── PlayerController.cs
│   │   ├── BotAI.cs
│   │   ├── Projectile.cs
│   │   ├── ConquestZone.cs
│   │   ├── VirtualJoystick.cs
│   │   ├── GameHUD.cs
│   │   ├── GameManager.cs
│   │   └── SetupValidator.cs
│   ├── SETUP_GUIDE.txt   # Detailed setup instructions
│   └── PREFAB_CONFIGS.json  # Prefab specifications
├── ProjectSettings/      # Unity project configuration
├── Packages/            # Package dependencies
├── README.md            # Project documentation
├── ARCHITECTURE.md      # System design documentation
├── DEVELOPMENT_NOTES.md # Developer guidelines
└── CHANGELOG.md         # Version history
```

## 🎯 Core Components

### PlayerController
**Purpose**: Player character control with mobile input
**Key Features**:
- Virtual joystick movement
- Basic attack (0.5s cooldown)
- Special ability - triple shot (5s cooldown)
- Health system (100 HP)
**Methods**:
- `SetMoveDirection(Vector2)` - Set movement from joystick
- `Attack()` - Fire single projectile
- `UseSpecialAbility()` - Fire 3-projectile spread
- `TakeDamage(int)` - Apply damage

### BotAI
**Purpose**: Enemy AI behavior
**States**:
1. **Patrol** - Wander randomly
2. **Chase** - Follow player (detection range: 15 units)
3. **Attack** - Fire at player (attack range: 10 units)
**Stats**:
- Health: 80 HP
- Move Speed: 3 units/sec
- Attack Cooldown: 1 second

### Projectile
**Purpose**: Damage-dealing projectile
**Stats**:
- Speed: 15 units/sec
- Damage: 10 HP
- Lifetime: 5 seconds
**Features**: Team-based collision, auto-destroy

### ConquestZone
**Purpose**: Capturable objective
**Mechanics**:
- Progress range: -100 (Enemy) to +100 (Player)
- Capture speed: 1 point/second per player
- Visual feedback via material colors
**States**: Neutral (Gray), Player (Blue), Enemy (Red)

### VirtualJoystick
**Purpose**: Touch-based movement input
**Implementation**: Drag handle to move, returns normalized 2D vector
**Position**: Bottom-left of screen

### GameHUD
**Purpose**: UI controller
**Displays**:
- Player health bar
- Special ability cooldown (radial fill)
- Conquest zone status bar
- Attack/Special buttons

### GameManager
**Purpose**: Match setup and management
**Features**:
- Bot spawning (3 per team)
- Match timer (5 minutes)
- Spawn point management

## 🛠️ Setup Quick Steps

1. **Open Project**: Unity Hub → Add → Select folder
2. **Create Scene**: File → New Scene → URP
3. **Add Ground**: Create Plane (10x10), tag "Environment"
4. **Add Player**: 
   - Capsule, tag "Player", add PlayerController
   - Child "FirePoint" at (0, 1, 1)
5. **Add Zone**: 
   - Cylinder (5x0.5x5), add ConquestZone
   - BoxCollider (Is Trigger = true)
6. **Create Bot Prefab**: 
   - Capsule, add BotAI
   - Child "FirePoint"
7. **Create Projectile Prefab**: 
   - Sphere (0.2 scale), add Projectile
   - Rigidbody (Kinematic), SphereCollider (Trigger)
8. **Setup UI**: 
   - Canvas with joystick, buttons, health bar
   - Add GameHUD, connect references

## 📱 Mobile Controls

| Control | Location | Function |
|---------|----------|----------|
| Virtual Joystick | Bottom-Left | Player movement |
| Attack Button | Bottom-Right | Fire projectile |
| Special Button | Above Attack | Triple shot (5s cooldown) |

## ⚙️ Key Parameters

### Player Settings
```
moveSpeed = 5.0
rotationSpeed = 10.0
attackCooldown = 0.5
specialAbilityCooldown = 5.0
maxHealth = 100
```

### Bot Settings
```
moveSpeed = 3.0
rotationSpeed = 5.0
attackRange = 10.0
attackCooldown = 1.0
detectionRange = 15.0
maxHealth = 80
```

### Projectile Settings
```
speed = 15.0
damage = 10
lifetime = 5.0
```

## 🐛 Debug Tools

### SetupValidator
**Usage**: Attach to GameObject → Right-click → "Validate Scene Setup"
**Checks**:
- Player configuration
- Bot setup
- Conquest zone
- UI components
- Camera presence

### Console Commands
```csharp
Debug.Log("message");           // Normal log
Debug.LogWarning("warning");    // Warning (yellow)
Debug.LogError("error");        // Error (red)
```

## 🔧 Common Tasks

### Change Player Speed
```csharp
// In PlayerController.cs
[SerializeField] private float moveSpeed = 5f; // Change this value
```

### Add New Ability
```csharp
// In PlayerController.cs
public void NewAbility() {
    // Your code here
}
// Wire to UI button in GameHUD
```

### Modify Bot Behavior
```csharp
// In BotAI.cs Update() method
// Adjust detection ranges or add new behaviors
```

## 📋 Pre-Flight Checklist

Before testing:
- [ ] Player has tag "Player"
- [ ] Player has PlayerController + CharacterController
- [ ] Player has FirePoint child
- [ ] Bot prefab has BotAI + FirePoint
- [ ] Projectile prefab has Projectile + trigger collider
- [ ] ConquestZone has trigger collider
- [ ] Canvas has EventSystem
- [ ] VirtualJoystick configured
- [ ] GameHUD references connected
- [ ] Bot and Projectile prefabs assigned

## 🚀 Build Process

### Android
1. File → Build Settings → Android
2. Switch Platform
3. Player Settings:
   - Set Bundle Identifier
   - Min API: 22
   - Target API: 30+
4. Build and Run

### iOS
1. File → Build Settings → iOS
2. Switch Platform
3. Player Settings:
   - Set Bundle Identifier
   - Min iOS: 11.0
4. Build (requires Xcode)

## 📚 Documentation Files

| File | Purpose |
|------|---------|
| README.md | Project overview and features |
| SETUP_GUIDE.txt | Step-by-step scene setup |
| ARCHITECTURE.md | System design and flow diagrams |
| DEVELOPMENT_NOTES.md | Developer guidelines and tips |
| CHANGELOG.md | Version history |
| PREFAB_CONFIGS.json | Prefab specifications |

## 🔗 Unity Packages Required

- Universal RP: 14.0.8
- TextMeshPro: 3.0.6
- Input System: 1.6.3 (optional)

## ⚡ Performance Tips

1. Use object pooling for projectiles
2. Limit bot count (3-5 recommended)
3. Keep polygon count low on models
4. Use URP optimized shaders
5. Target 60 FPS on mid-range devices

## 🎨 Visual Conventions

- **Player**: Blue color
- **Enemy**: Red color
- **Neutral Zone**: Gray color
- **Captured Zone**: Blue/Red based on control
- **Projectile**: Yellow/emissive

## 🆘 Troubleshooting

| Issue | Solution |
|-------|----------|
| Player won't move | Check VirtualJoystick connection |
| No projectiles | Verify prefab assignment and FirePoint |
| Bots don't spawn | Check GameManager configuration |
| UI not working | Verify EventSystem exists |
| Collisions fail | Check layer collision matrix |

## 📞 Need Help?

1. Run SetupValidator script
2. Check Unity Console for errors
3. Review SETUP_GUIDE.txt
4. Consult ARCHITECTURE.md for system design
5. Check DEVELOPMENT_NOTES.md for common issues

---

**Version**: 0.1.0  
**Unity**: 2022.3.10f1+  
**Platform**: Mobile (Android/iOS)  
**Render Pipeline**: URP 14.0.8
