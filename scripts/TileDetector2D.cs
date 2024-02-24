using Godot;

public partial class TileDetector2D : Area2D
{
    public bool IsOverlapping = false;

    public override void _Ready()
    {
        BodyEntered += OnArea2DBodyEntered;
        BodyExited += OnArea2DBodyExited;
    }

    [Signal]
    public delegate void OnTileEnteredEventHandler();

    [Signal]
    public delegate void OnTileExitedEventHandler();

    public void OnArea2DBodyEntered(Node2D body)
    {
        if (!IsOverlapping)
        {
            var bodies = GetOverlappingBodies();

            if (bodies.Count > 0)
            {
                IsOverlapping = true;
                EmitSignal("OnTileEntered");
            }
        }
    }

    public void OnArea2DBodyExited(Node2D body)
    {
        if (IsOverlapping)
        {
            var bodies = GetOverlappingBodies();

            if (bodies.Count == 0)
            {
                IsOverlapping = false;
                EmitSignal("OnTileExited");
            }
        }
    }
}
