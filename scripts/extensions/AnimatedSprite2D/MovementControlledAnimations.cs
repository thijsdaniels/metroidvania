using Godot;

public static class MovementControlledAnimations
{
    public static void SynchronizeAnimation(
        this AnimatedSprite2D sprite,
        float direction
    )
    {
        SynchronizeAnimation(sprite, new Vector2(direction, 0));
    }

    public static void SynchronizeAnimation(
        this AnimatedSprite2D sprite,
        Vector2 direction
    )
    {
        sprite.SpeedScale = direction.Length();

        if (direction.X != 0)
            sprite.FlipH = direction.X > 0;
    }
}
