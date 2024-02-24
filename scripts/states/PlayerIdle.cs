using Godot;

public partial class PlayerIdle : State
{
    [Export]
    private CharacterBody2D _body;

    [Export]
    private AnimatedSprite2D _sprite;

    [Export]
    private TileDetector2D _ladderDetector;

    [Export]
    private State _fallState;

    [Export]
    private State _runState;

    [Export]
    private State _jumpState;

    [Export]
    private State _climbState;

    [Export]
    private State _crouchState;

    public override void Enter()
    {
        _sprite.Play("Idle");
    }

    public override void UpdatePhysics(double delta)
    {
        switch (true)
        {
            case true when !_body.IsOnFloor():
                Transition(_fallState);
                break;

            case true
                when Input.IsActionPressed("MoveLeft")
                    || Input.IsActionPressed("MoveRight"):
                Transition(_runState);
                break;

            case true when Input.IsActionJustPressed("Jump"):
                Transition(_jumpState);
                break;

            case true
                when _ladderDetector.IsOverlapping
                    && Input.IsActionPressed("MoveUp"):
                Transition(_climbState);
                break;

            case true when Input.IsActionJustPressed("MoveDown"):
                Transition(_crouchState);
                break;

            // case true when Input.IsActionJustPressed("Attack"):
            //     Transition(_attackState);
            //     break;

            // case true when Input.IsActionJustPressed("Interact"):
            //     Transition(_interactState);
            //     break;}
        }
    }
}
