using Godot;

public static class CanDropThroughFloors
{
    public static void DropThroughFloor(this CharacterBody2D body)
    {
        if (Input.IsActionJustPressed(Controller.Down))
        {
            body.SetCollisionMaskValue(2, false);

            /// @todo I don't like translating the player by 1 pixel like this,
            /// but it does mean the player instantly passes through the floor.
            /// Without it, you have to keep the down button pressed for a few
            /// frames to get the player to cross the one-way-collision margin.
            if (body.IsOnFloor())
                body.Position += new Vector2(0, 1);
        }

        if (Input.IsActionJustReleased(Controller.Down))
            body.SetCollisionMaskValue(2, true);
    }
}
