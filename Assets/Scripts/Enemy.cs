using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : NavAgent
{
    public Vector2Int MyCellPos;
    public float PathPrecision = 100;
    public EnemyStates current_state = EnemyStates.Idle;
    public GameObject ObjGfx;

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
            Debug.LogError("Enter a Valid Cell Pos for player");
            Destroy(gameObject);
        }
        CurrentCell = GridGenerator.getCell(MyCellPos);
        transform.position = CurrentCell.transform.position;
        rb = GetComponent<Rigidbody>();
        precision = 1 / PathPrecision;
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
        rb.velocity = Vector3.zero;
        if (CurrentCell == null)
            return;

    }

    public void Walk()
    {
        Cell nextCell = MoveToPosition(current_target);
        if(nextCell == null)
        {
            current_state = EnemyStates.Idle;
            t = 0;
            return;
        }
        if ((rb.position - nextCell.transform.position).magnitude> precision)
        {
            rb.velocity = (nextCell.transform.position - rb.position).normalized;
        }
        else
        {
            CurrentCell.IsObstacle = false;
            CurrentCell = nextCell;
            t = 0;
        }

        Vector3 lookPos = (nextCell.transform.position - transform.position).normalized;
            
        t += Time.deltaTime;
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
