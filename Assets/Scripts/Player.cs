using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AI
{
    public void Idle();
    public void Walk();
}

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
        base.Start();
        if(GridGenerator.getCell(MyCellPos).IsObstacle)
        {
            Debug.LogError("Enter a Valid Cell Pos for Player");
        }
        CurrentCell = GridGenerator.getCell(MyCellPos);
        transform.position = CurrentCell.transform.position;
        rayCastManager = GetComponent<RayCastManager>();
        if (rayCastManager == null)
            Debug.LogError("Raycast Manager Component is missing");
        rb = GetComponent<Rigidbody>();
        precision = 1 / PathPrecision;
        Obj_Animator = ObjGfx.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CurrentCell.IsObstacle = true;
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
        rb.velocity = Vector3.zero;
        if (rayCastManager.selected_cell == null)
            return;
        if (rayCastManager.selected_cell.IsObstacle)
            return;
        if (CurrentCell == null)
            return;
        if (Input.GetMouseButton(0))
        {
            if (MoveToPosition(rayCastManager.selected_cell) == null)
                return;
            current_target = rayCastManager.selected_cell;
            current_state = PlayerStates.Walk;
            t = 0;
        }

        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", false);
    }

    public void Walk()
    {
        Cell nextCell = MoveToPosition(current_target);
        if(nextCell == null)
        {
            current_state = PlayerStates.Idle;
            t = 0;
            return;
        }
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

        Vector3 lookPos = (nextCell.transform.position - transform.position).normalized;
            
        t += Time.deltaTime * PlayerSpeed;
        t = Mathf.Min(t, 1);

        if (lookPos == Vector3.zero)
            return;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        ObjGfx.transform.rotation = Quaternion.Slerp(ObjGfx.transform.rotation, lookRot, t);

        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", true);
    }
}
public enum PlayerStates
{
    Idle,
    Walk
}
