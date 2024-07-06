using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : NavAgent,AI
{
    public Vector2Int MyCellPos;

    public float EnemySpeed = 10;
    //Higher the speed lower the PathPrecision should be
    public float PathPrecision = 100;
    
    public EnemyStates current_state = EnemyStates.Idle;
    public GameObject ObjGfx;

    Animator Obj_Animator;
    Player player;
    Cell current_target;
    Rigidbody rb;
    float precision;

    //t is used for turning animations in slerp
    float t;

    public override void Start()
    {
        base.Start();
        if(GridGenerator.getCell(MyCellPos).IsObstacle)
        {
            Debug.LogError("Enter a Valid Cell Pos for Enemy");
            Destroy(gameObject);
        }
        CurrentCell = GridGenerator.getCell(MyCellPos);
        transform.position = CurrentCell.transform.position;
        rb = GetComponent<Rigidbody>();
        precision = 1 / PathPrecision;

        //Not a very efficient method
        //Idelly I would have used a singleton, or a GameManager but for this demo this is fine
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Obj_Animator = ObjGfx.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //by default set whichever current cell enemy is in to true
        CurrentCell.IsObstacle = true;
        
        //Enemy State Machine
        switch (current_state)
        {
            case EnemyStates.Idle:
                Idle();
                break;
            case EnemyStates.Walk:
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
        if (CurrentCell == null)
            return;
        if (player == null)
            return;

        if (MoveToPosition(player.CurrentCell) == null)
            return;

        //Code reaches here if a valid path is available

        //If Already in target cell just return
        if (player.CurrentCell == current_target)
            return;

        //Set to walk state
        current_target = player.CurrentCell;
        current_state = EnemyStates.Walk;
        t = 0;
    }

    public void Walk()
    {
        //In walk State always set idle animation
        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", true);

        //Always set enemies current target to player cell
        current_target = player.CurrentCell;
        Cell nextCell = MoveToPosition(current_target);
        if(nextCell == player.CurrentCell || nextCell == null)
        {
            current_state = EnemyStates.Idle;
            t = 0;
            return;
        }
        //If within satisfiable range of target switch to idle or keep on walking
        if ((rb.position - nextCell.transform.position).magnitude > precision)
        {
            rb.velocity = (nextCell.transform.position - rb.position).normalized * EnemySpeed * Time.fixedDeltaTime;
        }
        else
        {
            CurrentCell.IsObstacle = false;
            CurrentCell = nextCell;
            t = 0;
        }

        //Generate look Rotation and face the model in that direction
        Vector3 lookPos = (nextCell.transform.position - transform.position).normalized;
            
        t += Time.deltaTime * EnemySpeed;
        t = Mathf.Min(t, 1);

        if (lookPos == Vector3.zero)
            return;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        ObjGfx.transform.rotation = Quaternion.Slerp(ObjGfx.transform.rotation, lookRot, t);


    }
}
public enum EnemyStates
{
    Idle,
    Walk
}
