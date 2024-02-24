using Godot;

public abstract partial class State : Node
{
    [Signal]
    public delegate void OnTransitionEventHandler(string state);

    public virtual void Enter() { }

    public virtual void Exit() { }

    public sealed override void _Process(double delta) { }

    public sealed override void _PhysicsProcess(double delta) { }

    public virtual void UpdateGraphics(double delta) { }

    public virtual void UpdatePhysics(double delta) { }

    protected void Transition(State state)
    {
        EmitSignal("OnTransition", state.Name);
    }
}
