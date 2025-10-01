# PixelRivals - System Architecture

## Overview
PixelRivals implements a modular, data-driven class system for a 3v3 mobile arena game using Unity's ScriptableObject pattern.

## Core Design Principles

### 1. Modularity
- Each class is defined as a standalone ScriptableObject asset
- Easy to add new classes without code changes
- Stats can be tweaked in Unity Inspector without recompiling

### 2. Data-Driven Design
- All gameplay stats stored in ScriptableObjects
- Controllers read from ScriptableObjects at runtime
- Same controller code works for all classes

### 3. Separation of Concerns
- **Data Layer**: PlayerClassSO (stats, abilities, skins)
- **Logic Layer**: PlayerController, BotController (behavior)
- **Presentation Layer**: UI scripts, HUD (display)
- **Management Layer**: GameManager (orchestration)

## Component Breakdown

### Data Layer: PlayerClassSO
```
PlayerClassSO (ScriptableObject)
├── Class Info (name, icon, type)
├── Base Stats (HP, attack, speed, cooldowns)
├── Ability Info (types, range, values)
└── Available Skins (mesh, material variants)
```

**Responsibilities:**
- Store immutable class configuration
- Provide data to runtime components
- Enable designer-friendly editing

**Why ScriptableObject?**
- Memory efficient (shared reference, not copied)
- Unity Inspector integration
- Asset-based workflow
- Easy serialization

### Logic Layer: Controllers

#### PlayerController
```
PlayerController (MonoBehaviour)
├── References PlayerClassSO
├── Handles player input
├── Manages runtime stats (current HP, cooldowns)
├── Executes abilities based on class type
└── Applies selected skin
```

**Key Features:**
- Input handling (WASD movement, mouse abilities)
- Ability execution based on class's ability types
- Runtime stat management (HP, cooldowns)
- Skin application on initialization

#### BotController
```
BotController (MonoBehaviour)
├── References PlayerClassSO
├── AI state machine
├── Class-specific AI behavior
├── Same ability system as player
└── Applies skin like player
```

**AI Behaviors:**
- **Tank/DPS**: Aggressive, seeks enemies
- **Support**: Defensive, heals allies
- Uses same ability system as PlayerController
- Adaptive targeting based on class role

### Presentation Layer: UI System

#### ClassSelectionUI
- Pre-match screen
- Class selection buttons
- Dynamic skin selection based on chosen class
- Start game with confirmed selection

#### GameHUD
- Real-time health display
- Class icon indicators
- Bot class visualization
- (Future: ability cooldown displays)

### Management Layer: GameManager

```
GameManager (Singleton)
├── Stores player class selection
├── Spawns player with selected class/skin
├── Spawns bots with assigned classes
└── Initializes HUD with references
```

**Lifecycle:**
1. ClassSelectionUI → Player selects class/skin
2. GameManager stores selection
3. Match starts → GameManager spawns entities
4. Controllers initialize with class data
5. HUD displays current state

## Data Flow

### Class Selection Flow
```
1. User clicks class button
   ↓
2. ClassSelectionUI.SelectClass(index)
   ↓
3. Display available skins for class
   ↓
4. User selects skin
   ↓
5. User clicks Start Game
   ↓
6. GameManager.SetPlayerClass(class, skinIndex)
   ↓
7. Selection stored for match initialization
```

### Match Initialization Flow
```
1. GameManager.InitializeMatch()
   ↓
2. Spawn PlayerController at spawn point
   ↓
3. PlayerController.SetPlayerClass(selectedClass, skinIndex)
   ↓
4. PlayerController.InitializeStats() - loads from SO
   ↓
5. PlayerController.ApplySkin() - applies visual
   ↓
6. Repeat for bots
   ↓
7. GameHUD.SetPlayer() and SetBots()
```

### Ability Execution Flow
```
1. Input detected (click/button)
   ↓
2. Check cooldown timer
   ↓
3. Read ability type from PlayerClassSO
   ↓
4. Execute ability method (PerformMeleeAttack, PerformHealCone, etc.)
   ↓
5. Apply effects to targets
   ↓
6. Start cooldown timer
```

## Class Balance Design

### Tank (150 HP, 15 ATK, 4.25 SPD)
- **Role**: Frontline, initiation, crowd control
- **Strengths**: High survivability, AoE disruption
- **Weaknesses**: Low damage, low mobility
- **Playstyle**: Absorb damage, disrupt with knockback

### DPS (100 HP, 30 ATK, 5.5 SPD)
- **Role**: Damage dealer, elimination
- **Strengths**: High burst damage, fast cooldowns
- **Weaknesses**: Low HP, requires positioning
- **Playstyle**: High-risk high-reward aggression

### Support (100 HP, 10 ATK, 5 SPD)
- **Role**: Sustain, team enabler
- **Strengths**: Team healing, sustained fights
- **Weaknesses**: Low personal damage
- **Playstyle**: Protect allies, enable extended engagements

## Extension Points

### Adding New Classes
1. Create new PlayerClassSO asset
2. Configure stats and abilities
3. Add skins to availableSkins array
4. Add to ClassSelectionUI's availableClasses
5. (Optional) Add class-specific AI behavior in BotController

### Adding New Abilities
1. Add new AbilityType enum value
2. Implement ability method in PlayerController
3. Implement AI usage in BotController
4. Assign to class's basicAbilityType or ultimateAbilityType

### Adding New Skins
1. Create material/mesh assets
2. Add to class's availableSkins array in Inspector
3. Automatically appears in ClassSelectionUI

## Performance Considerations

### Memory Efficiency
- ScriptableObjects are shared references (not copied)
- Single instance per class used by all entities
- Skin data stored in SO, not duplicated per entity

### Runtime Efficiency
- Stats read once on initialization
- No runtime lookups to ScriptableObject
- Controllers cache runtime values

### Scalability
- System supports unlimited classes/skins
- No hardcoded class-specific code
- Data-driven approach scales easily

## Testing Strategy

### Unit Testing
- Use ClassSystemTest.cs to verify:
  - All classes load correctly
  - Stats match requirements
  - Skins are configured

### Integration Testing
- Test in Unity Play Mode:
  - Class selection flow
  - Ability execution
  - AI behavior
  - Skin application

### Balance Testing
- Playtest to verify:
  - Class roles feel distinct
  - No single class dominates
  - All abilities are useful

## Future Enhancements

### Short Term
- Add visual effects for abilities
- Implement projectile system for ranged attacks
- Add team assignment (red vs blue)
- Victory/defeat conditions

### Medium Term
- Network synchronization (Netcode for GameObjects)
- More classes (Ranger, Mage, Assassin)
- More skins per class
- Stat modifiers/buffs system

### Long Term
- Shop system for unlocking skins
- Class progression/leveling
- Custom ability loadouts
- Seasonal events with temporary classes

## Known Limitations

### Current Implementation
- No network code (local only)
- No shop/unlock system
- Skins are cosmetic only (no mesh/material assets yet)
- Basic AI (no advanced tactics)
- No team assignment system

### Intentional Design Decisions
- Classes share same controller code (not subclassed)
- Abilities are enum-based (not component-based)
- Simple stat system (no derived stats)
- Local skin selection (no persistence)

These limitations align with MVP scope and can be addressed in future iterations.
