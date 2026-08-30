# Enemy Behavior Specification

## Purpose
Coordinates sensory perception (hearing, vision) and state machine behaviors for generic ground enemies, including idling/sleeping, waking on alert, target chasing, falling, and defeat.

## Requirements

### Requirement: Sensory Perception & Target Tracking
The `Enemy` coordinator SHALL detect the player through hearing and vision trigger areas.

#### Scenario: Hearing detection
- **GIVEN** an enemy in a resting or idling state
- **WHEN** the `Player` enters the enemy's `HearingArea`
- **THEN** the enemy emits the `OnAlerted` signal passing the player reference.

#### Scenario: Vision spotting and losing target
- **GIVEN** an enemy with an active `VisionArea`
- **WHEN** the `Player` enters the `VisionArea` and the enemy has no target
- **THEN** the enemy sets `Target = player`
- **AND** emits the `OnTargetSpotted` signal.
- **WHEN** the tracked `Target` exits the `VisionArea`
- **THEN** the enemy sets `Target = null`
- **AND** emits the `OnTargetLost` signal.

### Requirement: Enemy State Machine Behaviors
Enemies SHALL transition between modular states depending on sensory signals and environment.

#### Scenario: Sleeping and waking
- **GIVEN** an enemy in `EnemySleeping` state with vision monitoring disabled
- **WHEN** the `OnAlerted` signal is received
- **THEN** the enemy transitions to its configured wake state (`_onWake`)
- **AND** re-enables vision monitoring on state exit.

#### Scenario: Target chasing & navigation
- **GIVEN** an enemy in `EnemyRunning` state with a valid `Target`
- **WHEN** updating physics
- **THEN** the enemy accelerates horizontally toward the target's position up to its speed limit
- **WHEN** distance to target falls below the reach threshold
- **THEN** the enemy transitions to `_onTargetReached`.
- **WHEN** the target is lost (`Target == null`)
- **THEN** the enemy transitions to `_onTargetLost`.

#### Scenario: Airborne enemy falling
- **GIVEN** an enemy in any grounded state
- **WHEN** `IsOnFloor() == false`
- **THEN** the enemy transitions to `EnemyFalling` and applies gravity until grounded.

#### Scenario: Defeat and cleanup
- **GIVEN** an enemy taking fatal damage
- **WHEN** transitioning to `EnemyDying`
- **THEN** the enemy plays its death animation and queues removal from the scene tree.
