## Context

See `proposal.md` for motivation and background. The `Goblin` entity possesses sensory detection (`HearingArea`, `VisionArea`) and state machine states (`EnemySleeping`, `EnemyStanding`, `EnemyChasing`, `EnemyAttacking`, `EnemyFalling`, `EnemyDying`). Integrating an active attack state required defining strike conditions, hitbox lifecycle, elevation filtering, and animation synchronization.

## Goals / Non-Goals

**Goals:**
- Implement a reusable, modular `EnemyAttacking.cs` state component for melee strikes.
- Halt horizontal movement on attack start, face the target, and play the `"Stabbing"` animation.
- Activate the `AttackHitbox` strictly on active strike frames (Frame 1) to deliver 2 damage and knockback via the combat pipeline.
- Separate body contact damage (`ContactHitbox`, 1 DMG) from active weapon damage (`AttackHitbox`, 2 DMG).
- Account for vertical elevation to prevent infinite attack loops when the player stands on platforms above.
- Transition cleanly back to `EnemyStanding` / `EnemyChasing` once the attack animation finishes.
- Safely transition to `EnemyFalling` if the enemy loses ground contact during the attack.
- Ensure overlapping hurtboxes take damage when player invulnerability expires.

**Non-Goals:**
- Projectile / ranged enemy attacks (covered by separate enemy archetypes).
- Complex multi-hit combo patterns or directional vertical stabbing.
- Enemy hurt/stun state (damage handling on enemies is tracked under generic enemy health).

## Decisions

### 1. Dedicated `EnemyAttacking` State Class
- **Decision**: Implement `EnemyAttacking` inheriting from `State` in `scripts/entities/enemy/states/EnemyAttacking.cs`.
- **Rationale**: Keeps enemy behaviors modular and decoupled. Other melee enemies can reuse `EnemyAttacking` by pointing to their respective attack animations and hitboxes.

### 2. Dual Hitbox Architecture & Frame-Accurate Timing
- **Decision**:
  - `ContactHitbox`: Persistent body area (8×7 px, 1 DMG, 80 force) on Layer 32 / Mask 16.
  - `AttackHitbox`: Spear area (10×4 px, 2 DMG, 100 force) positioned at `X = ±7, Y = -2.5`.
  - `EnemyAttacking` uses `FrameChanged` to enable `AttackHitbox` only on Frame 1 (full thrust) and keep it disabled during windup, retract, and recovery frames.
- **Rationale**: Differentiates casual body contact from intentional weapon strikes while ensuring fair dodge windows.

### 3. Vertical Elevation Filtering & Deadzone Deceleration
- **Decision**: In `EnemyChasing.cs`, check `_reachThreshold = 13f` and `_verticalReachThreshold = 8f`. If horizontally aligned beneath the player, decelerate into `_idleAnimation = "Standing"` when within `_targetDeadzone = 2f`.
- **Rationale**: Eliminates false attack spamming when the target is out of reach on a ledge above.

### 4. Overlap Handling on Invulnerability Expiration
- **Decision**: Add `Hurtbox.CheckOverlapping()` called from `PlayerHurt.cs` when the blink tween completes.
- **Rationale**: Resolves the Godot edge case where entities already inside a persistent hitbox do not re-trigger `AreaEntered` when invulnerability ends.

### 5. Transition Wiring in `Goblin.tscn`
- **Decision**: 
  - Wire `EnemyStanding._onTargetSpotted` $\rightarrow$ `State/Chasing`.
  - Wire `EnemyChasing._onTargetReached` $\rightarrow$ `State/Attacking`.
  - Wire `EnemyAttacking._onComplete` $\rightarrow$ `State/Standing`.
  - Wire `EnemyAttacking._onFall` $\rightarrow$ `State/Falling`.
- **Rationale**: Seamlessly integrates into existing FSM flow without changing the core coordinator `Enemy.cs`.

## Risks / Trade-offs

- **[Risk] Rapid attack spamming**: Addressed naturally through the 4-frame animation cycle (0.8s total duration: 0.2s windup, 0.2s strike, 0.4s recovery) and knockback spacing.
