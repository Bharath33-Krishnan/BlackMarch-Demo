using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A general interface for all AI charecters
//For more advance charecters we can add more States
public interface AI
{
    public void Idle();
    public void Walk();
}

//NOTE: Enemy Script and Player scripts are very similar and inherits
//from NavAgent class and AI interface 

[RequireComponent(typeof(Rigidbody))]
public class Player : NavAgent,AI
{
    public Vector2Int MyCellPos;
    public float PlayerSpeed = 20;
    public float PathPrecision = 100;
    public PlayerStates current_state = PlayerStates.Idle;
    public GameObject ObjGfx;

    Animator Obj_Animator;
    Cell current_target;
    RayCastManager rayCastManager;
    Rigidbody rb;
    float precision;
    float t;

    public override void Start()
    {
        //NavAgent initialisation
        base.Start();
        if(GridGenerator.getCell(MyCellPos).IsObstacle)
        {
            Debug.LogError("Enter a Valid Cell Pos for Player");
        }
        //set initial pos to current cell pos
        CurrentCell = GridGenerator.getCell(MyCellPos);
        transform.position = CurrentCell.transform.position;

        //Access the raycast manager
        rayCastManager = GetComponent<RayCastManager>();
        if (rayCastManager == null)
            Debug.LogError("Raycast Manager Component is missing");
        rb = GetComponent<Rigidbody>();
        precision = 1 / PathPrecision;
        Obj_Animator = ObjGfx.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //by default set whichever current cell enemy is in to true
        CurrentCell.IsObstacle = true;

        //Player State Machine
        switch (current_state)
        {
            case PlayerStates.Idle:
                Idle();
                break;
            case PlayerStates.Walk:
                Walk();
                break;
        }
    }

    public void Idle()
    {
        //In Idle State always set idle animation
        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", false);

        rb.velocity = Vector3.zero;
        if (rayCastManager.selected_cell == null)
            return;
        if (rayCastManager.selected_cell.IsObstacle)
            return;
        if (CurrentCell == null)
            return;

        //rayCast Manager has a selected cell
        if (Input.GetMouseButton(0))
        {
            //Mouse clicked
            if (MoveToPosition(rayCastManager.selected_cell) == null)
                return;

            //Path can be generated
            current_target = rayCastManager.selected_cell;
            current_state = PlayerStates.Walk;
            t = 0;
        }

    }

    public void Walk()
    {
        //In walk State always set idle animation
        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", true);

        //Move towards current Target
        Cell nextCell = MoveToPosition(current_target);
        if(nextCell == null)
        {
            //If next cell is none
            //Path got completed or no more path exists
            current_state = PlayerStates.Idle;
            t = 0;
            return;
        }

        //If within satisfiable range of target switch to idle or keep on walking
        if ((rb.position - nextCell.transform.position).magnitude> precision)
        {
            rb.velocity = (nextCell.transform.position - rb.position).normalized * PlayerSpeed*Time.fixedDeltaTime;
        }
        else
        {
            CurrentCell.IsObstacle = false;
            CurrentCell = nextCell;
            t = 0;
        }

        //Generate look Rotation and face the model in that direction
        Vector3 lookPos = (nextCell.transform.position - transform.position).normalized;
            
        t += Time.deltaTime * PlayerSpeed;
        t = Mathf.Min(t, 1);

        if (lookPos == Vector3.zero)
            return;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        ObjGfx.transform.rotation = Quaternion.Slerp(ObjGfx.transform.rotation, lookRot, t);

    }
}
public enum PlayerStates
{
    Idle,
    Walk
}
