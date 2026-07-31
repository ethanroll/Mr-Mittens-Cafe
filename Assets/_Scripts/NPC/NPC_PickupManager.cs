using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_PickupManager : MonoBehaviour
{
    public static NPC_PickupManager Instance;

    public List<NPC> NPC_WaitingAtTables = new List<NPC>();
    [SerializeField] public Transform[] tables = new Transform[10];   // store table spots
    public bool[] tableOccupied = new bool[10];

    private int foundTableIndex;
    private bool foundIndex;

    void Awake()
    {
        Instance = this;
    }

    public int FindOpenTable()
    {
        foundIndex = false;

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
        return foundTableIndex;
    }

    public bool CheckTablesFull()
    {
        // check if tables are full
        if (tableOccupied.All(t => t))
        {
            return true;
        }
        return false;
    }
}
