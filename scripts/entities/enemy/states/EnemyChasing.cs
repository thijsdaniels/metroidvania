using Godot;

public partial class EnemyChasing : State
{
    [Export]
    public Enemy _enemy;

    [Export]
    public AnimatedSprite2D _sprite;

    [Export]
    public string _animation = "Running";

    [Export]
    public string _idleAnimation = "Standing";

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

    [Export]
    public float _reachThreshold = 13;

    [Export]
    public float _verticalReachThreshold = 8;

    [Export]
    public float _targetDeadzone = 2;

    public override void Enter()
    {
        _sprite.SpeedScale = 1.0f;
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

        bool inHorizontalRange = Mathf.Abs(distance.X) <= _reachThreshold;
        bool inVerticalRange = Mathf.Abs(distance.Y) <= _verticalReachThreshold;

        if (_onTargetReached != null && inHorizontalRange && inVerticalRange)
        {
            Transition(_onTargetReached);
            return;
        }

        if (_targetDeadzone > 0 && Mathf.Abs(distance.X) <= _targetDeadzone)
        {
            _enemy.Decelerate(new Vector2(_acceleration * (float)delta, 0));

            if (Mathf.Abs(_enemy.Velocity.X) < 5)
            {
                if (_sprite.Animation != _idleAnimation)
                {
                    _sprite.SpeedScale = 1.0f;
                    _sprite.Play(_idleAnimation);
                }
            }
            return;
        }

        if (_sprite.Animation != _animation)
        {
            _sprite.Play(_animation);
        }

        Vector2 direction = new Vector2(distance.X, 0).Normalized();

        _enemy.Accelerate(
            direction: direction.X,
            acceleration: _acceleration * (float)delta,
            limit: _limit
        );

        float speedRatio = _limit > 0 ? Mathf.Clamp(Mathf.Abs(_enemy.Velocity.X) / _limit, 0.2f, 1.0f) : 1.0f;
        _sprite.SpeedScale = speedRatio;

        if (direction.X != 0)
        {
            _sprite.FlipH = direction.X < 0;
        }
    }
}
