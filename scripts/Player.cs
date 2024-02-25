using Godot;

public partial class Player : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        /// @todo I should probably let the states determine whether the player
        /// can drop through the one-way-collision tiles or not.
        this.DropThroughFloor();

        MoveAndSlide();
    }
}
