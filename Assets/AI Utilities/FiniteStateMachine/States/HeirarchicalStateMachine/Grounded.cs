using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : BaseState
{
    protected MovementSM MSM;

    public Grounded(string name, MovementSM stateMachine) : base(name, stateMachine)
    {
        MSM = stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(((MovementSM)stateMachine).jumpingState);
        }
    }
}
