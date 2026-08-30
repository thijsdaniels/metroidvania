using Godot;

public partial class EnemyRunning : State
{
    [Export]
    public Enemy _enemy;

    [Export]
    public AnimatedSprite2D _sprite;

    [Export]
    public string _animation = "Running";

    [Export]
    public float _acceleration = 200;

    [Export]
    public float _limit = 50;

    [Export]
    public State _onFall;

    [Export]
    public State _onTargetLost;

    [Export]
    public State _onTargetReached;

    public override void Enter()
    {
        _sprite.Play(_animation);
    }

    public override void UpdatePhysics(double delta)
    {
        if (!_enemy.IsOnFloor())
        {
            Transition(_onFall);
            return;
        }

        if (_enemy.Target == null)
        {
            Transition(_onTargetLost);
            return;
        }

        Vector2 distance =
            _enemy.Target.GlobalPosition - _sprite.GlobalPosition;

        if (_onTargetReached != null && distance.Length() < 10)
        {
            Transition(_onTargetReached);
            return;
        }

        Vector2 direction = new Vector2(distance.X, 0).Normalized();

        _enemy.Accelerate(
            direction: direction.X,
            acceleration: _acceleration * (float)delta,
            limit: _limit
        );

        _sprite.SynchronizeAnimation(direction);
    }
}
