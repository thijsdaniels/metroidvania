using Godot;

public partial class PlayerFalling : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Gravity")]
    [Export]
    private float _gravity = 400;

    [Export]
    private float _terminalVelocity = 150;

    [ExportGroup("Movement")]
    [Export]
    private float _acceleration = 600;

    [Export]
    private float _deceleration = 100;

    [Export]
    private float _maximumVelocity = 50;

    [ExportGroup("Air Jumping")]
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
                when Input.IsActionJustPressed(Controller.A)
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
                        Input.IsActionPressed(Controller.Up)
                        || Input.IsActionPressed(Controller.Down)
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
        if (_body.Velocity.Y < _terminalVelocity)
            _body.Velocity += new Vector2(0, _gravity * (float)delta);
    }

    private void Move(double delta)
    {
        float direction = Controller.GetHorizontalDirection();

        _body.Accelerate(
            delta: delta,
            direction: direction,
            acceleration: _acceleration,
            deceleration: _deceleration,
            limit: _maximumVelocity
        );

        _sprite.SynchronizeAnimation(direction);
    }
}
