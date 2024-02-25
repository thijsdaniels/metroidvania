using Godot;

public partial class PlayerCrouching : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Collision")]
    [Export]
    private CollisionShape2D _collider;

    [Export]
    private Vector2 _colliderSize = new(4, 8);

    [ExportGroup("Movement")]
    [Export]
    private float _acceleration = 300;

    [Export]
    private float _deceleration = 200;

    [Export]
    private float _maximumVelocity = 30;

    [ExportGroup("Falling")]
    [Export]
    private State _fallingState;

    [ExportGroup("Jumping")]
    [Export]
    private State _jumpingState;

    [ExportGroup("Standing")]
    [Export]
    private State _standingState;

    [ExportGroup("Rolling")]
    [Export]
    private State _rollingState;

    private Vector2 _oldColliderPosition;
    private Shape2D _oldColliderShape;

    public override void Enter()
    {
        Crouch();
    }

    public override void Exit()
    {
        Stand();
    }

    private void Crouch()
    {
        _sprite.Play("Crouch");

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

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true when !_body.IsOnFloor():
                Transition(_fallingState);
                break;

            case true when Input.IsActionJustPressed(Controller.Up):
                Transition(_standingState);
                break;

            case true when Input.IsActionJustPressed(Controller.A):
                Transition(_jumpingState);
                break;

            case true when Input.IsActionJustPressed(Controller.Down):
                Transition(_rollingState);
                break;

            default:
                CrouchWalk(delta);
                break;
        }
    }

    private void CrouchWalk(double delta)
    {
        float direction = Controller.GetHorizontalDirection();

        _body.Accelerate(
            delta: delta,
            direction: direction,
            acceleration: _acceleration,
            deceleration: _deceleration,
            limit: _maximumVelocity
        );

        _sprite.SynchronizeAnimation(direction);
    }
}
