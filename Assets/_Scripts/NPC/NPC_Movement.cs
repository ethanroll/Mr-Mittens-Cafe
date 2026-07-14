using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public static NPC_Movement Instance;
    public enum NPC_State { WalkToCounter, WaitAtCounter, WalkToPickup, WaitForPickup };

    [SerializeField] private Transform[] waypoints = new Transform[2];// where NPC go next
    [SerializeField] private float speed = 5f;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private NPC_State currentState = NPC_State.WalkToCounter; // starting state

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        switch (currentState)
        {
            case NPC_State.WalkToCounter:
                NextWaypoint();
                break;

            case NPC_State.WaitAtCounter:
                break;  // just waiting

            case NPC_State.WalkToPickup:
                NextWaypoint();
                break;

            case NPC_State.WaitForPickup:
                break;  // just waiting
        }
    }

    public void NextWaypoint()
    {
        // check for waypoints
        if (waypoints.Length == 0) return;

        // move toward current waypoint
        targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // check if checkpoint reached
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            WaypointReached();
        }
    }

    public void WaypointReached()
    {
        currentWaypointIndex++;

        if (currentState == NPC_State.WalkToCounter)
        {
            currentState = NPC_State.WaitAtCounter;
        }
        else if (currentState == NPC_State.WalkToPickup)
        {
            currentState = NPC_State.WaitForPickup;
        }
    }

    public void OrderGiven()
    {
        if (currentState == NPC_State.WaitAtCounter)
            currentState = NPC_State.WalkToPickup;
    }

    public void OrderReceived()
    {
        if (currentState == NPC_State.WaitForPickup)
            Destroy(gameObject);    // destroy NPC once order process is done
    }
}
