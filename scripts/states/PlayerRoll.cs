using Godot;

public partial class PlayerRoll : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private CollisionShape2D _collider;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private float _velocity = 90;

    [Export]
    private int _colliderRadius = 2;

    [Export]
    private State _crouchState;

    [Export]
    private State _fallState;

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
                Transition(_crouchState);
                break;

            // @todo Transition to `PlayerMorphFall` state instead.
            case true when !_body.IsOnFloor():
                Transition(_fallState);
                break;

            default:
                Roll();
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

    private void Roll()
    {
        float x = Input.GetAxis("MoveLeft", "MoveRight");

        if (x == 0)
        {
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
