# Pause Menu Specification

## Purpose
Provides in-game pause menu controls, freezing scene tree execution, animating menu transitions, and providing options to resume, restart the level, or quit the application.

## Requirements

### Requirement: Game Pause Toggling
The `PauseMenu` component SHALL toggle the game pause state and animate UI transitions in response to user input.

#### Scenario: Pausing the game
- **GIVEN** the game is currently unpaused and the pause menu is hidden
- **WHEN** the "Start" input action is pressed
- **THEN** the scene tree pause state (`GetTree().Paused`) is set to true
- **AND** the menu becomes visible
- **AND** the animation player plays the "Open" animation.

#### Scenario: Resuming the game via input toggle
- **GIVEN** the game is currently paused with the pause menu open
- **WHEN** the "Start" input action is pressed
- **THEN** the scene tree pause state is set to false
- **AND** the menu is hidden
- **AND** the animation player plays the "Open" animation backwards.

### Requirement: Menu Button Actions
The `PauseMenu` SHALL handle interactive button events to resume gameplay, restart the scene, or quit.

#### Scenario: Resuming gameplay via button
- **GIVEN** the pause menu is open
- **WHEN** the Resume button is pressed
- **THEN** the game unpauses and the menu is closed.

#### Scenario: Restarting current scene
- **GIVEN** the pause menu is open
- **WHEN** the Restart button is pressed
- **THEN** the game unpauses
- **AND** the scene tree reloads the active scene (`GetTree().ReloadCurrentScene()`).

#### Scenario: Quitting application
- **GIVEN** the pause menu is open
- **WHEN** the Quit button is pressed
- **THEN** the application requests termination (`GetTree().Quit()`).
