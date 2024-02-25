using Godot;

public partial class PlayerClimbing : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Climbing")]
    [Export]
    private TileDetector2D _ladderDetector;

    [Export]
    private Vector2 _climbVelocity = new(30, 40);

    [Export]
    private Vector2 _climbAcceleration = new(300, 400);

    [Export]
    private Vector2 _climbDeceleration = new(600, 800);

    [Export]
    private float _leapVelocity = 40;

    [ExportGroup("Transitions")]
    [Export]
    private State _fallingState;

    [Export]
    private State _jumpingState;

    [Export]
    private State _standingState;

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
        if (Input.IsActionPressed(Controller.Up))
        {
            _body.Velocity = new Vector2(
                _body.Velocity.X,
                Mathf.Min(_body.Velocity.X, -_leapVelocity)
            );
        }

        Transition(_fallingState);
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true
                when _body.IsOnFloor()
                    && Input.IsActionPressed(Controller.Down):
                Transition(_standingState);
                return;

            case true when Input.IsActionJustPressed(Controller.A):
                Transition(_jumpingState);
                return;

            case true when Input.IsActionJustPressed(Controller.B):
                Transition(_fallingState);
                return;
        }

        Climb(delta);
    }

    private void Climb(double delta)
    {
        Vector2 direction = Controller.GetDirection();

        _body.Accelerate(
            delta: delta,
            direction: direction,
            acceleration: _climbAcceleration,
            deceleration: _climbDeceleration,
            limit: _climbVelocity
        );

        _sprite.SynchronizeAnimation(direction);
    }
}
