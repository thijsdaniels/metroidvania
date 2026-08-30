using Godot;

public partial class EnemyDying : State
{
    [Export]
    public Enemy _enemy;

    [Export]
    public AnimatedSprite2D _sprite;

    [Export]
    public string _animation = "Dying";

    public override void Enter()
    {
        _enemy.Velocity = Vector2.Zero;

        _sprite.AnimationFinished += OnAnimationFinished;

        if (_sprite != null && _animation != null)
            _sprite.Play(_animation);
    }

    public void OnAnimationFinished()
    {
        _sprite.AnimationFinished -= OnAnimationFinished;
        _enemy.QueueFree();
    }
}
