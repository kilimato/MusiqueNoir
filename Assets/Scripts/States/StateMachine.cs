// @author Tapio Mylläri

/// <summary>
/// Simple state machine that can update and change states. Makes sure that
/// only one state can be active.
/// </summary>
public class StateMachine
{
    IState currentState;

    //Changes state to some other state. Exits the current state if it's not null.
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //Executes the current state for every frame.
    public void Update()
    {
        currentState?.Execute();
    }

    //Used for debugging enemies states.
    public string GetCurrentState()
    {
        return currentState.GetType().Name;
    }

}
