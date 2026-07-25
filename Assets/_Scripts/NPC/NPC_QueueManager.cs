using UnityEngine;
using System.Collections.Generic;

public class NPC_QueueManager : MonoBehaviour
{
    public static NPC_QueueManager Instance;

    public List<NPC> NPC_StartLine = new List<NPC>();
    public List<NPC> NPC_EndLine = new List<NPC>();

    [SerializeField] public Transform[] queueSpotsStart = new Transform[5]; // store initial queue spots
    //[SerializeField] public Transform[] queueSpotsEnd = new Transfom[9]; // store waiting queue spots

    private bool queueFull;
    private Transform queueWaypoint; // get waypoint for queue

    public bool counterOccupied = false; // store if counter is currently occuipied by NPC


    void Awake()
    {
        Instance = this;
    }

    public bool IsNPC_StartQueueFull()
    {
        if (NPC_StartLine.Count >= queueSpotsStart.Length)
            queueFull = true;
        else
            queueFull = false;

        return queueFull;
    }

    /*
    public void AddNPC_ToQueue(NPC npc)
    {
        num_NPC_InQueue++;
    }

    public NPC RemoveNPC_FromQueue(NPC npc)
    {

    } */

    
    public Transform GetOpenQueueSpot()
    {
        for (int i = 0; i < queueSpotsStart.Length; i++)
        {
            if (NPC_StartLine[i] == null)
            {
                queueWaypoint = queueSpotsStart[i];
                break;
            }
            else
            {
                queueWaypoint = null;
            }
            
        }
        return queueWaypoint;
    } 

    // see of NPC can join line queue
    public bool CanJoinQueue(NPC npc)
    {
        // if line full can't join queue
        if (NPC_StartLine.Count >= queueSpotsStart.Length)
            return false;

        // add to queue if not full
        NPC_StartLine.Add(npc);
        return true;
    }

    // NPC leave line
    public void LeaveLine(NPC npc)
    {
        NPC_StartLine.Remove(npc);

        // other NPCs move up line
        for(int i = 0; i < NPC_StartLine.Count; i++)
        {
            NPC_StartLine[i].GetComponent<NPC_Movement>().SetQueueIndex(i);
        }
    }
}
