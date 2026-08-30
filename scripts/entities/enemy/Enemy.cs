using Godot;

public partial class Enemy : CharacterBody2D
{
    [Export]
    public Area2D HearingArea;

    [Export]
    public Area2D VisionArea;

    [Export]
    public TileDetector2D WaterDetector;

    public Player Target;

    [Signal]
    public delegate void OnAlertedEventHandler(Player player);

    [Signal]
    public delegate void OnTargetSpottedEventHandler(Player target);

    [Signal]
    public delegate void OnTargetLostEventHandler(Player target);

    public override void _Ready()
    {
        HearingArea.BodyEntered += OnHearingAreaBodyEntered;
        VisionArea.BodyEntered += OnVisionAreaBodyEntered;
        VisionArea.BodyExited += OnVisionAreaBodyExited;
    }

    public void OnHearingAreaBodyEntered(Node body)
    {
        if (body is Player)
            EmitSignal("OnAlerted", body);
    }

    public void OnVisionAreaBodyEntered(Node body)
    {
        if (Target == null && body is Player)
        {
            Target = body as Player;
            EmitSignal("OnTargetSpotted", body as Player);
        }
    }

    public void OnVisionAreaBodyExited(Node body)
    {
        if (body == Target)
        {
            Target = null;
            EmitSignal("OnTargetLost", body as Player);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
