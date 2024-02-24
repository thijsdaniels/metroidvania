using Godot;

public partial class PlayerStanding : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Standing")]
    // @todo Get this value from tile metadata.
    [Export]
    private float _friction = 500;

    [ExportGroup("Climbing")]
    [Export]
    private State _climbingState;

    [Export]
    private TileDetector2D _ladderDetector;

    [ExportGroup("Falling")]
    [Export]
    private State _fallingState;

    [ExportGroup("Running")]
    [Export]
    private State _runningState;

    [ExportGroup("Jumping")]
    [Export]
    private State _jumpingState;

    [ExportGroup("Crouching")]
    [Export]
    private State _crouchingState;

    public override void Enter()
    {
        _sprite.Play("Idle");
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true when !_body.IsOnFloor():
                Transition(_fallingState);
                break;

            case true
                when Input.IsActionPressed("MoveLeft")
                    || Input.IsActionPressed("MoveRight"):
                Transition(_runningState);
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

            // case true when Input.IsActionJustPressed("Attack"):
            //     Transition(_attackState);
            //     break;

            // case true when Input.IsActionJustPressed("Interact"):
            //     Transition(_interactState);
            //     break;}

            default:
                Stand();
                break;
        }

        void Stand()
        {
            _body.Velocity = new Vector2(
                Mathf.MoveToward(_body.Velocity.X, 0, _friction * (float)delta),
                _body.Velocity.Y
            );
        }
    }
}
