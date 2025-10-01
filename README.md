# PixelRivals
Unity URP 3v3 Arena — Mobile MVP

## Features

### Class System
Three playable classes with unique stats and abilities:
- **Tank**: High HP (150), crowd control ultimate (Knockback AoE)
- **DPS**: High damage (30), fast cooldowns
- **Support**: Healing abilities (20 HP cone heal, AoE ultimate)

### Skin System
Each class has 2-3 selectable skins:
- Purely cosmetic changes (materials/meshes)
- No gameplay impact
- Local selection (no shop required)

### ScriptableObject Architecture
- Modular stat management via `PlayerClassSO`
- Easy balancing through Unity Inspector
- Simple to extend with new classes/skins

### AI Bots
- AI-controlled bots can use any class
- Adaptive behavior based on class type
- Tank/DPS engage enemies
- Support prioritizes healing allies

### UI Components
- **Class Selection**: Pre-match screen for choosing class and skin
- **Game HUD**: Shows player/bot class icons, health, and cooldowns

## Project Structure
See `Assets/Scripts/README.md` for detailed documentation on:
- Class stats and balancing
- Controller implementation
- UI system
- Extending the system

## Getting Started
1. Open project in Unity (URP)
2. Load the ClassSelection scene
3. Select a class and skin
4. Start match to play with selected configuration
