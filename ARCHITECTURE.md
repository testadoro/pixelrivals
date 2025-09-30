# PixelRivals - Architecture Overview

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      GAME SCENE                             │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────────┐         ┌──────────────┐                │
│  │ GameManager  │────────▶│   BotAI (×3) │                │
│  │              │         │   Enemy Team │                │
│  │ - Spawning   │         └──────────────┘                │
│  │ - Match Time │                                          │
│  └──────────────┘         ┌──────────────┐                │
│                           │ ConquestZone │                │
│                           │              │                │
│  ┌──────────────┐         │ - Capture    │                │
│  │    Player    │◀────────│ - Teams      │                │
│  │  Controller  │         │ - Visual     │                │
│  └──────┬───────┘         └──────────────┘                │
│         │                                                  │
│         │ Fires                                           │
│         ▼                                                  │
│  ┌──────────────┐                                         │
│  │ Projectile   │                                         │
│  │              │                                         │
│  │ - Movement   │                                         │
│  │ - Collision  │                                         │
│  │ - Damage     │                                         │
│  └──────────────┘                                         │
│                                                             │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                        UI LAYER                             │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────────┐         ┌──────────────┐                │
│  │   GameHUD    │────────▶│ PlayerController              │
│  │              │         └──────────────┘                │
│  │ - Health Bar │                                          │
│  │ - Cooldowns  │         ┌──────────────┐                │
│  │ - Zone Info  │────────▶│ConquestZone  │                │
│  └──────────────┘         └──────────────┘                │
│                                                             │
│  ┌──────────────┐                                          │
│  │VirtualJoystick────────▶│ Player Movement │             │
│  │              │                                          │
│  │ - Touch Input│                                          │
│  │ - Direction  │                                          │
│  └──────────────┘                                          │
│                                                             │
│  ┌──────────────┐                                          │
│  │Action Buttons│────────▶│ Player Combat │               │
│  │              │                                          │
│  │ - Attack     │                                          │
│  │ - Special    │                                          │
│  └──────────────┘                                          │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## Component Descriptions

### Core Game Components

#### PlayerController
- **Purpose**: Main player character controller
- **Features**:
  - Movement with virtual joystick input
  - Attack system with cooldown
  - Special ability (triple shot) with longer cooldown
  - Health management
  - Character rotation toward movement direction
- **Dependencies**: CharacterController, VirtualJoystick, Projectile prefab

#### BotAI
- **Purpose**: Autonomous enemy AI
- **Behaviors**:
  - **Patrol**: Random wandering when no target
  - **Chase**: Move toward player within detection range
  - **Attack**: Fire projectiles at player within attack range
- **Parameters**:
  - Detection Range: 15 units
  - Attack Range: 10 units
  - Move Speed: 3 units/sec
  - Attack Cooldown: 1 second
- **Dependencies**: CharacterController, Projectile prefab

#### Projectile
- **Purpose**: Damage-dealing projectile
- **Features**:
  - Linear movement
  - Team-based collision (doesn't hit owner)
  - Auto-destroy after lifetime
  - Damage on hit
- **Parameters**:
  - Speed: 15 units/sec
  - Damage: 10 HP
  - Lifetime: 5 seconds

#### ConquestZone
- **Purpose**: Capturable objective
- **Mechanics**:
  - Capture progress from -100 (Enemy) to +100 (Player)
  - Captured by standing in zone
  - Visual feedback through material changes
  - Contest mechanic (both teams cancel each other)
- **States**: Neutral, Player-controlled, Enemy-controlled

#### GameManager
- **Purpose**: Match setup and management
- **Responsibilities**:
  - Spawn bots at designated points
  - Track match time
  - End match conditions
- **Configuration**:
  - Bots per team: 3
  - Match duration: 300 seconds (5 minutes)

### UI Components

#### VirtualJoystick
- **Purpose**: Touch-based movement input
- **Implementation**:
  - Drag to move handle
  - Returns normalized 2D vector
  - Visual feedback with handle position
- **Position**: Bottom-left corner

#### GameHUD
- **Purpose**: Main UI controller
- **Displays**:
  - Player health bar and text
  - Special ability cooldown (radial fill)
  - Conquest zone status and progress
  - Button states (enabled/disabled based on cooldowns)
- **Updates**: Every frame for real-time feedback

## Data Flow

### Input Flow
```
Touch Input → VirtualJoystick → PlayerController → CharacterController → Player Movement
Touch Input → Button → PlayerController.Attack() → Instantiate Projectile
```

### Combat Flow
```
Attack Button → PlayerController.Attack()
                ↓
            Check Cooldown
                ↓
        Instantiate Projectile
                ↓
        Projectile.Initialize(direction, "Player")
                ↓
        Projectile Movement (Update)
                ↓
        OnTriggerEnter(Collider)
                ↓
        Target.TakeDamage(damage)
                ↓
        Destroy Projectile
```

### AI Flow
```
BotAI.Update()
    ↓
FindTarget() → Detect Player in Range
    ↓
├─ If Target Found:
│   ├─ In Attack Range → AttackTarget()
│   │                    ├─ Face Target
│   │                    └─ Fire Projectile
│   └─ In Detection Range → MoveTowardsTarget()
│                          └─ Chase Player
└─ If No Target:
    └─ Patrol() → Random Wandering
```

### Conquest Zone Flow
```
Player/Bot Enters Zone (OnTriggerEnter)
    ↓
Increment Team Counter
    ↓
Update() → Calculate Capture Progress
    ↓
├─ More Players: Progress += captureSpeed
├─ More Enemies: Progress -= captureSpeed
└─ Equal/None: No change
    ↓
Update Visual Material
    ↓
├─ Progress > 50: Player Material (Blue)
├─ Progress < -50: Enemy Material (Red)
└─ Otherwise: Neutral Material (Gray)
```

## Mobile Optimization Considerations

### Performance
- Character Controller used instead of Rigidbody for better performance
- Simple AI behaviors to reduce CPU usage
- Limited number of simultaneous projectiles
- Object pooling recommended for projectiles (not yet implemented)

### Touch Controls
- Large touch areas for joystick and buttons
- Visual feedback on all interactions
- Bottom UI placement for thumb accessibility
- No overlapping interactive elements

### Screen Space
- HUD elements anchored to screen edges
- Responsive UI scaling with Canvas Scaler
- Important information (health, zone) at top
- Controls at bottom for easy reach

## Extensibility Points

### Easy to Add
1. **New Abilities**: Add methods to PlayerController, wire to UI buttons
2. **More Bot Types**: Extend BotAI or create new classes
3. **Power-ups**: Implement with trigger colliders
4. **Team System**: Extend for 3v3 with friendly bots
5. **Scoring**: Add score tracking to GameManager
6. **Visual Effects**: Add particle systems to projectiles/abilities

### Requires More Work
1. **Networking**: Would need complete rewrite for multiplayer
2. **Save System**: Add PlayerPrefs or file-based saves
3. **Multiple Arenas**: Scene management system
4. **Character Customization**: Asset loading system
5. **Advanced AI**: Behavior trees or state machines

## Testing Checklist

- [ ] Player movement responds to joystick
- [ ] Attack button fires projectiles
- [ ] Special ability uses 3-projectile spread
- [ ] Cooldowns work correctly
- [ ] Player takes damage from bot projectiles
- [ ] Bots patrol when no target
- [ ] Bots chase and attack player
- [ ] Conquest zone captures correctly
- [ ] Zone visual updates based on control
- [ ] Health bar updates in real-time
- [ ] UI scales properly on different resolutions
- [ ] Touch controls work on mobile device

## Known Limitations

1. **No .meta files**: Unity will generate these when project is opened
2. **No prefabs**: Must be created manually in Unity Editor
3. **No materials**: Default materials will be used until created
4. **No scene file**: Scene must be set up following SETUP_GUIDE.txt
5. **Basic AI**: Bots use simple patrol/chase/attack logic
6. **No object pooling**: New projectiles created each time
7. **No audio**: No sound effects or music
8. **No animations**: Static models without animation
9. **No team system**: Only player vs bots implemented
10. **No progression**: No levels, upgrades, or unlocks
