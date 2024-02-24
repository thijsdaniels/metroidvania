using Godot;

public partial class PlayerClimb : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private TileDetector2D _ladderDetector;

    [Export]
    private float _climbVelocity = 50;

    [Export]
    private float _leapVelocity = 40;

    [Export]
    private State _fallState;

    [Export]
    private State _jumpState;

    [Export]
    private State _idleState;

    public override void Enter()
    {
        _sprite.Play("Swim"); // @todo Change to "Climb".
        _ladderDetector.OnTileExited += OnLadderExited;
    }

    public override void Exit()
    {
        _ladderDetector.OnTileExited -= OnLadderExited;
    }

    private void OnLadderExited()
    {
        if (Input.IsActionPressed("MoveUp"))
        {
            _body.Velocity = new Vector2(
                _body.Velocity.X,
                Mathf.Min(_body.Velocity.X, -_leapVelocity)
            );
        }

        Transition(_fallState);
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true
                when _body.IsOnFloor() && Input.IsActionPressed("MoveDown"):
                Transition(_idleState);
                return;

            case true when Input.IsActionJustPressed("Jump"):
                Transition(_jumpState);
                return;

            case true when Input.IsActionJustPressed("Cancel"):
                Transition(_fallState);
                return;
        }

        Climb();
    }

    private void Climb()
    {
        Vector2 direction = Input.GetVector(
            "MoveLeft",
            "MoveRight",
            "MoveUp",
            "MoveDown"
        );

        if (direction == Vector2.Zero)
        {
            _body.Velocity = new Vector2(
                Mathf.MoveToward(_body.Velocity.X, 0, _climbVelocity),
                Mathf.MoveToward(_body.Velocity.Y, 0, _climbVelocity)
            );
        }
        else
        {
            _body.Velocity = direction * _climbVelocity;
        }

        _sprite.SpeedScale = Mathf.Abs(direction.Length());
    }
}
