using Godot;

public partial class PlayerLanding : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AudioStreamPlayer2D _landEffect;

    [Export]
    private State _runningState;

    [Export]
    private State _crouchingState;

    [Export]
    private State _standingState;

    public override void Enter()
    {
        _landEffect.Play();

        switch (true)
        {
            case true when _body.Velocity.X != 0:
                Transition(_runningState);
                break;

            case true when Input.IsActionPressed("MoveDown"):
                Transition(_crouchingState);
                break;

            default:
                Transition(_standingState);
                break;
        }
    }
}
