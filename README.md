# PixelRivals

<div align="center">

**Unity URP 3v3 Arena — Mobile MVP**

A mobile-optimized Unity prototype for a 3v3 arena game with conquest zone mechanics, AI bots, and touch controls.

[![Unity Version](https://img.shields.io/badge/Unity-2022.3.10f1+-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![URP](https://img.shields.io/badge/Render%20Pipeline-URP%2014.0.8-green.svg)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/index.html)

</div>

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Quick Start](#-quick-start)
- [Documentation](#-documentation)
- [Project Structure](#-project-structure)
- [Core Systems](#-core-systems)
- [Mobile Controls](#-mobile-controls)
- [Development](#-development)
- [Contributing](#-contributing)
- [License](#-license)

## 🎮 Overview

PixelRivals is a complete Unity URP prototype demonstrating the core mechanics of a mobile 3v3 arena game. The project includes:

- **Player Controller** with virtual joystick movement
- **AI Bot System** with patrol, chase, and attack behaviors
- **Conquest Zone** capture mechanics
- **Combat System** with projectiles and abilities
- **Mobile-Optimized UI** with touch controls
- **Complete C# Scripts** ready to use and extend

This prototype serves as a solid foundation for developing a full mobile arena game.

## ✨ Features

### ✅ Implemented
- 🎮 **Virtual Joystick** - Touch-based movement controls
- 🤖 **AI Bots** - Autonomous enemy behavior (patrol, chase, attack)
- 🎯 **Combat System** - Projectile-based attacks with cooldowns
- ⚡ **Special Abilities** - Triple shot spread attack
- 🏳️ **Conquest Zone** - Territory capture mechanics
- 💚 **Health System** - Player and bot health management
- 📊 **HUD** - Health bars, cooldown indicators, zone status
- 📱 **Mobile-Optimized** - Touch controls and performance considerations
- 🎨 **URP Support** - Universal Render Pipeline for better mobile performance

### 🚧 Ready to Add
- Sound effects and music
- Particle effects for abilities
- 3v3 team system
- Match scoring and victory conditions
- Respawn system
- Character customization
- Multiple arenas
- Power-ups and pickups

## 🚀 Quick Start

### Prerequisites
- Unity 2022.3.10f1 or later
- Basic Unity knowledge
- Mobile development setup (for device testing)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/testadoro/pixelrivals.git
   cd pixelrivals
   ```

2. **Open in Unity Hub**
   - Open Unity Hub
   - Click "Add" → "Add project from disk"
   - Select the cloned folder
   - Open the project

3. **Setup Scene**
   - Follow the detailed instructions in [Assets/SETUP_GUIDE.txt](Assets/SETUP_GUIDE.txt)
   - Or refer to the [Quick Reference](QUICK_REFERENCE.md)

4. **Test in Editor**
   - Create a test scene following the setup guide
   - Press Play to test
   - UI buttons work with mouse clicks in editor

5. **Build for Mobile** (Optional)
   - File → Build Settings → Android/iOS
   - Configure player settings
   - Build and deploy to device

## 📚 Documentation

| Document | Description |
|----------|-------------|
| [QUICK_REFERENCE.md](QUICK_REFERENCE.md) | Quick reference card with all essential information |
| [SETUP_GUIDE.txt](Assets/SETUP_GUIDE.txt) | Step-by-step scene setup instructions |
| [ARCHITECTURE.md](ARCHITECTURE.md) | System architecture and design documentation |
| [DEVELOPMENT_NOTES.md](DEVELOPMENT_NOTES.md) | Development guidelines and best practices |
| [CONTRIBUTING.md](CONTRIBUTING.md) | How to contribute to the project |
| [CHANGELOG.md](CHANGELOG.md) | Version history and changes |

## 📁 Project Structure

```
pixelrivals/
├── Assets/
│   ├── Scripts/               # All C# game scripts
│   │   ├── PlayerController.cs      # Player movement and combat
│   │   ├── BotAI.cs                 # Enemy AI behavior
│   │   ├── Projectile.cs            # Projectile system
│   │   ├── ConquestZone.cs          # Zone capture mechanics
│   │   ├── VirtualJoystick.cs       # Touch joystick
│   │   ├── GameHUD.cs               # UI controller
│   │   ├── GameManager.cs           # Match management
│   │   └── SetupValidator.cs        # Debug validation tool
│   ├── Scenes/                # Unity scenes (create your own)
│   ├── Prefabs/               # Game prefabs (create from guide)
│   └── Materials/             # Materials (create as needed)
├── ProjectSettings/           # Unity project configuration
├── Packages/                  # Package dependencies
└── Documentation/             # Project documentation
```

## 🎯 Core Systems

### PlayerController
Controls the player character with mobile input support.

**Features:**
- Movement with virtual joystick
- Basic attack (0.5s cooldown)
- Special ability - triple shot (5s cooldown)
- Health system (100 HP)
- Character rotation toward movement

**Key Methods:**
```csharp
SetMoveDirection(Vector2)    // Called by virtual joystick
Attack()                     // Fire single projectile
UseSpecialAbility()         // Fire 3-projectile spread
TakeDamage(int)             // Apply damage to player
```

### BotAI
Autonomous enemy AI with three behavioral states.

**States:**
1. **Patrol** - Wander randomly when no target
2. **Chase** - Follow player within detection range (15 units)
3. **Attack** - Fire at player within attack range (10 units)

**Configuration:**
- Move Speed: 3 units/sec
- Health: 80 HP
- Attack Cooldown: 1 second

### ConquestZone
Capturable objective zone with visual feedback.

**Mechanics:**
- Capture progress: -100 (Enemy) to +100 (Player)
- Capture rate: 1 point/second per player in zone
- Contest mode: Equal teams cancel each other
- Visual states: Neutral (Gray), Player (Blue), Enemy (Red)

### Combat System
Projectile-based combat with team collision detection.

**Projectile Stats:**
- Speed: 15 units/sec
- Damage: 10 HP
- Lifetime: 5 seconds
- Team-based collision (doesn't hit owner)

## 📱 Mobile Controls

| Control | Location | Function |
|---------|----------|----------|
| 🕹️ Virtual Joystick | Bottom-Left | Move player in any direction |
| ⚔️ Attack Button | Bottom-Right | Fire single projectile |
| ⚡ Special Button | Above Attack | Triple shot spread (5s cooldown) |

**UI Elements:**
- ❤️ Health Bar (top-left)
- 🏳️ Conquest Zone Status (top-center)
- ⏱️ Cooldown Indicators (on buttons)

## 🛠️ Development

### Setup Validation
Use the built-in validator to check your scene setup:

1. Add `SetupValidator` script to any GameObject
2. Right-click → "Validate Scene Setup"
3. Check Console for validation results

### Customization
All parameters are exposed in the inspector:

**Player Settings:**
- Move Speed
- Attack Cooldown
- Special Ability Cooldown
- Max Health

**Bot Settings:**
- Detection Range
- Attack Range
- Move Speed
- Attack Cooldown

**See [DEVELOPMENT_NOTES.md](DEVELOPMENT_NOTES.md) for detailed customization guides.**

### Building for Mobile

#### Android
```
1. File → Build Settings → Android
2. Switch Platform
3. Player Settings:
   - Bundle Identifier: com.yourcompany.pixelrivals
   - Minimum API Level: 22
   - Target API Level: 30+
4. Build and Run
```

#### iOS
```
1. File → Build Settings → iOS
2. Switch Platform
3. Player Settings:
   - Bundle Identifier: com.yourcompany.pixelrivals
   - Target iOS Version: 11.0+
4. Build (requires macOS + Xcode)
```

## 🤝 Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on:
- Code style guidelines
- Development workflow
- Pull request process
- Code of conduct

### Quick Contribution Guide
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Feature: Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📋 Requirements

### Unity Packages
- **Universal RP**: 14.0.8
- **TextMeshPro**: 3.0.6
- **Input System**: 1.6.3 (optional)

### Supported Platforms
- ✅ Android (API 22+)
- ✅ iOS (11.0+)
- ✅ Unity Editor (for testing)

### Recommended Specs
- **Mobile**: Mid-range devices (2GB+ RAM)
- **Target FPS**: 60 FPS
- **Unity Version**: 2022.3 LTS

## 🎯 Roadmap

### Version 0.2.0 (Planned)
- [ ] Object pooling for projectiles
- [ ] Respawn system
- [ ] Basic audio effects
- [ ] Particle effects for abilities

### Version 0.3.0 (Planned)
- [ ] 3v3 team system
- [ ] Match scoring
- [ ] Victory conditions
- [ ] Power-ups system

### Version 1.0.0 (Future)
- [ ] Multiple arenas
- [ ] Character customization
- [ ] Progression system
- [ ] Enhanced AI behaviors

## 🐛 Known Issues

- No .meta files (Unity generates these automatically)
- Scene files must be created manually (use SETUP_GUIDE.txt)
- Prefabs must be created manually (specifications in PREFAB_CONFIGS.json)
- Basic AI without advanced pathfinding
- No object pooling (causes minor GC allocations)

See [Issues](https://github.com/testadoro/pixelrivals/issues) for full list.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Unity Technologies for the Universal Render Pipeline
- Contributors and testers
- Unity community for resources and support

## 📞 Support

- 📖 Check [QUICK_REFERENCE.md](QUICK_REFERENCE.md) for quick answers
- 📚 Read [SETUP_GUIDE.txt](Assets/SETUP_GUIDE.txt) for setup help
- 🐛 Report bugs via [Issues](https://github.com/testadoro/pixelrivals/issues)
- 💬 Ask questions in [Discussions](https://github.com/testadoro/pixelrivals/discussions)

---

<div align="center">

**Made with ❤️ for the Unity community**

[⬆ Back to Top](#pixelrivals)

</div>
