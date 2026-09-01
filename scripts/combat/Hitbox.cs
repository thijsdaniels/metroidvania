using Godot;

namespace Zeldavania.Combat;

[GlobalClass]
public partial class Hitbox : Area2D
{
    [Export]
    public int Damage { get; set; } = 1;

    [Export]
    public float Force { get; set; } = 250f;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Hurtbox hurtbox)
        {
            var hit = new Hit(Damage, GlobalPosition, Force);
            hurtbox.ReceiveHit(hit);
        }
    }
}
