using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Grounded
{
    private float horizontalInput;

    public Moving(MovementSM stateMachine) : base("Moving", stateMachine){}

    public override void Enter()
    {
        base.Enter();
        horizontalInput = 0;
        MSM.spriteRenderer.color = Color.red;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) < Mathf.Epsilon)
        {
            stateMachine.ChangeState(((MovementSM)stateMachine).idleState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Vector2 vloct = MSM.rigidbody.velocity;
        vloct.x = horizontalInput * MSM.speed;
        MSM.rigidbody.velocity = vloct;
    }
}
