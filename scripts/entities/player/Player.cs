using Godot;

public partial class Player : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        /// @todo I should probably let the states determine whether the player
        /// can drop through the one-way-collision tiles or not.
        DropThroughFloor();

        MoveAndSlide();
    }

    protected void DropThroughFloor()
    {
        if (Input.IsActionJustPressed(Controller.Down))
        {
            SetCollisionMaskValue(2, false);

            /// @todo I don't like translating the player by 1 pixel like this,
            /// but it does mean the player instantly passes through the floor.
            /// Without it, you have to keep the down button pressed for a few
            /// frames to get the player to cross the one-way-collision margin.
            if (IsOnFloor())
                Position += new Vector2(0, 1);
        }

        if (Input.IsActionJustReleased(Controller.Down))
            SetCollisionMaskValue(2, true);
    }
}
