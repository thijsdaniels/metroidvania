using Godot;

public partial class EnemySleeping : State
{
    [Export]
    public Enemy _enemy;

    [Export]
    public AnimatedSprite2D _sprite;

    [Export]
    public string _animation = "Sleeping";

    [Export]
    public State _onWake;

    public override void Enter()
    {
        _enemy.Target = null;

        if (_animation != null)
            _sprite?.Play(_animation);

        _enemy.OnAlerted += OnAlerted;

        _enemy.VisionArea.Monitoring = false;
    }

    public override void Exit()
    {
        _enemy.OnAlerted -= OnAlerted;

        _enemy.VisionArea.Monitoring = true;
    }

    public void OnAlerted(Node body)
    {
        Transition(_onWake);
    }
}
