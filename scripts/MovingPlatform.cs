using Godot;

public partial class MovingPlatform : Path2D
{
    [Export]
    private float _velocity = 16;

    private PathFollow2D _follower;
    private Line2D _line;

    public override void _Ready()
    {
        _follower = GetNode<PathFollow2D>("PathFollow2D");
        _line = GetNode<Line2D>("Line2D");

        foreach (Vector2 point in Curve.GetBakedPoints())
        {
            _line.AddPoint(point);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // @todo Should this loop using a modulo operator to avoid overflow?
        _follower.Progress += _velocity * (float)delta;
    }
}
