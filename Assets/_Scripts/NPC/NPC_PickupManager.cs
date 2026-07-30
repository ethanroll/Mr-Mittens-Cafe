using System.Collections.Generic;
using UnityEngine;

public class NPC_PickupManager : MonoBehaviour
{
    public static NPC_PickupManager Instance;

    public List<NPC> NPC_WaitingAtTables = new List<NPC>();
    [SerializeField] public Transform[] tables = new Transform[10];   // store table spots
    public bool[] tableOccupied = new bool[10];

    public bool tablesFull = false;
    private int foundTableIndex;
    private bool foundIndex;

    void Awake()
    {
        Instance = this;
    }

    public int FindOpenTable()
    {
        // do an if tables full return -1
        foundIndex = false;
        // int attempts = 0;

        while (!foundIndex)
        {
            foundTableIndex = UnityEngine.Random.Range(0, tables.Length);
            if (!tableOccupied[foundTableIndex])
            {
                tableOccupied[foundTableIndex] = true;
                foundIndex = true;

                return foundTableIndex;
            }
        }

        /*/ have attempts to see if nothings open 
        attempts++;
        if (attempts >= tables.Length)
        {
            tablesFull = true; // we've tried enough times to conclude nothing's open
            return -1;
        }
        */
        return foundTableIndex;
    }
}
