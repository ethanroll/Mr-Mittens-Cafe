using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.InputSystem;

public class NPCListUI : MonoBehaviour
{
    public static NPCListUI Instance;

    [SerializeField] private GameObject npcList;
    [SerializeField] private Transform npcTextPrefabParent;
    [SerializeField] private TextMeshProUGUI npcTextPrefab;
    [SerializeField] private ScrollRect npcScrollRect;


    void Awake()
    {
        Instance = this;
    }


    public void PrintAllActiveNPCS()
    {
        // CancelManager.Instance.SetCancellable(this);

        CancelManager.Instance.cancelButton.gameObject.SetActive(true);
        npcList.SetActive(true);

        StringBuilder sb = new StringBuilder();


        // print each active npc
        if (NPC_Manager.Instance.activeNPCs.Count != 0)
        {

        }
    }
    // print all npcs
}
