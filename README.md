# pixelrivals
Unity URP 3v3 Arena — Mobile MVP

## Description
A mobile-optimized Unity URP prototype for a 3v3 arena game featuring:
- Conquest zone capture mechanics
- AI-controlled enemy bots
- Virtual joystick controls
- Attack and special ability buttons
- Basic HUD with health bar and cooldown indicators

## Project Structure

### Scripts
- **PlayerController.cs** - Controls player movement and combat with virtual joystick input
- **BotAI.cs** - Enemy bot AI with patrol, chase, and attack behaviors
- **Projectile.cs** - Projectile system for attacks
- **ConquestZone.cs** - Capture zone mechanics with team control
- **VirtualJoystick.cs** - Mobile touch joystick implementation
- **GameHUD.cs** - Main HUD controller for health, abilities, and zone status
- **GameManager.cs** - Game setup and bot spawning

### How to Open in Unity
1. Install Unity 2022.3.10f1 or later with URP support
2. Open Unity Hub
3. Click "Add" and select this project folder
4. Open the project
5. Create a new scene or use the scripts to build your arena scene

### Setting Up a Scene
1. Create a new scene in Unity
2. Add a Plane for the ground (tagged as "Environment")
3. Create a Player GameObject:
   - Add a Capsule mesh
   - Tag it as "Player"
   - Add PlayerController script
   - Create a child GameObject named "FirePoint" for projectile spawn
4. Create a ConquestZone GameObject:
   - Add a Cylinder mesh (scale it as the capture zone)
   - Add BoxCollider with "Is Trigger" enabled
   - Add ConquestZone script
5. Create Bot Prefab:
   - Create a Capsule GameObject
   - Add BotAI script
   - Create a child GameObject named "FirePoint"
   - Save as Prefab
6. Create Projectile Prefab:
   - Create a Sphere GameObject (scale to 0.2)
   - Add Rigidbody (Is Kinematic = true)
   - Add SphereCollider (Is Trigger = true)
   - Add Projectile script
   - Save as Prefab
7. Setup UI Canvas:
   - Create UI Canvas (Screen Space - Overlay)
   - Add virtual joystick (2 Images: background + handle)
   - Add attack button (UI Button)
   - Add special ability button (UI Button)
   - Add health bar (UI Slider)
   - Add conquest zone bar (UI Slider)
   - Add GameHUD script to Canvas
   - Connect all UI references

### Required Unity Packages
- Universal RP (URP) 14.0.8
- TextMeshPro 3.0.6
- Input System 1.6.3 (optional, can use legacy input)

### Mobile Controls
- **Virtual Joystick** (bottom-left) - Movement
- **Attack Button** (bottom-right) - Basic attack
- **Special Ability Button** - Triple projectile spread (5s cooldown)

### Features
- Player movement with virtual joystick
- Bot AI with patrol and combat behavior
- Projectile-based combat system
- Conquest zone capture mechanics
- Health system
- Special ability with cooldown
- Mobile-optimized UI

## Next Steps
To complete the prototype:
1. Create 3D models or use Unity primitives
2. Add materials and visual effects
3. Configure URP rendering settings
4. Add audio feedback
5. Implement team system for 3v3 matches
6. Add respawn mechanics
7. Implement match scoring system
8. Polish UI/UX for mobile

## License
MIT License
