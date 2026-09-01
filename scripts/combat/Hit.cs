using Godot;

namespace Zeldavania.Combat;

/// <summary>
/// Represents combat strike data transmitted from a Hitbox to a Hurtbox.
/// </summary>
public readonly struct Hit
{
    public int Damage { get; init; }
    public Vector2 Origin { get; init; }
    public float Force { get; init; }

    public Hit(int damage, Vector2 origin, float force)
    {
        Damage = damage;
        Origin = origin;
        Force = force;
    }
}
