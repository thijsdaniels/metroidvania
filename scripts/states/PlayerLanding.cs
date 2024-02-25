using Godot;

public partial class PlayerLanding : State
{
    [Export]
    private CharacterBody2D _body;

    [ExportGroup("Landing")]
    [Export]
    private AudioStreamPlayer2D _soundEffect;

    [ExportGroup("Standing")]
    [Export]
    private State _standingState;

    [ExportGroup("Running")]
    [Export]
    private State _runningState;

    [ExportGroup("Crouching")]
    [Export]
    private State _crouchingState;

    public override void Enter()
    {
        _soundEffect.Play();

        switch (true)
        {
            case true when _body.Velocity.X != 0:
                Transition(_runningState);
                break;

            case true when Input.IsActionPressed(Controller.Down):
                Transition(_crouchingState);
                break;

            default:
                Transition(_standingState);
                break;
        }
    }
}
