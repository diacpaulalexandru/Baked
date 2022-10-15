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

        if (StateMachine == null)
            return;

        if (StateMachine.GameState == this)
            return;
        GoToState(this);
    }
}