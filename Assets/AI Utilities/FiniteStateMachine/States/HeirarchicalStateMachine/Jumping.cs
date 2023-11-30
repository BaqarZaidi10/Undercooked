using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : BaseState
{
    private bool grounded;
    private LayerMask groundLayer = 1 << 6; 
    protected MovementSM MSM;

    public Jumping(MovementSM stateMachine) : base("Jumping", stateMachine)
    {
        MSM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        MSM.spriteRenderer.color = Color.green;

        Vector2 vloct = MSM.rigidbody.velocity;
        vloct.y += MSM.jumpForce;
        MSM.rigidbody.velocity = vloct;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(grounded)
        {
            MSM.ChangeState(MSM.idleState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        grounded = MSM.rigidbody.velocity.y < Mathf.Epsilon && MSM.rigidbody.IsTouchingLayers(groundLayer);
    }
}
