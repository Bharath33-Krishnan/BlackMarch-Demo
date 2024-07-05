using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.PriorityQueue;
using System.Linq;
using UnityEditor;



//An implementation of the A* path finding algorithm
public class NavAgent : MonoBehaviour
{

    public Cell CurrentCell { get; set; }

    Cell Target;
    bool[] IsProcessed;
    int[] fCostMat;

    int currPosInd = 0;
    List<Vector2Int> path;

    public virtual void Start()
    {
        IsProcessed = new bool[10 * 10];
        fCostMat = new int[10 * 10];
        path = new List<Vector2Int>();
    }

    //Returns the next cell to which the agent needs to travel to
    public Cell MoveToPosition(Cell TargetCell)
    {
        if (CurrentCell == null || TargetCell == null)
        {
            return null;
        }
        if(CurrentCell == TargetCell)
        {
            return null;
        }
        if (TargetCell != Target)
        {
            Target = TargetCell;
        }
        setPath(CurrentCell,TargetCell);
        if (path == null)
            return null;
        path.Reverse();
        currPosInd = 0;
        for(int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(GridGenerator.getCellPos(path[i-1]) + Vector3.up, GridGenerator.getCellPos(path[i]) + Vector3.up,Color.red);
        }
        Cell nextTargetCell = GridGenerator.cells[path[currPosInd].x, path[currPosInd].y];
        if(nextTargetCell == CurrentCell)
        {
            currPosInd++;
            nextTargetCell = GridGenerator.cells[path[currPosInd].x, path[currPosInd].y];
        }

        return nextTargetCell;
    }

    public void setPath(Cell Start, Cell Stop)
    {
        Vector2Int _startID = Start.CellIndex;
        Vector2Int _stopID = Stop.CellIndex;
        path = getPath(_startID,_stopID);
    }

    public void ResetPath()
    {
        for (int i = 0; i < 100 ; i++)
        {
            IsProcessed[i] = false;
            fCostMat[i] = int.MaxValue;
        }
    }
    bool isPresentinGrid(Vector2Int vec)
    {
        if(vec.x >= 0 && vec.x < 10 && vec.y >= 0 && vec.y < 10)
        {
            return true;
        }
        return false;
    }


    // Currently doing for one cell component
    private List<Vector2Int> getPath(Vector2Int Start, Vector2Int Stop)
    {
        List<Vector2Int> pathList = new List<Vector2Int>();
        ResetPath();

        //Add a dummy node to initialize the priority queue
        Node min_p = new Node(-1 , new Vector2Int(),null);
        PriorityQueue<Node,Node> pq = new PriorityQueue<Node, Node>(min_p);

        //Initialize the current node
        int cost = (int)(Start - Stop).magnitude * 10 * 2;
        Node currNode = new Node(cost, Start,null);
        currNode.ParentNode = currNode;
        pq.Insert(currNode, currNode);
        int i = 0;

        //Loop until we find the path Iterative DP approach
        while (!pq.isEmpty())
        {
            currNode= pq.Top();
            i++;
            Vector2Int curr = currNode.currentVec;pq.Pop();

            //If curr node is stopping node we break
            if (curr == Stop)
                break;

            //Array of all adjacent cells that can be accessed, add the diagonals if needed
            Vector2Int[] arr = new Vector2Int[4];
            arr[0] = curr + new Vector2Int(0, 1);
            arr[1] = curr + new Vector2Int(0, -1);
            arr[2] = curr + new Vector2Int(1, 0);
            arr[3] = curr + new Vector2Int(-1, 0);
           // arr[4] = curr + new Vector2Int(1, 1);
           // arr[5] = curr + new Vector2Int(-1, 1);
           // arr[6] = curr + new Vector2Int(1, -1);
           // arr[7] = curr + new Vector2Int(-1, -1);

            //set fcost and gcost for every new nodes and mark them as processed and add them to the piority queue
            foreach(Vector2Int vec in arr)
            {
                int id = vec.y * 10 + vec.x;
                if (isPresentinGrid(vec) && GridGenerator.isTraversible(vec.x,vec.y) && !IsProcessed[id])
                {
                    Vector2Int vecFromStart = (vec - Start);
                    int startCost = (int)(vecFromStart.magnitude*10);
                    Vector2Int vecFromStop = (vec - Stop);
                    int endCost = (int)(vecFromStop.magnitude*10);
                    int fCost =  (startCost + endCost);
                    if (fCost < fCostMat[id])
                    {
                        fCostMat[id] = fCost;
                        Node a = new Node(fCost, vec,currNode);
                        a.gCost = startCost;
                        a.hCost = endCost;
                        pq.Insert(a, a);
                    }
                    IsProcessed[id] = true;
                }
                if (isPresentinGrid(vec) && !GridGenerator.isTraversible(vec.x,vec.y) && !IsProcessed[id] && vec == Stop)
                {
                    Vector2Int vecFromStart = (vec - Start);
                    int startCost = (int)(vecFromStart.magnitude*10);
                    Vector2Int vecFromStop = (vec - Stop);
                    int endCost = (int)(vecFromStop.magnitude*10);
                    int fCost =  (startCost + endCost);
                    if (fCost < fCostMat[id])
                    {
                        fCostMat[id] = fCost;
                        Node a = new Node(fCost, vec,currNode);
                        a.gCost = startCost;
                        a.hCost = endCost;
                        pq.Insert(a, a);
                    }
                    IsProcessed[id] = true;
                  
                }

            }
        }

        if (currNode.currentVec != Stop)
            return null;

        while(currNode.currentVec != Start)
        {
            pathList.Add(currNode.currentVec);
            currNode = currNode.ParentNode;
        }

        return pathList;
    }
    private class Node:IComparable<Node>
    {
        //How far current node is from the end node
        public int hCost=-1;
        //How far current node is from start node
        public int gCost=-1;

        //fCost = gCost + hCost
        public int fCost=-1;
        
        //Index of current grid cell
        public Vector2Int currentVec;

        //Parent from which the calculation proceed,this helps us to back track to get the path
        public Node ParentNode = null;

        public Node(int fCost,Vector2Int currentVec,Node ParentNode)
        {
            this.fCost = fCost;
            this.currentVec = currentVec;
            this.ParentNode = ParentNode;
        }
        int IComparable<Node>.CompareTo(Node x)
        {
            if (x.fCost > this.fCost)
                return -1;
            else if (x.fCost < this.fCost)
                return 1;
            else
            {
                if (x.hCost > this.hCost)
                    return -1;
                if (x.hCost < this.hCost)
                    return 1;
                else
                    return 0;
            }
        }
    }
}
