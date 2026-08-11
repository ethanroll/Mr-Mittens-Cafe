using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    //public NPC[] NPC_Prefabs;

    public static NPC_Manager Instance;

    public List<NPC> NPC_List = new List<NPC>();

    [SerializeField] public NPC prefab; 
    [SerializeField] public Transform spawnPoint;    // where NPCs spawn
    [SerializeField] public Transform waypointContainer;

    public int totalNumNPCs = 0;    // store how many NPCs were spawned total in the game
    public int numNPC_Cap = 20;    // npc cap for the day

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
        while (!RoundManager.Instance.isRoundOver)
        {
            if (totalNumNPCs < numNPC_Cap)
            {
                if (NPC_QueueManager.Instance.IsNPC_StartQueueFull())
                {
                    yield return new WaitForSeconds(1f); // wait to check again
                    continue;
                }

                NPC newNPC = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

                newNPC.NPC_Number = totalNumNPCs;
                totalNumNPCs++;
                NPC_List.Add(newNPC);

                NPC_Movement movement = newNPC.GetComponent<NPC_Movement>();
                movement.waypoints = waypointContainer.Cast<Transform>().ToArray();

                yield return new WaitForSeconds(5f);
            }            
        }
    }
}
