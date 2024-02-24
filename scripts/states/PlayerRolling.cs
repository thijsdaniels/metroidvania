using Godot;

public partial class PlayerRolling : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private CollisionShape2D _collider;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private float _maximumVelocity = 90;

    [Export]
    private float _traction = 700;

    [Export]
    private float _friction = 200;

    [Export]
    private int _colliderRadius = 2;

    [Export]
    private State _crouchingState;

    [Export]
    private State _fallingState;

    private Vector2 _oldColliderPosition;

    private Shape2D _oldColliderShape;

    public override void Enter()
    {
        _sprite.Play("Roll");

        Morph();
    }

    public override void Exit()
    {
        _sprite.SpeedScale = 1;

        Unmorph();
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true when Input.IsActionJustPressed("MoveUp"):
                Transition(_crouchingState);
                break;

            // @todo Transition to `PlayerMorphFall` state instead.
            case true when !_body.IsOnFloor():
                Transition(_fallingState);
                break;

            default:
                Roll(delta);
                break;
        }
    }

    private void Morph()
    {
        _oldColliderPosition = _collider.Position;
        _oldColliderShape = _collider.Shape;

        _collider.Position = new Vector2(0, -_colliderRadius);
        _collider.Shape = new CircleShape2D();
        ((CircleShape2D)_collider.Shape).Radius = _colliderRadius;
    }

    private void Unmorph()
    {
        _collider.Position = _oldColliderPosition;
        _collider.Shape = _oldColliderShape;
    }

    private void Roll(double delta)
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
