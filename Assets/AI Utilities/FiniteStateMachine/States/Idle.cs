using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grounded
{
    private float horizontalInput;

    public Idle(MovementSM stateMachine) : base("Idle", stateMachine){}

    public override void Enter()
    {
        base.Enter();
        horizontalInput = 0;
        MSM.spriteRenderer.color = Color.blue;
    }

    public override void UpdateLogic() 
    { 
        base.UpdateLogic();
        horizontalInput = Input.GetAxis("Horizontal");

        if(Mathf.Abs(horizontalInput) > Mathf.Epsilon)
        {
            stateMachine.ChangeState(((MovementSM) stateMachine).movingState);
        }
    }
}
