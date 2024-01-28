using UnityEngine;

/// <summary>
///     Creates a Finite State Machine (FSM) that controls the behaviour of an AI agent.
///     In case of multiple AI agents, each agent is controlled by a single StateMachine
/// </summary>
public abstract class StateMachine : MonoBehaviour
{   
    [HideInInspector] public State currentState;
    [HideInInspector] public State previousState = null;                    // keeping track of the previous state

    private void Start()
    {
        currentState = GetInitialState();                                   // set initial state
        if (currentState != null) currentState.Enter();
        else Debug.LogWarning("Initial State is Null");
    }

    private void Update()
    {
        currentState.Update();                                              // calls Update for the current state the agent is in
    }

    private void LateUpdate()
    {
        currentState.LateUpdate();                                          // calls LateUpdate for the current state the agent is in
    }

    /// <summary>
    ///     When an action (or State) has ended, the corresponding state should notify the finite state machine
    ///     by calling this function. The FSM then decides which action (or State) should the agent perform next.
    ///     The way the FSM determines the next state is implemented using the function DetermineNextState.
    ///     Note: the words action and state are used interchangeably; they mean the same thing.
    /// </summary>
    public void NotifyActionCompleted(bool isPreviousStateRepeatable = true)
    {
        currentState.Exit();
        
        previousState = currentState;

        currentState = DetermineNextState(isPreviousStateRepeatable);
        currentState.Enter();
    }

    /// <summary>
    ///     Implement this to determine how this FSM decides on which action to perform next for the agent.
    /// </summary>
    /// <returns>Next state that the agent should perform\be in.</returns>
    public abstract State DetermineNextState(bool isPreviousStateRepeatable = true);

    /// <summary>
    ///     Implement this to specify which initial state should the agent perform\be in.
    /// </summary>
    /// <returns>Initial state the agent should perform\be in.</returns>
    protected abstract State GetInitialState();
}
