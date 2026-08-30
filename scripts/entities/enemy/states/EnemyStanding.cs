using Godot;

public partial class EnemyStanding : State
{
    [Export]
    public Enemy _enemy;

    [Export]
    public AnimatedSprite2D _sprite;

    [Export]
    public string _animation = "Standing";

    [Export]
    public float _deceleration = 200;

    [Export]
    public State _onTargetSpotted;

    [Export]
    public Timer _sleepTimer;

    [Export]
    public State _onSleep;

    public override void Enter()
    {
        if (_sprite != null && _animation != null)
            _sprite.Play(_animation);

        if (_sleepTimer != null)
        {
            _sleepTimer.Start();
            _sleepTimer.Timeout += OnSleepTimerTimeout;
        }
    }

    public override void Exit()
    {
        if (_sleepTimer != null)
            _sleepTimer.Timeout -= OnSleepTimerTimeout;
    }

    public void OnSleepTimerTimeout()
    {
        Transition(_onSleep);
    }

    public override void UpdatePhysics(double delta)
    {
        if (_onTargetSpotted != null && _enemy.Target != null)
        {
            Transition(_onTargetSpotted);
            return;
        }

        _enemy.Decelerate(new Vector2(_deceleration * (float)delta, 0));

        if (!_enemy.IsOnFloor() && _enemy.Velocity.Y < 150)
            _enemy.Velocity += new Vector2(0, 400 * (float)delta);
    }
}
