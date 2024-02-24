using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class FiniteStateMachine : Node
{
    private readonly Dictionary<string, State> _states = new();

    [Export]
    private State _state;

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is State state)
            {
                RegisterState(state);
            }
        }

        string initialState = _state.Name ?? _states.First().Value.Name;

        OnTransition(initialState);
    }

    public override void _Process(double delta)
    {
        _state.UpdateGraphics(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _state.UpdatePhysics(delta);
    }

    private void RegisterState(State state)
    {
        _states.Add(state.Name, state);
        state.OnTransition += OnTransition;
    }

    public void OnTransition(string state)
    {
        if (!_states.ContainsKey(state))
        {
            throw new KeyNotFoundException(
                $"State {state} not found in state machine {this.Name}."
            );
        }

        _state?.Exit();
        _state = _states[state];
        _state.Enter();
    }
}
