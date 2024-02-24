using Godot;

public partial class PlayerSwim : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private TileDetector2D _waterDetector;

    [Export]
    private float _sinkAcceleration = 250;

    [Export]
    private float _swimVelocity = 40;

    [Export]
    private float _leapVelocity = 90;

    [Export]
    private AudioStreamPlayer2D _splashEffect;

    [Export]
    private State _fallState;

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
        if (Input.IsActionPressed("MoveUp"))
        {
            _splashEffect.Play();

            _body.Velocity = new Vector2(
                _body.Velocity.X,
                Mathf.Min(_body.Velocity.X, -_leapVelocity)
            );
        }

        Transition(_fallState);
    }

    public override void UpdatePhysics(double delta)
    {
        Swim();
        Sink(delta);
    }

    private void Sink(double delta)
    {
        _body.Velocity += new Vector2(0, _sinkAcceleration * (float)delta);
    }

    private void Swim()
    {
        Vector2 direction = Input.GetVector(
            "MoveLeft",
            "MoveRight",
            "MoveUp",
            "MoveDown"
        );

        if (direction == Vector2.Zero)
        {
            _sprite.Play("Fall");

            _body.Velocity = new Vector2(
                Mathf.MoveToward(_body.Velocity.X, 0, _swimVelocity),
                Mathf.MoveToward(_body.Velocity.Y, 0, _swimVelocity)
            );
        }
        else
        {
            _sprite.Play("Swim");

            _body.Velocity = direction * _swimVelocity;

            if (direction.X > 0)
                _sprite.FlipH = true;
            else if (direction.X < 0)
                _sprite.FlipH = false;
        }
    }
}
