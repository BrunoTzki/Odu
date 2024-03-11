using UnityEngine;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set  { _isRootState = value; }}
    protected PlayerStateMachine Ctx { get { return _ctx; }}
    protected PlayerStateFactory Factory { get { return _factory; }}

    

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory){
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract bool CheckSwitchStates();

    public abstract void InitializeSubState();

    public void UpdateStates(){
        UpdateState();
        _currentSubState?.UpdateStates();
    }

    protected void SwitchState(PlayerBaseState newState){
        //current state exits state
        ExitState();

        //new state enters state
        newState.EnterState();

        if(_isRootState){
            //switch current state of context
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null){
            //set the current super states sub state to the new state
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState){
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState){
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

        _currentSubState.EnterState();
    }

    protected void ExitSubState(){
        if(_currentSubState != null)
            _currentSubState.ExitState();
    }

    public string GetActiveStates(){
        return this.ToString() + "\n" + _currentSubState.ToString();
    }
}
