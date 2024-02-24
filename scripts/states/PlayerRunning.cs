using Godot;

public partial class PlayerRunning : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private TileDetector2D _ladderDetector;

    [Export]
    private float _maximumVelocity = 70;

    // @todo Get this value from tile metadata.
    [Export]
    private float _traction = 800;

    [Export]
    private State _standingState;

    [Export]
    private State _fallingState;

    [Export]
    private State _jumpingState;

    [Export]
    private State _climbingState;

    [Export]
    private State _crouchingState;

    public override void Enter()
    {
        _sprite.Play("Run");
    }

    public override void UpdatePhysics(double delta)
    {
        float input = Input.GetAxis("MoveLeft", "MoveRight");

        switch (true)
        {
            case true when input == 0:
                Transition(_standingState);
                break;

            case true when !_body.IsOnFloor():
                Transition(_fallingState);
                break;

            case true when Input.IsActionJustPressed("Jump"):
                Transition(_jumpingState);
                break;

            case true
                when _ladderDetector.IsOverlapping
                    && Input.IsActionPressed("MoveUp"):
                Transition(_climbingState);
                break;

            case true when Input.IsActionJustPressed("MoveDown"):
                Transition(_crouchingState);
                break;

            default:
                Run(input, delta);
                break;
        }
    }

    private void Run(float input, double delta)
    {
        if (
            (input < 0 && _body.Velocity.X > -_maximumVelocity)
            || (input > 0 && _body.Velocity.X < _maximumVelocity)
        )
            _body.Velocity += new Vector2(_traction * (float)delta * input, 0);

        _sprite.FlipH = input > 0;
        _sprite.SpeedScale = Mathf.Abs(input);
    }
}
