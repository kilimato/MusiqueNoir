// @author Tapio Mylläri
public class StateMachine
{
    IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }

    public string GetCurrentState()
    {
        return currentState.GetType().Name;
    }

}
