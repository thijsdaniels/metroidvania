# Proposal: In-Game HUD Framework and Player Health Visualization

## Why

With the core player health and damage pipeline (`Damageable`, `Hitbox`, `Hurtbox`, `PlayerHurt`) implemented, players currently have no visual feedback on screen representing their remaining hit points, damage taken, or healing. A clean in-game HUD overlay is required to communicate the player's current and maximum health in real time during gameplay.

This change directly addresses the core HUD framework and health display requirements of GitHub **Issue #29**: *Implement In-Game HUD Framework and Player Health Display* (with stamina, active items, and keys split into standalone issues [#48](https://github.com/thijsdaniels/zeldavania/issues/48), [#49](https://github.com/thijsdaniels/zeldavania/issues/49), and [#50](https://github.com/thijsdaniels/zeldavania/issues/50)).

## What Changes

- **In-Game HUD Canvas Layer (`Hud.tscn` / `Hud.cs`)**: Creates a UI canvas layer and controller positioned at the top of the screen that remains fixed relative to the viewport.
- **Pixel-Art Health Bar (`HealthBar.cs`)**: Compact, pixel-perfect health bar widget with 1px border, dark background underlay, and proportional red health fill.
- **Dynamic Health Synchronization**: Subscribes to the player's `Damageable.OnHealthChanged` and `Damageable.OnDepleted` signals to automatically update the health bar when taking damage, healing, or resetting upon defeat.

## Capabilities

### New Capabilities
- `in-game-hud`: Defines visual HUD elements, specifically the viewport HUD overlay framework, dynamic player health visualization via a pixel-art health bar, and synchronization with entity health state.

### Modified Capabilities
*(None - no existing spec requirements are changing)*

## Impact

- **UI Scenes & Scripts**: Creates `scenes/userInterface/Hud.tscn` and `scripts/userInterface/HealthBar.cs`, `scripts/userInterface/Hud.cs`.
- **Game / Test World Integration**: Adds the HUD canvas layer instance to active game/test levels (`World.tscn`).
- **Signal Subscriptions**: Connects to `Player` / `Damageable` health signals without coupling UI logic to player physics or movement.
