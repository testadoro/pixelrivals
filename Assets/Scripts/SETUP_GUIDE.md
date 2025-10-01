# PixelRivals Setup Guide

## Unity Project Setup

### 1. Prerequisites
- Unity 2021.3 or newer with URP
- TextMeshPro package installed

### 2. Scene Setup

#### Class Selection Scene
1. Create a new scene named `ClassSelection`
2. Add UI Canvas (Screen Space - Overlay)
3. Create GameObject `ClassSelectionManager` and attach `ClassSelectionUI.cs`
4. Set up UI elements:
   - 3 buttons for class selection (Tank, DPS, Support)
   - 3 images for class icons
   - 3 text elements for class names
   - 3 buttons for skin selection
   - 3 images for skin previews
   - 3 text elements for skin names
   - 1 button for "Start Game"
   - Text fields for displaying current selection
5. Assign UI references in ClassSelectionUI component
6. Drag the 3 class ScriptableObjects from `Assets/Resources/Classes/` to the availableClasses array

#### Game Scene
1. Create a new scene named `GameArena`
2. Add a plane for the ground
3. Create GameObject `GameManager` and attach `GameManager.cs`
4. Create player prefab:
   - Create empty GameObject
   - Add `PlayerController.cs`
   - Add `CharacterController` component
   - Add child object with MeshFilter and MeshRenderer for visuals
   - Save as prefab
5. Create bot prefab:
   - Create empty GameObject
   - Add `BotController.cs`
   - Add `CharacterController` component
   - Add child object with MeshFilter and MeshRenderer for visuals
   - Save as prefab
6. Assign prefabs to GameManager
7. Create spawn points (Empty GameObjects) and assign to GameManager
8. Create UI Canvas for HUD:
   - Add GameObject `HUD` with `GameHUD.cs`
   - Create UI elements for health bars, class icons, etc.
   - Assign references in GameHUD component

### 3. ScriptableObject Configuration

The three class assets are already configured in `Assets/Resources/Classes/`:
- `Tank.asset` - High HP tank with knockback ultimate
- `DPS.asset` - High damage dealer with fast attacks
- `Support.asset` - Healer with cone and AoE heal abilities

To modify stats:
1. Select the asset in Project window
2. Adjust values in Inspector
3. Changes apply immediately to all instances using that class

### 4. Adding Custom Skins

1. Create materials for each skin variant in `Assets/Resources/Skins/`
2. (Optional) Create custom meshes for skin variants
3. Select a class asset (e.g., Tank.asset)
4. In Inspector, expand `availableSkins` array
5. For each skin:
   - Set skinName (displayed in UI)
   - Assign skinMaterial
   - Assign skinMesh (optional, uses default if null)

### 5. Testing

#### Test in Editor (Play Mode)
1. Open GameArena scene
2. In GameManager, manually assign a class to test
3. Press Play
4. Use WASD to move, left-click for basic ability, right-click for ultimate

#### Test Class Selection Flow
1. Open ClassSelection scene
2. Press Play
3. Click a class button
4. Select a skin
5. Click "Start Game"
6. Should transition to game scene with selected class

### 6. Input Setup

Add to Unity Input Manager (Edit → Project Settings → Input):
- Horizontal: A/D or Left/Right arrows
- Vertical: W/S or Up/Down arrows
- Fire1: Left mouse button (basic ability)
- Fire2: Right mouse button (ultimate)

### 7. Bot AI Configuration

Bots automatically adapt based on class type:
- **Tank/DPS**: Seek and engage enemy players
- **Support**: Prioritize healing low-health allies

To adjust bot behavior:
1. Open `BotController.cs`
2. Modify values:
   - `detectionRange` - how far bots can see enemies
   - `attackRange` - distance to start attacking
   - Timers in `Wander()` method

### 8. Common Issues

#### "Missing Script Reference"
- Ensure all .meta files are present
- Reimport scripts (Right-click → Reimport)

#### "PlayerClassSO not found"
- Check that class assets are in `Resources/Classes/` folder
- Resources folder name is case-sensitive

#### Controller not moving
- Ensure CharacterController component is attached
- Check that Input axes are configured

#### Skins not applying
- Verify skin material/mesh is assigned in ScriptableObject
- Check that player/bot has MeshFilter and MeshRenderer components

### 9. Building for Mobile

1. Switch platform to Android/iOS (File → Build Settings)
2. Replace mouse input with touch input in PlayerController
3. Add on-screen buttons for abilities
4. Adjust UI scaling for mobile screens
5. Test performance and optimize as needed

## Next Steps

- Implement projectile system for ranged attacks
- Add visual effects for abilities
- Create team assignment system for 3v3
- Add match timer and victory conditions
- Implement respawn system
- Add sound effects
- Create particle effects for heals/knockback
