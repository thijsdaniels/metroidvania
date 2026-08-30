# Moving Platforms Specification

## Purpose
Provides path-following moving platforms for level navigation and dynamic world traversal.

## Requirements

### Requirement: Path-Following Platform Movement
The `MovingPlatform` node SHALL traverse along its configured curve path and visually render its path trajectory.

#### Scenario: Rendering the platform path
- **GIVEN** a `MovingPlatform` node inheriting from `Path2D` with baked curve points
- **WHEN** the platform executes `_Ready()`
- **THEN** it adds all baked curve points to its child `Line2D` node to render the path line.

#### Scenario: Progressing along the path
- **GIVEN** an active `MovingPlatform` node with a child `PathFollow2D`
- **WHEN** the game physics process runs with delta time
- **THEN** the platform advances `_follower.Progress` by `_velocity * delta`.
