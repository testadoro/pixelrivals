# PixelRivals - Class System Documentation

## Overview
This system implements a modular class-based character system for PixelRivals 3v3 arena game using Unity ScriptableObjects.

## Architecture

### ScriptableObjects
- **PlayerClassSO**: Core ScriptableObject that defines all stats and abilities for a class
  - Location: `Assets/Scripts/ScriptableObjects/PlayerClassSO.cs`
  - Contains: HP, Attack, Speed, Cooldowns, Ability types, Available skins

### Classes Implemented

#### 1. Tank
- **HP**: 150 (+50% base)
- **Attack**: 15
- **Speed**: 4.25 (-15% base)
- **Attack Cooldown**: 1.2s
- **Ultimate Cooldown**: 15s
- **Basic Ability**: Melee Attack
- **Ultimate**: Knockback AoE (8m range)
- **Skins**: 3 options (Default, Heavy, Elite)

#### 2. DPS
- **HP**: 100 (base)
- **Attack**: 30 (+25% base)
- **Speed**: 5.5
- **Attack Cooldown**: 0.8s (-20% base)
- **Ultimate Cooldown**: 10s
- **Basic Ability**: Melee Attack
- **Ultimate**: Ranged Attack
- **Skins**: 3 options (Default, Striker, Assassin)

#### 3. Support
- **HP**: 100 (base)
- **Attack**: 10
- **Speed**: 5
- **Attack Cooldown**: 1.5s
- **Ultimate Cooldown**: 12s
- **Basic Ability**: Heal Cone (20 HP, 10m range)
- **Ultimate**: Heal AoE (20 HP, 10m range)
- **Skins**: 3 options (Default, Medic, Priest)

## Controllers

### PlayerController
- Location: `Assets/Scripts/Controllers/PlayerController.cs`
- Manages player input and movement
- Uses stats from assigned PlayerClassSO
- Handles ability execution
- Applies selected skin

### BotController
- Location: `Assets/Scripts/Controllers/BotController.cs`
- AI-controlled entities using same class system
- Adaptive AI based on class type:
  - Tank/DPS: Engage enemies
  - Support: Prioritize healing allies
- Uses stats from assigned PlayerClassSO

## UI System

### ClassSelectionUI
- Location: `Assets/Scripts/UI/ClassSelectionUI.cs`
- Pre-match screen for selecting class and skin
- Displays all available classes with icons
- Shows available skins for selected class
- Confirms selection before starting match

### GameHUD
- Location: `Assets/Scripts/UI/GameHUD.cs`
- In-game HUD displaying:
  - Player class icon and name
  - Health bar
  - Bot class icons
  - Ability cooldowns

## GameManager
- Location: `Assets/Scripts/GameManager.cs`
- Singleton managing game state
- Stores player class selection
- Spawns player and bots with selected classes
- Initializes match with proper stats

## Asset Structure
```
Assets/
├── Scripts/
│   ├── ScriptableObjects/
│   │   └── PlayerClassSO.cs
│   ├── Controllers/
│   │   ├── PlayerController.cs
│   │   └── BotController.cs
│   ├── UI/
│   │   ├── ClassSelectionUI.cs
│   │   └── GameHUD.cs
│   └── GameManager.cs
└── Resources/
    ├── Classes/
    │   ├── Tank.asset
    │   ├── DPS.asset
    │   └── Support.asset
    └── Skins/
        └── (skin materials and meshes)
```

## How to Extend

### Adding a New Class
1. Create a new ScriptableObject asset (Right-click → Create → PixelRivals → Player Class)
2. Configure stats according to class role
3. Add 2-3 skin options
4. Add to ClassSelectionUI's availableClasses array

### Adding a New Skin
1. Create skin material and/or mesh
2. Add to class's availableSkins array in the inspector
3. Skin will automatically appear in selection UI

### Adding New Abilities
1. Add new ability type to `AbilityType` enum in PlayerClassSO.cs
2. Implement ability logic in PlayerController and BotController
3. Assign to class's basicAbilityType or ultimateAbilityType

## Balancing Notes
- Tank: High survivability, crowd control ultimate
- DPS: High damage output, fast cooldowns
- Support: Team utility, healing focused
- All values are exposed in ScriptableObject for easy tweaking

## Future Enhancements
- Network synchronization for multiplayer
- Shop system for unlocking skins
- More class options
- Advanced AI behaviors per class
- Stat modifiers and buffs system
