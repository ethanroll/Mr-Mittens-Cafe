using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    //public NPC[] NPC_Prefabs;

    public static NPC_Manager Instance;

    //List<NPC> NPC_List = new List<NPC>();
    //[SerializeField] public Transform[] queueSpots = new Transform[4]; // store queue spots

    [SerializeField] public NPC prefab; 
    [SerializeField] public Transform spawnPoint;    // where NPCs spawn
    [SerializeField] public Transform waypointContainer;

    public NPC currentNPC;  // store current NPC
    public int totalNumNPCs = 0;    // store how many NPCs are currently in the game

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnSystem());
    }

    IEnumerator SpawnSystem()
    {
        while (true)
        {
            if (totalNumNPCs < 20)
            {
                if (NPC_QueueManager.Instance.IsNPC_StartQueueFull())
                {
                    yield return new WaitForSeconds(1f); // wait to check again
                    continue;
                }

                NPC newNPC = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

                totalNumNPCs++;

                NPC_Movement movement = newNPC.GetComponent<NPC_Movement>();
                movement.waypoints = waypointContainer.Cast<Transform>().ToArray();

                yield return new WaitForSeconds(1f);
            }            
        }
    }
}
