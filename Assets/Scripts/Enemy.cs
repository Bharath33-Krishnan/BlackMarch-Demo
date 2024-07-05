using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : NavAgent,AI
{
    public Vector2Int MyCellPos;
    public float EnemySpeed = 10;
    public float PathPrecision = 100;
    public EnemyStates current_state = EnemyStates.Idle;
    public GameObject ObjGfx;

    Animator Obj_Animator;
    Player player;
    Cell current_target;
    Rigidbody rb;
    float precision;
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
        CurrentCell.IsObstacle = true;
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
        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", false);
        rb.velocity = Vector3.zero;
        if (CurrentCell == null)
            return;
        if (player == null)
            return;

        if (MoveToPosition(player.CurrentCell) == null)
            return;


        if (player.CurrentCell == current_target)
            return;
        current_target = player.CurrentCell;
        current_state = EnemyStates.Walk;
        t = 0;
    }

    public void Walk()
    {
        if(Obj_Animator != null)
            Obj_Animator.SetBool("IsRunning", true);

        current_target = player.CurrentCell;
        Cell nextCell = MoveToPosition(current_target);
        if(nextCell == player.CurrentCell || nextCell == null)
        {
            current_state = EnemyStates.Idle;
            t = 0;
            return;
        }
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
