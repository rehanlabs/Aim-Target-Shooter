Project Details
---------------
Unity Version: 6000.0.58f1 LTS
Recommended Resolution: 1920x1080 (Portrait)

Architectural Overview
----------------------
The project is built around decoupled communication and design patterns.

- Event-Driven: Systems communicate using a central GameEvents hub (no direct dependencies).
- Singleton Managers: Core systems are globally accessible.
- Object Pooling: Targets and projectiles are pooled for performance.
- SOLID Principles: 
  - Single Responsibility: Each script handles one main task.
  - Open/Closed: New target types can be added without changing core logic.

Core Systems (Managers)
-----------------------
GameManager.cs
  - Central game state and data manager.
  - Tracks ammo and kill count.
  - Controls game flow (Start, Game Over).
  - Game Over triggers when ammo = 0 and all active projectiles are used.

InputHandler.cs
  - Unifies PC (mouse) and Mobile (touch) input.
  - Exposes simplified states (IsPressedDown, ScreenPosition).

AudioManager.cs
  - Plays background music and sound effects.
  - Manages volume settings.

GameEvents.cs
  - Static event hub.
  - Defines and broadcasts events such as OnAmmoChanged, OnGameOver.

Gameplay Mechanics
------------------
PlayerShooter.cs
  - Reads input and raycasts from the camera.
  - Retrieves a bullet from BulletPool.
  - Fires the projectile and decrements ammo.

Projectile.cs
  - Propels the bullet forward.
  - Auto-despawns after its lifetime.
  - On collision calls ITarget.OnHit() and returns to BulletPool.

ITarget.cs
  - Interface for all targets.
  - Defines OnSpawn(), OnHit(), OnDespawn().

TargetBase.cs
  - Abstract base target.
  - Common hit logic: particle effect, GameManager.AddKill(), despawn.

SpecialTarget.cs
  - Time-limited bonus target.
  - Grants bonus ammo on hit.
  - Auto-despawns after a short duration.

MovingTarget.cs
  - Oscillating movement pattern.
  - Moves back and forth relative to its starting position.

Performance and Spawning
------------------------
BulletPool.cs
  - Pre-spawns and reuses bullet objects.
  - Tracks ActiveBulletsCount for the game-over condition.

TargetPool.cs
  - Separate pools for Static, Moving, and Special targets.
  - Provides a method to fetch the correct type.

TargetSpawner.cs
  - Controls spawn rate and random spawn locations.
  - Ammo-aware spawning (prioritizes special targets when ammo is low).
  - Increases speed of moving targets based on kill count.

Key Features
------------
- Object Pooling for smooth performance.
- Event-driven decoupled communication.
- Unified cross-platform input handling.
- Extendable target types without touching core logic.
- Dynamic difficulty scaling.

Folder Structure
----------------
Assets/
 ├── Prefabs/
 │    ├── ParticleEffects/
 │    └── Targets/
 └── Scripts/
      ├── Bullets/
      │    ├── Projectile.cs
      │    └── BulletPool.cs
      ├── Player/
      │    └── PlayerShooter.cs
      ├── System/
      │    ├── AudioManager.cs
      │    ├── GameEvents.cs
      │    ├── GameManager.cs
      │    ├── InputHandler.cs
      │    └── UIController.cs
      └── Target/
           ├── ITarget.cs
           ├── MovingTarget.cs
           ├── SpecialTarget.cs
           ├── StaticTarget.cs
           ├── TargetBase.cs
           └── TargetSpawning/
                ├── TargetPool.cs
                └── TargetSpawner.cs

How to Play
-----------
1. Press Start to begin.
2. Tap or click on targets to shoot.
3. Manage your ammo — game ends when ammo is out and all bullets are spent.
4. Hit special targets to gain bonus ammo.
