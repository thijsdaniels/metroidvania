using Godot;

public partial class PlayerLand : State
{
    [Export]
    private AudioStreamPlayer2D _landEffect;

    [Export]
    private State _runState;

    [Export]
    private State _crouchState;

    [Export]
    private State _idleState;

    public override void Enter()
    {
        _landEffect.Play();

        switch (true)
        {
            case true when Input.GetAxis("MoveLeft", "MoveRight") != 0:
                Transition(_runState);
                break;

            case true when Input.IsActionPressed("MoveDown"):
                Transition(_crouchState);
                break;

            default:
                Transition(_idleState);
                break;
        }
    }
}
