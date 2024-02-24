using Godot;

public partial class PlayerJumping : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AudioStreamPlayer2D _jumpEffect;

    [Export]
    private float _velocity = 100;

    [Export]
    private State _fallingState;

    public override void Enter()
    {
        Jump();

        /// @todo Stay in the Jump state until the player is at the apex of the
        /// jump.
        Transition(_fallingState);
    }

    private void Jump()
    {
        _body.Velocity = new Vector2(_body.Velocity.X, -_velocity);

        _jumpEffect.Play();
    }
}
