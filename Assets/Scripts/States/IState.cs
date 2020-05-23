// @author Tapio Mylläri

/// <summary>
/// Interface for a simple state machine behaviour. 
/// </summary>
public interface IState
{
    //Handles what happens when you enter a state.
    void Enter();

    //Handles what happens on every frame while on the state.
    void Execute();

    //Handles what happens when you exit a state.
    void Exit();
}