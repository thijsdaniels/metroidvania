using Godot;
using Zeldavania.Combat;

public partial class Player : CharacterBody2D
{
    [Export]
    private Damageable _damageable;

    [Export]
    private AudioStreamPlayer2D _deathSoundEffect;

    private Vector2 _spawnPosition;

    public override void _Ready()
    {
        _spawnPosition = GlobalPosition;

        if (_damageable != null)
        {
            _damageable.OnDepleted += HandleDefeat;
        }
    }

    public override void _ExitTree()
    {
        if (_damageable != null)
        {
            _damageable.OnDepleted -= HandleDefeat;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        /// @todo I should probably let the states determine whether the player
        /// can drop through the one-way-collision tiles or not.
        DropThroughFloor();

        MoveAndSlide();
    }

    protected void DropThroughFloor()
    {
        if (Input.IsActionJustPressed(Controller.Down))
        {
            SetCollisionMaskValue(2, false);

            /// @todo I don't like translating the player by 1 pixel like this,
            /// but it does mean the player instantly passes through the floor.
            /// Without it, you have to keep the down button pressed for a few
            /// frames to get the player to cross the one-way-collision margin.
            if (IsOnFloor())
                Position += new Vector2(0, 1);
        }

        if (Input.IsActionJustReleased(Controller.Down))
            SetCollisionMaskValue(2, true);
    }

    private void HandleDefeat()
    {
        _deathSoundEffect?.Play();
        GlobalPosition = _spawnPosition;
        Velocity = Vector2.Zero;
        _damageable?.Reset();
    }
}
