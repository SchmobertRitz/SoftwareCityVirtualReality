using UnityEngine;
using System.Collections;
using System;

public class PlayingStateMachine : MonoBehaviour {

    private State currentState;

	// Use this for initialization
	void Start () {
        currentState = new InitState();
        currentState.OnEnterState();
	}

    public void PostStateEvent(StateEvent stateEvent)
    {
        State nextState = currentState.GetNextState(stateEvent);
        if (!nextState.GetType().Equals(currentState.GetType()))
        {
            currentState.OnExitState();
            currentState = nextState;
            currentState.OnEnterState();
            Debug.Log("Current playing state is " + currentState.GetType());
        }
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public Boolean IsCurrentState(State state)
    {
        return currentState.GetType().Equals(state.GetType());
    }
}

public enum StateEvent
{
    Restart,
    StartCalibrating,
    FinishCalibrating,
    StartPlaying,
    StopPlaying,
    StartChange,
    ChooseBar,
    ChooseJuRob
}

public interface State
{
    State GetNextState(StateEvent stateEvent);
    void OnEnterState();
    void OnExitState();
}

public class InitState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.StartChange)
        {
            return new ChangeState();
        }
        return this;
    }

    public void OnEnterState()
    {
        // Activate Interaction Calibration Switch
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.ReadyForCalibration));
    }

    public void OnExitState()
    {
    }
}

public class CalibratingState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        } else if (stateEvent == StateEvent.FinishCalibrating)
        {
            return new WaitingForPlayingState();
        }
        return this;
    }

    public void OnEnterState()
    {
        // Activate Interaction Calibration
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Calibration));
    }

    public void OnExitState()
    {
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Nothing));
    }
}

public class WaitingForPlayingState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.StartPlaying)
        {
            return new PlayingState();
        }
        return this;
    }

    public void OnEnterState()
    {
    }

    public void OnExitState()
    {
    }
}

public class PlayingState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.StopPlaying)
        {
            return new WaitingForPlayingState();
        }
        return this;
    }

    public void OnEnterState()
    {
        EventBus.Post(new StartPlayingEvent());
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
    }

    public void OnExitState()
    {
        EventBus.Post(new StopPlayingEvent());
        EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Nothing));
    }
}

public class RestartState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        return this;
    }

    public void OnEnterState()
    {
        GameObject.FindObjectOfType<LifeCycle>().Restart();
    }

    public void OnExitState()
    {
    }
}

public class ChangeState : State
{
    public State GetNextState(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Restart)
        {
            return new RestartState();
        }
        else if (stateEvent == StateEvent.ChooseBar)
        {
            return new WaitingForPlayingState();
        }
        else if (stateEvent == StateEvent.ChooseJuRob)
        {
            return new CalibratingState();
        }
        return this;
    }

    public void OnEnterState()
    {
        GameObject.FindObjectOfType<LifeCycle>().LoadChangeScene();
    }

    public void OnExitState()
    {
    }
}