using Godot;

public partial class PlayerSwimming : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Swimming")]
    [Export]
    private TileDetector2D _waterDetector;

    [Export]
    private float _gravity = 150;

    [Export]
    private float _terminalVelocity = 5;

    [Export]
    private float _maximumVelocity = 40;

    [Export]
    private float _acceleration = 400;

    [Export]
    private float _deceleration = 600;

    [Export]
    private float _leapVelocity = 110;

    [Export]
    private AudioStreamPlayer2D _splashEffect;

    [ExportGroup("Falling")]
    [Export]
    private State _fallingState;

    public override void Enter()
    {
        _splashEffect.Play();
        _sprite.Play("Fall");
        _waterDetector.OnTileExited += OnWaterExited;
    }

    public override void Exit()
    {
        _waterDetector.OnTileExited -= OnWaterExited;
    }

    private void OnWaterExited()
    {
        if (Input.IsActionPressed(Controller.Up))
        {
            _splashEffect.Play();

            _body.Velocity = new Vector2(
                _body.Velocity.X,
                Mathf.Min(_body.Velocity.X, -_leapVelocity)
            );
        }

        Transition(_fallingState);
    }

    public override void UpdatePhysics(double delta)
    {
        Swim(delta);
        Sink(delta);
    }

    private void Swim(double delta)
    {
        Vector2 direction = Controller.GetDirection();

        _body.Accelerate(
            delta: delta,
            direction: direction,
            acceleration: _acceleration,
            deceleration: _deceleration,
            limit: _maximumVelocity
        );

        if (direction == Vector2.Zero)
            _sprite.Play("Fall");
        else
            _sprite.Play("Swim");

        _sprite.SynchronizeAnimation(direction);
    }

    private void Sink(double delta)
    {
        if (_body.Velocity.Y < _terminalVelocity)
            _body.Velocity += new Vector2(0, _gravity * (float)delta);
    }
}
