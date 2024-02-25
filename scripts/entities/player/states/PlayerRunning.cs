using Godot;

public partial class PlayerRunning : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Movement")]
    [Export]
    private float _acceleration = 800;

    [Export]
    private float _deceleration = 600;

    [Export]
    private float _maximumVelocity = 70;

    [ExportGroup("Standing")]
    [Export]
    private State _standingState;

    [ExportGroup("Falling")]
    [Export]
    private State _fallingState;

    [ExportGroup("Jumping")]
    [Export]
    private State _jumpingState;

    [ExportGroup("Climbing")]
    [Export]
    private State _climbingState;

    [Export]
    private TileDetector2D _ladderDetector;

    [ExportGroup("Crouching")]
    [Export]
    private State _crouchingState;

    public override void Enter()
    {
        _sprite.Play("Run");
    }

    public override void UpdatePhysics(double delta)
    {
        float direction = Controller.GetHorizontalDirection();

        switch (true)
        {
            case true when direction == 0 && _body.Velocity.X == 0:
                Transition(_standingState);
                break;

            case true when !_body.IsOnFloor():
                Transition(_fallingState);
                break;

            case true when Input.IsActionJustPressed(Controller.A):
                Transition(_jumpingState);
                break;

            case true
                when _ladderDetector.IsOverlapping
                    && Input.IsActionPressed(Controller.Up):
                Transition(_climbingState);
                break;

            case true when Input.IsActionJustPressed(Controller.Down):
                Transition(_crouchingState);
                break;

            default:
                Run(delta, direction);
                break;
        }
    }

    private void Run(double delta, float direction)
    {
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
