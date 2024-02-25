using Godot;

public partial class PlayerRolling : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [ExportGroup("Collider")]
    [Export]
    private CollisionShape2D _collider;

    [Export]
    private int _colliderRadius = 2;

    [ExportGroup("Movement")]
    [Export]
    private float _acceleration = 700;

    [Export]
    private float _deceleration = 200;

    [Export]
    private float _maximumVelocity = 90;

    [ExportGroup("Crouching")]
    [Export]
    private State _crouchingState;

    [ExportGroup("Falling")]
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
            case true when Input.IsActionJustPressed(Controller.Up):
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
