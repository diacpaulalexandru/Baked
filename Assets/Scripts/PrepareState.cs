using System.Collections;
using System.Collections.Generic;
using SimpleFSM;
using UnityEngine;

public class PrepareState : BaseState
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fund") == false)
            return;
Debug.Log(collision.gameObject);
        if (StateMachine == null)
            return;

        if (StateMachine.GameState == this)
            return;
        
        if (StateMachine.GameState is CutState)
            return;
        
        Debug.Log("Change to prepare state!");
        GoToState(this);
    }

    public void Cut()
    {
        GoToState<CutState>();
    }
}