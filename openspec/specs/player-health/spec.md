# Player Health Specification

## Purpose

Defines player-specific health handling, `PlayerHurt` reaction state in the finite state machine, knockback physics, input locking during hitstun, visual invulnerability blinking feedback, and defeat audio/respawn handling.

## Requirements

### Requirement: Player Hurt Reaction and Knockback
The player controller SHALL enter a dedicated `PlayerHurt` state upon receiving damage, applying directional knockback and temporary input locking.

#### Scenario: Player enters hurt state on taking damage
- **GIVEN** the player is in any controllable state (`PlayerStanding`, `PlayerRunning`, `PlayerJumping`, `PlayerFalling`, `PlayerCrouching`, etc.)
- **WHEN** the player's `Hurtbox` receives damage
- **THEN** the finite state machine transitions the player to `PlayerHurt` state
- **AND** player directional input is locked during hitstun
- **AND** a knockback impulse is applied to the player's body directed away from the damage source.

#### Scenario: Player recovers from hurt state
- **GIVEN** the player is in `PlayerHurt` state
- **WHEN** the hitstun duration expires
- **THEN** the player transitions to `PlayerStanding` (if on floor) or `PlayerFalling` (if airborne)
- **AND** player input control is restored.

### Requirement: Player Invulnerability Frames and Feedback
The player controller SHALL trigger temporary invulnerability and visual sprite blinking following damage.

#### Scenario: Invulnerability window on hit
- **GIVEN** the player enters `PlayerHurt` state
- **WHEN** damage is taken
- **THEN** the player's `Hurtbox` enters invulnerability (`IsInvulnerable = true`) for a configurable duration (e.g., 1.2 seconds)
- **AND** `IsInvulnerable` is set back to `false` once the timer expires.

#### Scenario: Visual sprite blinking and audio feedback during invulnerability
- **GIVEN** the player's `Hurtbox` is in an invulnerable state following damage
- **WHEN** invulnerability is active
- **THEN** a hit sound effect is played
- **AND** the animated sprite flashes/blinks visibly for the duration of the invulnerability window
- **AND** sprite visibility is fully restored once the timer expires.

### Requirement: Player Defeat and Reset
The player controller SHALL handle player defeat when hit points reach zero by resetting to the initial spawn position.

#### Scenario: Player health depletes to zero
- **GIVEN** the player takes damage reducing `CurrentHitPoints` to 0
- **WHEN** the `OnDepleted` signal is received
- **THEN** a defeat sound effect is played
- **AND** player input is temporarily locked
- **AND** the player's position is reset to the room spawn point with hit points fully restored.
