using Godot;

public partial class PlayerFall : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private float _fallAcceleration = 300;

    [Export]
    private float _terminalVelocity = 100;

    [Export]
    private float _moveVelocity = 50;

    [Export]
    private int _airJumps = 1;

    [Export]
    private State _airJumpState;

    [Export]
    private State _landState;

    [Export]
    private State _swimState;

    [Export]
    private TileDetector2D _waterDetector;

    [Export]
    private State _climbState;

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
                Transition(_airJumpState);
                break;

            case true when _body.IsOnFloor():
                _airJumpsRemaining = _airJumps;
                Transition(_landState);
                break;

            case true when _waterDetector.IsOverlapping:
                Transition(_swimState);
                break;

            case true
                when _ladderDetector.IsOverlapping
                    && (
                        Input.IsActionPressed("MoveUp")
                        || Input.IsActionPressed("MoveDown")
                    ):
                Transition(_climbState);
                break;

            default:
                Fall(delta);
                Move();
                break;
        }
    }

    private void Fall(double delta)
    {
        _body.Velocity = new Vector2(
            _body.Velocity.X,
            Mathf.Min(
                _body.Velocity.Y + (_fallAcceleration * (float)delta),
                _terminalVelocity
            )
        );
    }

    private void Move()
    {
        float x = Input.GetAxis("MoveLeft", "MoveRight");

        if (x == 0)
        {
            _body.Velocity = new Vector2(
                Mathf.MoveToward(_body.Velocity.X, 0, _moveVelocity),
                _body.Velocity.Y
            );
        }
        else
        {
            _body.Velocity = new Vector2(x * _moveVelocity, _body.Velocity.Y);

            if (x > 0)
                _sprite.FlipH = true;
            if (x < 0)
                _sprite.FlipH = false;
        }
    }
}
