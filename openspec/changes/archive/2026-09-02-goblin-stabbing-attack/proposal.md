# Proposal: Add Stabbing Attack State to Goblin Enemy

## Why

With player health, damage reactions (`Damageable`, `Hitbox`, `Hurtbox`, `PlayerHurt`), and the in-game HUD health bar in place, the Goblin enemy previously chased the player but lacked an offensive melee attack to deal damage during close combat. The Goblin sprite sheet includes a 4-frame `"Stabbing"` animation in `Goblin.tscn`. Implementing an intentional stabbing attack state closes the combat loop, allowing the enemy to engage and damage the player in close proximity with distinct attack and contact damage.

This change directly addresses GitHub **Issue #47**: *Add stabbing attack state to Goblin enemy*.

## What Changes

- **Enemy Attacking State (`EnemyAttacking.cs`)**:
  - Implements a new modular `State` node for melee attacks.
  - Plays the `"Stabbing"` animation on enter and halts horizontal movement during the strike.
  - Synchronizes orientation to face the target at the start of the attack.
  - Enables the attack `Hitbox` strictly during active thrust damage frames (Frame 1) and deactivates it during windup and recovery.
  - Transitions to follow-up states (`_onComplete` to `EnemyStanding`, `_onFall` if knocked airborne).
- **Enemy Chasing Refactor (`EnemyChasing.cs`)**:
  - Renamed from `EnemyRunning` to `EnemyChasing` to accurately reflect chasing and stationary pursuit states.
  - Adds horizontal and vertical reach thresholds (`_reachThreshold = 13f`, `_verticalReachThreshold = 8f`) to prevent false attack loops when target is elevated on ledges.
  - Adds configurable deadzone deceleration (`_targetDeadzone = 2f`) with velocity-scaled animation playback.
- **Dual Hitbox Architecture (`Goblin.tscn`)**:
  - `ContactHitbox`: Full-body passive collision (1 DMG, 80 knockback) active during normal movement.
  - `AttackHitbox`: Spear thrust strike (2 DMG, 100 knockback) covering the 10×4 px spear shaft, active exclusively during Frame 1.
  - Frame sequence reordered and sprite atlas Y-offset adjusted by 1px for grounded stance alignment.
- **Combat Pipeline Enhancements (`Hurtbox.cs` & `PlayerHurt.cs`)**:
  - Adds `CheckOverlapping()` to `Hurtbox` to immediately process contact damage when player invulnerability expires inside an enemy hitbox.
  - Tuned player invulnerability duration to `0.75f`.

## Capabilities

### New Capabilities
*(None - extends existing enemy behavior and combat traits)*

### Modified Capabilities
- `enemy-behavior`: Adds requirement for melee attack execution, vertical elevation checking, frame-accurate hitbox activation, and transition flow from chase to attack.

## Impact

- **Enemy Scripts**: Creates `scripts/entities/enemy/states/EnemyAttacking.cs` and renames `EnemyRunning.cs` $\to$ `EnemyChasing.cs`.
- **Enemy Scenes**: Updates `scenes/entities/Goblin.tscn` to wire `Attacking` and `Chasing` states, configure dual hitboxes, and correct frame sequence.
- **Combat Pipeline**: Updates `Hurtbox.cs` and `PlayerHurt.cs` for reliable post-invulnerability damage overlap.
