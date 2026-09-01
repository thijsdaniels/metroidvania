## 1. Core Combat Data & Collision Layer Setup

- [x] 1.1 Create `Hit.cs` struct with `Damage`, `Origin`, and `Force` in `scripts/combat/` and verify compilation with `dotnet build`.
- [x] 1.2 Configure 2D collision layer names in `project.godot` (Layer 5: PlayerHurtbox, Layer 6: EnemyHitbox, Layer 7: PlayerHitbox, Layer 8: EnemyHurtbox).

## 2. Generic Combat Trait Nodes

- [x] 2.1 Implement `Damageable.cs` node with `MaxHitPoints`, `CurrentHitPoints`, `TakeDamage(Hit)`, `Heal(int)`, and signals (`OnDamaged`, `OnHealthChanged`, `OnDepleted`).
- [x] 2.2 Implement `Hurtbox.cs` (`Area2D`) with `Damageable` export reference, `IsInvulnerable` toggle, and `ReceiveHit(Hit)` routing.
- [x] 2.3 Implement `Hitbox.cs` (`Area2D`) with `Damage` and `Force` exports, detecting overlapping `Hurtbox` areas in `_Ready` via `AreaEntered`.
- [x] 2.4 Verify all combat trait nodes compile cleanly and pass code formatting with `dotnet build`.

## 3. Player Health & Hurt State

- [x] 3.1 Implement `PlayerHurt.cs` state with directional knockback, hitstun duration timer, and sprite alpha blinking during invulnerability.
- [x] 3.2 Add Player defeat signal handling to reset position to room spawn point and restore hit points when `Damageable.OnDepleted` fires.
- [x] 3.3 Register `PlayerHurt` state within `Player.cs` and wire transitions back to `PlayerStanding` and `PlayerFalling`.

## 4. Scene Assembly & Entity Wiring

- [x] 4.1 Update `scenes/entities/Player.tscn` to add `Damageable` node, `Hurtbox` (Layer 5) with `CollisionShape2D`, and `PlayerHurt` state node with exported references wired.
- [x] 4.2 Update `scenes/entities/Goblin.tscn` to attach a `Hitbox` (Layer 6) with `CollisionShape2D`, setting `Damage = 1` and `Force = 180`.

## 5. Verification & Playtesting

- [x] 5.1 Run `dotnet build` to ensure zero C# compilation errors or warnings.
- [x] 5.2 Launch test scene and verify:
  - Goblin running into player triggers `PlayerHurt` state with backward/upward knockback.
  - Player sprite flashes with i-frames preventing multi-hit death.
  - Player health decreases by 1 on hit.
  - On 6th hit (0 HP), player resets to room spawn with full health.
