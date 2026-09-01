# Proposal: Implement Player Damage, Hurt State & Damageable Architecture

## Why
Currently, player-enemy interactions lack damage mechanics. When an enemy (such as the Goblin) collides with the player, physics collision occurs but no health is deducted, no knockback is applied, and the player cannot take damage or trigger invulnerability frames. Furthermore, there is no standardized `Damageable`, `Hitbox`, or `Hurtbox` node for entities to communicate damage across combat and environmental interactions.

This change directly addresses and resolves GitHub **Issue #34**: *Implement Player Hurt State, Knockback, Invulnerability Frames, and Screen Shake*.

## What Changes
- **`Damageable` Node**: Universal node managing hit points (`MaxHitPoints`, `CurrentHitPoints`), healing, damage handling, and emitting `OnHealthChanged` / `OnDamaged` / `OnDepleted` signals for both living entities and breakable props.
- **`Hitbox` & `Hurtbox` Nodes**: Standardized `Area2D` sub-nodes with collision layers for combat detection and damage payloads (damage amount, knockback force, source position).
- **`PlayerHurt` State**: A dedicated FSM state for the player when taking damage:
  - Deducts health via `Damageable`.
  - Applies directional knockback away from the damage source.
  - Locks player directional input during brief hitstun.
  - Grants temporary invulnerability frames (i-frames) with visual sprite flashing/blinking feedback.
- **Goblin Enemy Hitbox Wiring**: Equips the Goblin enemy entity with a `Hitbox` to deal damage on contact with the player's `Hurtbox`.

## Capabilities

### New Capabilities
- `combat-traits`: Defines universal hit point tracking (`Damageable`) and combat collision areas (`Hitbox`, `Hurtbox`) with damage payload routing.
- `player-health`: Defines player-specific health handling, `PlayerHurt` reaction state, knockback physics, and visual invulnerability feedback.

### Modified Capabilities
*(None - no existing spec requirements are changing)*

## Impact
- **Player Controller & FSM**: Adds `PlayerHurt` state in `scripts/entities/player/states/PlayerHurt.cs` and updates `Player.cs` node references.
- **New Nodes**: Creates reusable `Damageable`, `Hitbox`, and `Hurtbox` scripts in `scripts/entities/traits/` (or `scripts/nodes/`).
- **Scene Wiring**: Updates `Player.tscn` and `Goblin.tscn` to attach hitbox/hurtbox/damageable nodes.
- **Dependencies**: Lays prerequisite foundation for HUD Hearts display (Issue #29) and Checkpoint/Respawn on defeat (Issue #28).
