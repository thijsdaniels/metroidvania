# Tile Detection Specification

## Purpose
Provides an Area2D-based detection component to track when entities enter and exit interactive tile layers (such as ladders, water bodies, and hazard zones) and dispatch events without duplicate signals.

## Requirements

### Requirement: Tile and Zone Overlap Detection
The `TileDetector2D` component SHALL track body enter and exit events and emit clean signals when the active overlap state changes.

#### Scenario: Entering an interactive tile zone
- **GIVEN** an entity with an active `TileDetector2D` component and `IsOverlapping` is false
- **WHEN** the detector enters the collision area of a target tile or body
- **THEN** `IsOverlapping` becomes true
- **AND** `TileDetector2D` emits the `OnTileEntered` signal.

#### Scenario: Exiting an interactive tile zone
- **GIVEN** `IsOverlapping` is true on `TileDetector2D`
- **WHEN** all overlapping bodies leave the detector area (overlapping body count reaches 0)
- **THEN** `IsOverlapping` becomes false
- **AND** `TileDetector2D` emits the `OnTileExited` signal.
