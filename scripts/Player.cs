using Godot;

public partial class Player : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("MoveDown"))
        {
            SetCollisionMaskValue(2, false);
            Position += new Vector2(0, 1);
            MoveAndSlide();
        }

        if (Input.IsActionJustReleased("MoveDown"))
        {
            SetCollisionMaskValue(2, true);
        }

        MoveAndSlide();
    }
}
