# Contributing to PixelRivals

Thank you for your interest in contributing to PixelRivals! This document provides guidelines for contributing to the project.

## 🎯 Project Vision

PixelRivals is a mobile-optimized Unity URP prototype for a 3v3 arena game. The goal is to create a solid foundation that demonstrates:
- Mobile touch controls
- AI bot behaviors
- Conquest zone mechanics
- Clean, maintainable code architecture

## 🤝 How to Contribute

### Reporting Bugs

**Before submitting a bug report:**
1. Check existing issues to avoid duplicates
2. Run the SetupValidator script to ensure proper setup
3. Check the Unity Console for error messages
4. Verify you're using Unity 2022.3.10f1 or later

**When reporting bugs, include:**
- Unity version
- Platform (Editor/Android/iOS)
- Steps to reproduce
- Expected vs actual behavior
- Screenshots or videos if applicable
- Console error messages

**Bug Report Template:**
```markdown
**Unity Version**: 2022.3.10f1
**Platform**: Android / iOS / Editor
**Description**: Brief description of the bug

**Steps to Reproduce**:
1. Step one
2. Step two
3. Step three

**Expected Behavior**: What should happen
**Actual Behavior**: What actually happens

**Console Errors**: Paste any error messages

**Screenshots**: Add if applicable
```

### Suggesting Features

**Before suggesting features:**
1. Check if it aligns with the mobile 3v3 arena vision
2. Consider performance implications for mobile
3. Review existing feature requests

**Feature Request Template:**
```markdown
**Feature Name**: Brief name

**Problem**: What problem does this solve?

**Solution**: Describe your proposed solution

**Alternatives**: Other approaches considered

**Mobile Considerations**: Impact on mobile performance/UX

**Implementation Notes**: Technical considerations
```

### Code Contributions

#### Getting Started

1. **Fork the repository**
2. **Clone your fork**
   ```bash
   git clone https://github.com/YOUR_USERNAME/pixelrivals.git
   ```
3. **Open in Unity** (2022.3.10f1 or later)
4. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

#### Development Guidelines

##### Code Style

**Naming Conventions:**
```csharp
// Classes and Methods: PascalCase
public class PlayerController { }
public void TakeDamage() { }

// Private fields: camelCase
private int currentHealth;
private float moveSpeed;

// Serialized fields: camelCase with attribute
[SerializeField] private float attackCooldown;

// Constants: PascalCase or UPPER_CASE
private const int MaxPlayers = 3;
private const float DEFAULT_SPEED = 5f;

// Properties: PascalCase
public int CurrentHealth { get; private set; }
```

**Code Organization:**
```csharp
public class ExampleScript : MonoBehaviour
{
    // 1. Serialized fields (grouped with [Header])
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    
    [Header("Combat")]
    [SerializeField] private int damage;
    
    // 2. Private fields
    private Transform target;
    private float lastAttackTime;
    
    // 3. Properties
    public int CurrentHealth { get; private set; }
    
    // 4. Unity lifecycle methods
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    
    // 5. Public methods
    public void TakeDamage(int amount) { }
    
    // 6. Private methods
    private void FindTarget() { }
}
```

**Comments:**
```csharp
/// <summary>
/// XML documentation for public methods and classes
/// </summary>
/// <param name="amount">Parameter description</param>
public void TakeDamage(int amount)
{
    // Comment complex logic
    currentHealth = Mathf.Max(0, currentHealth - amount);
    
    // TODO: Add death animation
    if (currentHealth <= 0)
    {
        Die();
    }
}
```

##### Performance Considerations

**DO:**
- Cache component references in Awake/Start
- Use object pooling for frequently spawned objects
- Minimize allocations in Update loop
- Use SerializeField instead of public for inspector exposure
- Profile your changes on mobile devices

**DON'T:**
- Use GetComponent in Update
- Create garbage in Update/FixedUpdate
- Use Find methods every frame
- Use Camera.main repeatedly (cache it)
- Ignore mobile performance implications

**Example:**
```csharp
// ❌ BAD
void Update()
{
    GetComponent<Rigidbody>().AddForce(Vector3.up); // Allocates every frame
    GameObject player = GameObject.Find("Player"); // Slow search every frame
}

// ✅ GOOD
private Rigidbody rb;
private GameObject player;

void Awake()
{
    rb = GetComponent<Rigidbody>();
    player = GameObject.FindGameObjectWithTag("Player"); // Once at start
}

void Update()
{
    rb.AddForce(Vector3.up); // Cached reference
}
```

##### Testing Your Changes

1. **Run SetupValidator** in your test scene
2. **Test in Unity Editor** first
3. **Test on mobile device** (Unity Remote or build)
4. **Check for errors** in Console
5. **Profile performance** if adding new systems
6. **Test different scenarios**:
   - Player movement
   - Bot AI behavior
   - Combat interactions
   - UI responsiveness
   - Edge cases

##### Documentation

**Update documentation when:**
- Adding new scripts or systems
- Changing public APIs
- Modifying setup procedures
- Adding dependencies
- Changing architecture

**Required documentation:**
- XML comments on public classes/methods
- Update README.md if changing features
- Update SETUP_GUIDE.txt if changing setup
- Update ARCHITECTURE.md if changing design
- Add to CHANGELOG.md

#### Commit Guidelines

**Commit Message Format:**
```
Type: Brief description (50 chars or less)

Optional detailed explanation. Wrap at 72 characters.
Explain what and why, not how.

Fixes #123
```

**Types:**
- `Feature:` New feature
- `Bugfix:` Bug fix
- `Refactor:` Code restructuring
- `Docs:` Documentation changes
- `Performance:` Performance improvements
- `Test:` Adding tests
- `Style:` Code style changes

**Examples:**
```
Feature: Add dash ability to player

Adds a dash ability that propels the player forward quickly.
Includes cooldown system and visual feedback.

Implements #45
```

```
Bugfix: Fix bot pathfinding on slopes

Bots were getting stuck on steep slopes. Added slope
angle check and adjusted CharacterController parameters.

Fixes #67
```

#### Pull Request Process

1. **Update your branch** with latest main
   ```bash
   git checkout main
   git pull upstream main
   git checkout feature/your-feature
   git rebase main
   ```

2. **Test thoroughly** before submitting

3. **Create Pull Request** with:
   - Clear title and description
   - List of changes
   - Screenshots/videos for visual changes
   - Link to related issues
   - Testing performed

**Pull Request Template:**
```markdown
## Description
Brief description of changes

## Changes
- Change 1
- Change 2
- Change 3

## Testing
- [x] Tested in Unity Editor
- [x] Tested on Android/iOS
- [x] No console errors
- [x] Performance tested

## Screenshots
Add screenshots for visual changes

## Related Issues
Fixes #123
Implements #456

## Checklist
- [x] Code follows style guidelines
- [x] Documentation updated
- [x] CHANGELOG.md updated
- [x] No warnings in Console
- [x] Mobile performance acceptable
```

4. **Address review feedback** promptly

5. **Squash commits** if requested before merge

### Code Review Guidelines

**For Reviewers:**
- Be constructive and respectful
- Focus on code quality and maintainability
- Consider mobile performance implications
- Check for proper documentation
- Test the changes if possible
- Approve when satisfied or request changes clearly

**Review Checklist:**
- [ ] Code follows project style guidelines
- [ ] Changes are well-documented
- [ ] No obvious bugs or issues
- [ ] Mobile performance considered
- [ ] Appropriate error handling
- [ ] No unnecessary dependencies added
- [ ] Documentation updated if needed

## 🎨 Asset Contributions

### 3D Models
- Use low-poly models (mobile-optimized)
- Include proper UV mapping
- Provide in .fbx or .obj format
- Maximum 2000 triangles for characters
- Include diffuse texture (512x512 or 1024x1024)

### Textures
- Use compressed formats (ASTC/ETC2)
- Keep resolution appropriate (512-2048)
- Include normal maps if needed
- Provide source files if possible

### Audio
- Use compressed formats (.ogg or .mp3)
- Normalize audio levels
- Keep file sizes small
- Provide attribution for non-original content

### UI
- Design for 1920x1080 base resolution
- Consider various aspect ratios
- Keep elements readable on small screens
- Use mobile-friendly button sizes (minimum 100x100)

## 🏷️ Issue Labels

- `bug`: Something isn't working
- `feature`: New feature request
- `enhancement`: Improvement to existing feature
- `documentation`: Documentation improvements
- `performance`: Performance-related issues
- `mobile`: Mobile-specific issues
- `ui`: User interface related
- `good first issue`: Good for newcomers
- `help wanted`: Need community help

## 📜 Code of Conduct

### Our Standards

**Positive behavior:**
- Using welcoming and inclusive language
- Being respectful of differing viewpoints
- Gracefully accepting constructive criticism
- Focusing on what's best for the community
- Showing empathy towards others

**Unacceptable behavior:**
- Trolling, insulting, or derogatory comments
- Public or private harassment
- Publishing others' private information
- Other conduct reasonably considered inappropriate

### Enforcement

Violations may result in:
1. Warning
2. Temporary ban
3. Permanent ban

## ❓ Questions?

- Check documentation first (README, SETUP_GUIDE, etc.)
- Search existing issues
- Ask in discussions or create a question issue

## 🙏 Recognition

Contributors will be recognized in:
- README.md contributors section
- Release notes
- Project credits

Thank you for contributing to PixelRivals! 🎮
