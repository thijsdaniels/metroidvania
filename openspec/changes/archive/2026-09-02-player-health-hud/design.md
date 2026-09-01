## Context

See `proposal.md` for motivation and background. The player character currently has a functional `Damageable` component managing `MaxHitPoints` and `CurrentHitPoints`, emitting `OnHealthChanged(int currentHitPoints, int maxHitPoints)` and `OnDepleted()`.

## Goals / Non-Goals

**Goals:**
- Implement an in-game HUD (`CanvasLayer`) displaying the player's health in the top-left viewport corner.
- Implement a pixel-perfect, clean retro health bar (`HealthBar.cs`) with 1px border, background underlay, and proportional red fill.
- Automatically synchronize display with `Damageable.OnHealthChanged` without tight coupling to player physics.

**Non-Goals:**
- Stamina/magic meter (split into Issue [#48](https://github.com/thijsdaniels/zeldavania/issues/48)).
- Active item slot and ammo badges (split into Issue [#49](https://github.com/thijsdaniels/zeldavania/issues/49)).
- Dungeon small keys and currency counter (split into Issue [#50](https://github.com/thijsdaniels/zeldavania/issues/50)).
- Health Container collectible item progression logic (tracked separately under Issue [#43](https://github.com/thijsdaniels/zeldavania/issues/43)).

## Decisions

### 1. CanvasLayer + Control Architecture
- **Decision**: Implement `Hud.tscn` as a `CanvasLayer` containing a `MarginContainer` hosting `HealthBar`.
- **Rationale**: `CanvasLayer` guarantees viewport-fixed UI rendering independent of camera position, zooming, or parallax scrolling.

### 2. Custom-Drawn Pixel-Art Health Bar
- **Decision**: Implement `HealthBar.cs` inheriting from `Control` utilizing pixel-aligned `_Draw()` rendering.
  - Dimensions: 40×5 px (`BarSize`).
  - Outline: 1px `#18101e` (`BorderColor`).
  - Underlay: Dark `#281420` (`BackgroundColor`).
  - Fill: Vibrant red `#df2438` (`FillColor`).
- **Rationale**: Custom drawing guarantees 1:1 integer pixel alignment with zero texture sampling distortion or subpixel blur, perfectly matching the 8×8 world tile aesthetic.

### 3. Signal Connection & Binding
- **Decision**: Provide `[Export] private Damageable _playerDamageable` on `Hud.cs` with an optional `Bind(Damageable damageable)` method.
- **Rationale**: Allows assigning references directly in the inspector for static test scenes while supporting dynamic assignment in procedural level transitions.

## Risks / Trade-offs

- **[Risk] Window scaling distortion**: UI controls could blur if not integer-snapped.
  - *Mitigation*: Pixel math rounds fill width to whole integers and draws with 1px line widths.
