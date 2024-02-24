using Godot;

public partial class PlayerFalling : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Falling")]
    [Export]
    private float _gravity = 300;

    [Export]
    private float _terminalVelocity = 100;

    [ExportGroup("Moving")]
    [Export]
    private float _maximumVelocity = 50;

    [Export]
    private float _traction = 600;

    [Export]
    private float _friction = 400;

    [ExportGroup("Jumping")]
    [Export]
    private State _airJumpingState;

    [Export]
    private int _airJumps = 1;

    [ExportGroup("Landing")]
    [Export]
    private State _landingState;

    [ExportGroup("Swimming")]
    [Export]
    private State _swimmingState;

    [Export]
    private TileDetector2D _waterDetector;

    [ExportGroup("Climbing")]
    [Export]
    private State _climbingState;

    [Export]
    private TileDetector2D _ladderDetector;

    private int _airJumpsRemaining;

    public override void _Ready()
    {
        _airJumpsRemaining = _airJumps;
    }

    public override void Enter()
    {
        _sprite.Play("Fall");
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true
                when Input.IsActionJustPressed("Jump")
                    && _airJumpsRemaining > 0:
                _airJumpsRemaining--;
                Transition(_airJumpingState);
                break;

            case true when _body.IsOnFloor():
                _airJumpsRemaining = _airJumps;
                Transition(_landingState);
                break;

            case true when _waterDetector.IsOverlapping:
                Transition(_swimmingState);
                break;

            case true
                when _ladderDetector.IsOverlapping
                    && (
                        Input.IsActionPressed("MoveUp")
                        || Input.IsActionPressed("MoveDown")
                    ):
                Transition(_climbingState);
                break;

            default:
                Fall(delta);
                Move(delta);
                break;
        }
    }

    private void Fall(double delta)
    {
        _body.Velocity = new Vector2(
            _body.Velocity.X,
            Mathf.Min(
                _body.Velocity.Y + (_gravity * (float)delta),
                _terminalVelocity
            )
        );
    }

    private void Move(double delta)
    {
        float input = Input.GetAxis("MoveLeft", "MoveRight");

        if (input == 0)
            Decelerate(delta);
        else
            Accelerate(input, delta);

        _sprite.SpeedScale = Mathf.Abs(input);
    }

    private void Decelerate(double delta)
    {
        _body.Velocity = new Vector2(
            Mathf.MoveToward(_body.Velocity.X, 0, _friction * (float)delta),
            _body.Velocity.Y
        );
    }

    private void Accelerate(float input, double delta)
    {
        _sprite.FlipH = input > 0;

        if (
            (input < 0 && _body.Velocity.X > -_maximumVelocity)
            || (input > 0 && _body.Velocity.X < _maximumVelocity)
        )
        {
            _body.Velocity += new Vector2(input * _traction * (float)delta, 0);
        }
    }
}
