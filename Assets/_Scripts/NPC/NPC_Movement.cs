using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    private NPC npc; // reference to NPC

    public enum NPC_State { Spawn, WalkToCounter, InQueue, WaitForQueue, WaitAtCounter, WalkToPickup, WaitForPickup, OrderReceived };

    [SerializeField] public Transform[] waypoints = new Transform[3];// where NPC go next
    [SerializeField] private float speed = 5f;

    private NPC_State currentState = NPC_State.Spawn; // starting state
    private Transform targetWaypoint;

    private int currentWaypointIndex = 0;
    private int assignedQueueIndex = -1;
    private int assignedTableIndex = -1;
    private bool orderGiven = false;

    // change later
    private bool haveJoinedList = false;


    void Awake()
    {
        npc = GetComponent<NPC>();
    }

    void Update()
    {
        switch (currentState)
        {
            case NPC_State.Spawn:
                TryJoinQueue();
                break;

            case NPC_State.WalkToCounter:
                int counterIdx = Mathf.Min(1, waypoints.Length - 1);    // use index 1 for counter
                if (counterIdx >= 0 && waypoints.Length > 0)
                {
                    targetWaypoint = waypoints[counterIdx];
                    transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

                    if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
                    {
                        currentState = NPC_State.WaitAtCounter;
                    }
                }
                break;

            case NPC_State.WaitForQueue:
                TryJoinQueue();
                break;

            case NPC_State.InQueue:
                // move through queue if not at counter
                if (assignedQueueIndex >= 0 && assignedQueueIndex < NPC_QueueManager.Instance.queueSpotsStart.Length)
                {
                    targetWaypoint = NPC_QueueManager.Instance.queueSpotsStart[assignedQueueIndex];
                    if (targetWaypoint != null)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
                    }
                }

                // If front of line and counter is free, step up to counter
                if (assignedQueueIndex == 0 && !NPC_QueueManager.Instance.counterOccupied)
                {
                    if (targetWaypoint != null && Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
                    {
                        NPC_QueueManager.Instance.LeaveLine(npc); // step out of the line
                        NPC_QueueManager.Instance.counterOccupied = true;    // claim the counter
                        currentWaypointIndex = 1;
                        currentState = NPC_State.WalkToCounter;              // walk up to counter
                    }
                }
                break;

            case NPC_State.WaitAtCounter:
                break;  // just waiting

            case NPC_State.WalkToPickup:
                    // add npc to tables to wait for order HAVE CHECK LATER IF CANT JOIN
                    if (!haveJoinedList)
                    {
                        NPC_PickupManager.Instance.NPC_WaitingAtTables.Add(npc);
                        assignedTableIndex = NPC_PickupManager.Instance.FindOpenTable();  // set index
                        targetWaypoint = NPC_PickupManager.Instance.tables[assignedTableIndex];
                        haveJoinedList = true;
                    }

                    // move to table
                    if (targetWaypoint != null)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

                        if (targetWaypoint != null && Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
                            currentState = NPC_State.WaitForPickup;
                    }
                // NextWaypoint();
                break;

            case NPC_State.WaitForPickup:
                break;  // just waiting

            case NPC_State.OrderReceived:
                // move toward exit
                targetWaypoint = waypoints[currentWaypointIndex];
                transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

                // destroy NPC once order process is done
                if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
                {
                    Destroy(gameObject);
                    NPC_PickupManager.Instance.NPC_WaitingAtTables.Remove(npc);
                    NPC_PickupManager.Instance.tableOccupied[assignedTableIndex] = false;
                    //NPC_Manager.Instance.totalNumNPCs--;
                }
                break;
        }
    }

    public void NextWaypoint()
    {
        // check for waypoints
        if (waypoints.Length == 0) return;

        // move toward current waypoint
        targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
    }

    public void OrderGiven()
    {
        if (currentState == NPC_State.WaitAtCounter)
        {
            npc.orderGiven = true;
            NPC_QueueManager.Instance.counterOccupied = false;  // counter is vacant
            currentState = NPC_State.WalkToPickup;
        }
    }

    public void OrderReceived()
    {
        // exit store
        if (currentState == NPC_State.WaitForPickup)
        {
            currentWaypointIndex++;
            currentState = NPC_State.OrderReceived;
        }
    }

    // set the new queue index
    public void SetQueueIndex(int index)
    {
        assignedQueueIndex = index;
    }

    private void TryJoinQueue()
    {
        if (NPC_QueueManager.Instance.CanJoinQueue(npc))
        {
            assignedQueueIndex = NPC_QueueManager.Instance.NPC_StartLine.Count - 1;

            // If first in line and counter is vacant, walk straight to counter
            if (assignedQueueIndex == 0 && !NPC_QueueManager.Instance.counterOccupied)
            {
                NPC_QueueManager.Instance.LeaveLine(npc);
                NPC_QueueManager.Instance.counterOccupied = true;
                currentWaypointIndex = 1;
                currentState = NPC_State.WalkToCounter;
            }
            else
            {
                currentState = NPC_State.InQueue;
            }
        }
        else
        {
            currentState = NPC_State.WaitForQueue;
        }
    }
}
