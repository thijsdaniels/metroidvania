## 1. Health Bar Component

- [x] 1.1 Implement `HealthBar` (`scripts/userInterface/HealthBar.cs`) with pixel-aligned custom drawing (1px border, background underlay, red fill) and dynamic `UpdateHealth(currentHp, maxHp)`.

## 2. HUD Controller & Scene Construction

- [x] 2.1 Implement `Hud` (`scripts/userInterface/Hud.cs`) to bind `Damageable.OnHealthChanged` and update the health bar.
- [x] 2.2 Create `scenes/userInterface/Hud.tscn` with `CanvasLayer`, `MarginContainer`, and `HealthBar`.

## 3. Scene Wiring & Verification

- [x] 3.1 Wire `Hud` into `scenes/environments/World.tscn` and bind to `Player/Damageable`.
- [x] 3.2 Verify damage, healing, and defeat respawn updates in play mode.
- [x] 3.3 Format C# files and verify `dotnet build` compiles cleanly with no warnings or errors.
