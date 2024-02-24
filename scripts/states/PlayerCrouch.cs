using Godot;

public partial class PlayerCrouch : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private CollisionShape2D _collider;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private Vector2 _colliderSize = new(4, 8);

    [Export]
    private float _velocity = 20;

    [Export]
    private State _fallState;

    [Export]
    private State _jumpState;

    [Export]
    private State _idleState;

    [Export]
    private State _rollState;

    private Vector2 _oldColliderPosition;

    private Shape2D _oldColliderShape;

    public override void Enter()
    {
        _sprite.Play("Crouch");

        Crouch();
    }

    public override void Exit()
    {
        Stand();
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true when !_body.IsOnFloor():
                Transition(_fallState);
                break;

            case true when Input.IsActionJustPressed("MoveUp"):
                Transition(_idleState);
                break;

            case true when Input.IsActionJustPressed("Jump"):
                Transition(_jumpState);
                break;

            case true when Input.IsActionJustPressed("MoveDown"):
                Transition(_rollState);
                break;

            default:
                CrouchWalk();
                break;
        }
    }

    private void Crouch()
    {
        _oldColliderPosition = _collider.Position;
        _oldColliderShape = _collider.Shape;

        _collider.Position = new Vector2(0, -_colliderSize.Y / 2);
        _collider.Shape = new RectangleShape2D();
        ((RectangleShape2D)_collider.Shape).Size = _colliderSize;
    }

    private void Stand()
    {
        _collider.Position = _oldColliderPosition;
        _collider.Shape = _oldColliderShape;
    }

    private void CrouchWalk()
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
