using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    //public NPC[] NPC_Prefabs;

    public static NPC_Manager Instance;

    public List<NPC> NPC_List = new List<NPC>();    // store all NPCs that ever existed
    public List<NPC> activeNPCs = new List<NPC>();  // store active NPCs in game
    public List<NPC> completedNPCs = new List<NPC>();   // store npcs that had order finished NPC_Manager.Instance.completedNPCS

    [SerializeField] public NPC prefab; 
    [SerializeField] public Transform spawnPoint;    // where NPCs spawn
    [SerializeField] public Transform waypointContainer;

    public int totalNumNPCs = 0;    // store how many NPCs are currently in the game
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
            if (NPC_List.Count < numNPC_Cap)
            {
                if (NPC_QueueManager.Instance.IsNPC_StartQueueFull())
                {
                    yield return new WaitForSeconds(1f); // wait to check again
                    continue;
                }

                NPC newNPC = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                newNPC.NPC_Number = NPC_List.Count;

                // have order generated for npc
                Drink drink = new Drink();
                Food food = new Food();
                Order newOrder = OrderManager.Instance.GenerateRandomOrder(newNPC, drink, food);
                newNPC.AssignOrder(newOrder);

                // increment lists
                totalNumNPCs++;
                NPC_List.Add(newNPC);
                activeNPCs.Add(newNPC);

                // have npc move to initial spot
                NPC_Movement movement = newNPC.GetComponent<NPC_Movement>();
                movement.waypoints = waypointContainer.Cast<Transform>().ToArray();
                yield return new WaitForSeconds(5f);
            }            
        }
    }
}
