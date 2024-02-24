using Godot;

public partial class PlayerRun : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private TileDetector2D _ladderDetector;

    [Export]
    private float _velocity = 70;

    [Export]
    private State _idleState;

    [Export]
    private State _fallState;

    [Export]
    private State _jumpState;

    [Export]
    private State _climbState;

    [Export]
    private State _crouchState;

    public override void Enter()
    {
        _sprite.Play("Run");
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true when !_body.IsOnFloor():
                Transition(_fallState);
                break;

            case true when Input.IsActionJustPressed("Jump"):
                Transition(_jumpState);
                break;

            case true
                when _ladderDetector.IsOverlapping
                    && Input.IsActionPressed("MoveUp"):
                Transition(_climbState);
                break;

            case true when Input.IsActionJustPressed("MoveDown"):
                Transition(_crouchState);
                break;

            default:
                Run();
                break;
        }
    }

    private void Run()
    {
        float x = Input.GetAxis("MoveLeft", "MoveRight");

        if (x == 0)
        {
            if (_body.Velocity.X == 0)
            {
                Transition(_idleState);
                return;
            }

            _body.Velocity = new Vector2(
                Mathf.MoveToward(_body.Velocity.X, 0, _velocity),
                _body.Velocity.Y
            );
        }
        else
        {
            _body.Velocity = new Vector2(x * _velocity, _body.Velocity.Y);

            if (x > 0)
                _sprite.FlipH = true;
            if (x < 0)
                _sprite.FlipH = false;
        }

        _sprite.SpeedScale = Mathf.Abs(x);
    }
}
