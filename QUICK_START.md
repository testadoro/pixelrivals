# Quick Start Guide - PixelRivals Class System

## 🎮 What Was Implemented

This implementation provides a complete class system for a 3v3 arena game with:
- **3 Playable Classes**: Tank, DPS, Support
- **Multiple Skins**: 3 skins per class
- **Smart AI**: Bots that adapt to their class role
- **Full UI**: Class selection and in-game HUD

## 📊 Class Overview

| Class   | HP  | Attack | Speed | Cooldown | Special Ability        |
|---------|-----|--------|-------|----------|------------------------|
| Tank    | 150 | 15     | 4.25  | 1.2s     | Knockback AoE Ultimate |
| DPS     | 100 | 30     | 5.5   | 0.8s     | High damage, fast atk  |
| Support | 100 | 10     | 5.0   | 1.5s     | Heal Cone + AoE Heal   |

## 🚀 How to Use

### For Unity Developers

1. **Open the Project in Unity**
   - Unity 2021.3+ with URP
   - TextMeshPro package installed

2. **Load Class Assets**
   ```
   Assets/Resources/Classes/
   ├── Tank.asset
   ├── DPS.asset
   └── Support.asset
   ```

3. **Test the System**
   - Create empty GameObject
   - Add `ClassSystemTest` component
   - Press Play - check Console for test results

4. **Create a Scene**
   - See `Assets/Scripts/SETUP_GUIDE.md` for detailed setup
   - Create ClassSelection scene (UI)
   - Create GameArena scene (gameplay)

### For Designers

1. **Adjust Class Stats**
   - Select any class asset in Project window
   - Modify values in Inspector:
     - Base HP
     - Attack damage
     - Movement speed
     - Cooldowns
   - Changes apply immediately

2. **Add/Modify Skins**
   - Select class asset
   - Expand `Available Skins` array
   - Add skin name, material, mesh
   - Appears automatically in UI

### For Programmers

1. **Add New Class**
   ```
   Right-click in Project → Create → PixelRivals → Player Class
   Configure stats and abilities
   Add to ClassSelectionUI's availableClasses array
   ```

2. **Add New Ability**
   ```csharp
   // In PlayerClassSO.cs
   public enum AbilityType {
       // ... existing abilities
       MyNewAbility
   }
   
   // In PlayerController.cs and BotController.cs
   private void PerformMyNewAbility() {
       // Implementation
   }
   ```

3. **Extend AI Behavior**
   ```csharp
   // In BotController.cs, UpdateAI() method
   if (botClass.classType == ClassType.MyNewClass) {
       // Custom AI logic
   }
   ```

## 📁 Project Structure

```
pixelrivals/
│
├── 📄 README.md              → Project overview
├── 📄 ARCHITECTURE.md        → System design details
├── 📄 SETUP_GUIDE.md         → Unity editor setup
├── 📄 IMPLEMENTATION_SUMMARY → What's been built
└── 📄 QUICK_START.md         → This file!
│
└── Assets/
    ├── Scripts/
    │   ├── 🎯 PlayerClassSO.cs       → ScriptableObject definition
    │   ├── 🎮 PlayerController.cs    → Player control + abilities
    │   ├── 🤖 BotController.cs       → AI behavior
    │   ├── 🎨 ClassSelectionUI.cs    → Pre-match UI
    │   ├── 📊 GameHUD.cs             → In-game HUD
    │   ├── ⚙️ GameManager.cs         → Game coordination
    │   └── 🧪 ClassSystemTest.cs     → Verification tests
    │
    └── Resources/Classes/
        ├── Tank.asset     → Tank class data
        ├── DPS.asset      → DPS class data
        └── Support.asset  → Support class data
```

## 🔍 System Components

### 1. Data (ScriptableObjects)
```
PlayerClassSO
├── Stats: HP, Attack, Speed, Cooldowns
├── Abilities: Basic + Ultimate types
└── Skins: Material/Mesh variants
```

### 2. Controllers (Gameplay)
```
PlayerController → Player input + abilities
BotController → AI + abilities (same system)
```

### 3. UI (Presentation)
```
ClassSelectionUI → Pre-match class/skin selection
GameHUD → In-game status display
```

### 4. Management (Orchestration)
```
GameManager → Match setup + entity spawning
```

## 🎯 Common Use Cases

### "I want to balance a class"
1. Find class asset: `Assets/Resources/Classes/Tank.asset`
2. Select it
3. Change values in Inspector
4. Test in Play Mode

### "I want to add a new class"
1. Right-click → Create → PixelRivals → Player Class
2. Name it (e.g., "Ranger")
3. Configure stats
4. Add 2-3 skins
5. Drag to ClassSelectionUI's available classes

### "I want to test the system"
1. Create empty GameObject
2. Add `ClassSystemTest` component
3. Press Play
4. Check Console for results

### "I want to create a playable scene"
Follow `Assets/Scripts/SETUP_GUIDE.md` step-by-step

## 🐛 Troubleshooting

### Classes not loading?
- Check that assets are in `Assets/Resources/Classes/` folder
- Resources folder is case-sensitive
- Ensure .asset files are present

### Controller not working?
- Add CharacterController component to GameObject
- Verify PlayerClassSO is assigned
- Check Unity Input settings (Edit → Project Settings → Input)

### UI not displaying?
- Ensure TextMeshPro is installed
- Check UI references are assigned in Inspector
- Verify Canvas is set to Screen Space - Overlay

## 📚 Documentation Index

| Document | Purpose |
|----------|---------|
| README.md | Project overview & features |
| ARCHITECTURE.md | System design & patterns |
| SETUP_GUIDE.md | Unity editor setup steps |
| IMPLEMENTATION_SUMMARY.md | Feature checklist |
| QUICK_START.md | This guide! |
| Assets/Scripts/README.md | Code implementation details |

## ✨ Key Features

- ✅ **Modular**: Add classes without code changes
- ✅ **Data-Driven**: All stats in ScriptableObjects
- ✅ **Extensible**: Easy to add abilities/skins
- ✅ **AI-Ready**: Bots use same system as player
- ✅ **Designer-Friendly**: Edit stats in Inspector
- ✅ **Well-Documented**: Comprehensive guides
- ✅ **Production-Ready**: Follows Unity best practices

## 🎉 You're Ready!

This system is complete and ready to integrate into your Unity project. Start by:
1. Testing with `ClassSystemTest`
2. Creating scenes following `SETUP_GUIDE.md`
3. Customizing classes for your game

For questions or issues, refer to the comprehensive documentation in the project root and `Assets/Scripts/` folder.

**Happy developing! 🚀**
