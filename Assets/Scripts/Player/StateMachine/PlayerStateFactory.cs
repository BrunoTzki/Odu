using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerStates {
    Idle,
    Move,
    Grounded,
    Jump,
    Fall
}
public class PlayerStateFactory 
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext){
        _context = currentContext;
        _states[PlayerStates.Idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.Move] = new PlayerMoveState(_context, this);
        _states[PlayerStates.Grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.Jump] = new PlayerJumpState(_context, this);
        _states[PlayerStates.Fall] = new PlayerFallState(_context, this);
    }

    public PlayerBaseState Idle(){
        return _states[PlayerStates.Idle];
    }
    public PlayerBaseState Move(){
        return _states[PlayerStates.Move];
    }
    public PlayerBaseState Grounded(){
        return _states[PlayerStates.Grounded];
    }
    public PlayerBaseState Fall(){
        return _states[PlayerStates.Fall];
    }
    public PlayerBaseState Jump(){
        return _states[PlayerStates.Jump];
    }
}
