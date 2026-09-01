## Purpose

Defines universal combat building blocks including hit point management (`Damageable`), damage dealing collision areas (`Hitbox`), hit receiving collision areas (`Hurtbox`), and damage payload transmission.

## ADDED Requirements

### Requirement: Hit Point Tracking and Damage Calculation
The system SHALL provide a `Damageable` node that tracks current and maximum hit points and processes damage and healing payloads.

#### Scenario: Taking damage reduces hit points
- **GIVEN** an entity has an active `Damageable` node with `CurrentHitPoints > 0`
- **WHEN** `TakeDamage` is invoked with a positive damage amount
- **THEN** `CurrentHitPoints` is decreased by the damage amount (clamped to a minimum of 0)
- **AND** the `OnDamaged` and `OnHealthChanged` signals are emitted.

#### Scenario: Healing increases hit points
- **GIVEN** an entity has an active `Damageable` node with `CurrentHitPoints < MaxHitPoints`
- **WHEN** `Heal` is invoked with a positive healing amount
- **THEN** `CurrentHitPoints` is increased by the healing amount (clamped to `MaxHitPoints`)
- **AND** the `OnHealthChanged` signal is emitted.

#### Scenario: Hit points depleted
- **GIVEN** an entity has an active `Damageable` node with `CurrentHitPoints > 0`
- **WHEN** damage reduces `CurrentHitPoints` to 0
- **THEN** the `OnDepleted` signal is emitted.

---

### Requirement: Combat Collision and Hit Transmission
The system SHALL provide `Hitbox` and `Hurtbox` `Area2D` nodes to detect combat collisions and route `Hit` data.

#### Scenario: Hitbox strikes Hurtbox
- **GIVEN** a `Hitbox` on an attacker's collision layer overlaps a `Hurtbox` on a receiver's collision layer
- **WHEN** the overlap is detected
- **THEN** a `Hit` containing damage amount, physical force, and hit origin position is passed to the `Hurtbox`.

#### Scenario: Hurtbox receives valid hit
- **GIVEN** a `Hurtbox` is not in an invulnerable state (`IsInvulnerable == false`)
- **WHEN** a `Hit` is received
- **THEN** the `Hurtbox` forwards the hit to its assigned `Damageable` node
- **AND** emits the `OnHurt` signal.

#### Scenario: Hurtbox ignores hits during invulnerability
- **GIVEN** a `Hurtbox` is in an invulnerable state (`IsInvulnerable == true`)
- **WHEN** an overlapping `Hitbox` attempts to transmit a hit
- **THEN** the hit is ignored and no damage is dealt.
