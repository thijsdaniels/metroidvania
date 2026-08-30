using Godot;

public partial class EnemyFalling : State
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
    public float _gravity = 400;

    [Export]
    public float _terminalVelocity = 150;

    [Export]
    public State _onLand;

    [Export]
    public State _onPlunge;

    public override void Enter()
    {
        if (_sprite != null && _animation != null)
            _sprite.Play(_animation);

        if (_onPlunge != null)
            _enemy.WaterDetector.OnTileEntered += OnWaterEntered;
    }

    public override void Exit()
    {
        if (_onPlunge != null)
            _enemy.WaterDetector.OnTileEntered -= OnWaterEntered;
    }

    public void OnWaterEntered()
    {
        Transition(_onPlunge);
    }

    public override void UpdatePhysics(double delta)
    {
        if (_enemy.IsOnFloor())
        {
            Transition(_onLand);
            return;
        }

        _enemy.Decelerate(new Vector2(_deceleration * (float)delta, 0));

        if (_enemy.Velocity.Y < _terminalVelocity)
            _enemy.Velocity += new Vector2(0, _gravity * (float)delta);
    }
}
