using Godot;
using Zeldavania.Combat;

namespace Zeldavania.UserInterface;

[GlobalClass]
public partial class Hud : CanvasLayer
{
    [Export]
    private Damageable _playerDamageable;

    [Export]
    private HealthBar _healthBar;

    public override void _Ready()
    {
        if (_playerDamageable != null)
        {
            Bind(_playerDamageable);
        }
    }

    public override void _ExitTree()
    {
        if (_playerDamageable != null)
        {
            _playerDamageable.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public void Bind(Damageable damageable)
    {
        if (_playerDamageable != null)
        {
            _playerDamageable.OnHealthChanged -= HandleHealthChanged;
        }

        _playerDamageable = damageable;

        if (_playerDamageable != null)
        {
            _playerDamageable.OnHealthChanged += HandleHealthChanged;
            _healthBar?.UpdateHealth(_playerDamageable.CurrentHitPoints, _playerDamageable.MaxHitPoints);
        }
    }

    private void HandleHealthChanged(int currentHitPoints, int maxHitPoints)
    {
        _healthBar?.UpdateHealth(currentHitPoints, maxHitPoints);
    }
}
