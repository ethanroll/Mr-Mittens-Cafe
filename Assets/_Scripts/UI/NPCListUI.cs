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

    private NPC currentNPC;

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
            for (int i = 0; i < NPC_Manager.Instance.activeNPCs.Count; i++)
            {
                currentNPC = NPC_Manager.Instance.activeNPCs[i];

                // only show NPCs whose order has actually been taken
                if (!currentNPC.orderGiven)
                {
                    continue;
                }

                // check if any orders were taken yet
                if (currentNPC.CurrentOrder == null)
                {
                    // newTextBox.text = "No orders yet.";
                    continue;
                }


                sb.AppendLine($"NPC number: {currentNPC.NPC_Number}:");

                // print npc & instantiate the text box prefab
                TextMeshProUGUI newTextBox = Instantiate(npcTextPrefab, npcTextPrefabParent, false);
                newTextBox.text = sb.ToString();
                sb.Clear();

                // make it clickable
                NPC chosen = currentNPC;
                newTextBox.GetComponent<Button>().onClick.AddListener(() => OnNPCListUIButtonPressed(chosen));
            }
            // Canvas.ForceUpdateCanvases();
        }
    }


    public void OnNPCListUIButtonPressed(NPC npc)
    {
        Debug.Log($"Chosen npc: {npc.NPC_Number}");

        // have npc walk over


        // npc stop at counter


        // check order
        // npc.CheckOrder();




    }
}
