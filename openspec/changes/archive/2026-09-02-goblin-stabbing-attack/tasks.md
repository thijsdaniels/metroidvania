## 1. Enemy Attacking & Chasing States

- [x] 1.1 Implement `EnemyAttacking` (`scripts/entities/enemy/states/EnemyAttacking.cs`) with animation playback, orientation synchronization, movement lock, frame-accurate hitbox activation (Frame 1), and recovery transitions.
- [x] 1.2 Refactor and rename `EnemyRunning` $\to$ `EnemyChasing` (`scripts/entities/enemy/states/EnemyChasing.cs`) with horizontal/vertical reach thresholds (`_reachThreshold = 13f`, `_verticalReachThreshold = 8f`) and deadzone deceleration (`_targetDeadzone = 2f`).

## 2. Scene Wiring & Visual Adjustments

- [x] 2.1 Wire `Attacking` and `Chasing` state nodes into `scenes/entities/Goblin.tscn` finite state machine.
- [x] 2.2 Configure dual hitbox architecture on `Goblin.tscn`: separate `ContactHitbox` (1 DMG, 80 force) and `AttackHitbox` (2 DMG, 100 force, 10×4 px).
- [x] 2.3 Correct `Stabbing` animation frame sequence and shift AtlasTexture coordinates by 1px vertically (Y=23) for grounded stance alignment.

## 3. Combat Pipeline Enhancements

- [x] 3.1 Implement `Hurtbox.CheckOverlapping()` and connect it to `PlayerHurt` blink completion to ensure damage triggers upon invulnerability expiration.
- [x] 3.2 Tune player invulnerability duration to 0.75s in `PlayerHurt.cs`.

## 4. Build & Integration Verification

- [x] 4.1 Format code with CSharpier and verify `dotnet build` compiles cleanly with zero warnings or errors.
- [x] 4.2 Verify Goblin stabbing attack behavior in play mode (pursuit, strike trigger at melee range, damage application on player, recovery, ledge bypass).
