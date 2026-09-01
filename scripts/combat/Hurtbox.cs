using Godot;

namespace Zeldavania.Combat;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Signal]
    public delegate void OnHurtEventHandler(int damage, Vector2 origin, float force);

    [Export]
    private Damageable _damageable;

    public bool IsInvulnerable { get; set; } = false;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;

        if (_damageable == null)
        {
            GD.PushError($"[Hurtbox] '{GetPath()}' has no Damageable node assigned in the Inspector!");
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Hitbox hitbox)
        {
            ReceiveHit(new Hit(hitbox.Damage, hitbox.GlobalPosition, hitbox.Force));
        }
    }

    public void ReceiveHit(Hit hit)
    {
        if (IsInvulnerable || hit.Damage <= 0)
        {
            return;
        }

        EmitSignal(SignalName.OnHurt, hit.Damage, hit.Origin, hit.Force);
        _damageable?.TakeDamage(hit);
    }
}
