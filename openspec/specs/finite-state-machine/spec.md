# Finite State Machine Specification

## Purpose
Provides a modular, node-based lifecycle architecture for controlling entity behaviors (Player, Enemies, NPCs). It separates state logic into discrete child nodes and uses inspector exports for explicit dependency wiring.

## Requirements

### Requirement: State Machine Lifecycle Management
The `FiniteStateMachine` node SHALL discover, register, and execute state nodes in the scene hierarchy.

#### Scenario: Automatic child state registration
- **GIVEN** an entity scene containing a `FiniteStateMachine` node with child nodes inheriting from `State`
- **WHEN** `_Ready()` executes on the `FiniteStateMachine`
- **THEN** all child `State` nodes are registered into an internal state dictionary by name
- **AND** the state machine connects each state's `OnTransition` signal to its transition handler
- **AND** the initial state executes its `Enter()` method.

#### Scenario: Frame processing delegation
- **GIVEN** an active state within the state machine
- **WHEN** Godot's `_Process(delta)` fires on the state machine
- **THEN** the state machine delegates to `_state.UpdateGraphics(delta)`
- **WHEN** Godot's `_PhysicsProcess(delta)` fires on the state machine
- **THEN** the state machine delegates to `_state.UpdatePhysics(delta)`.

### Requirement: State Transition Protocol
States SHALL cleanly exit before entering a new target state.

#### Scenario: Transitioning between states
- **GIVEN** a current active state $A$
- **WHEN** state $A$ calls `Transition(State nextState)` with target state $B$
- **THEN** state $A$ emits `OnTransition(nextState.Name)`
- **AND** the state machine calls `Exit()` on state $A$
- **AND** the state machine sets state $B$ as active and calls `Enter()` on state $B$.

#### Scenario: Transition to non-existent state
- **GIVEN** an active state machine
- **WHEN** a transition is requested for an unregistered state name
- **THEN** the state machine throws a `KeyNotFoundException`.

### Requirement: State Base Class Encapsulation
The `State` base class SHALL encapsulate Godot's built-in process loops so individual state subclasses cannot override them directly.

#### Scenario: Sealed engine lifecycle methods
- **GIVEN** any subclass inheriting from `State`
- **THEN** `_Process(double delta)` and `_PhysicsProcess(double delta)` are marked `sealed`
- **AND** subclasses implement `UpdatePhysics(double delta)` or `UpdateGraphics(double delta)` instead.
