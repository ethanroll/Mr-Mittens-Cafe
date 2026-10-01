using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OrderListUI : MonoBehaviour, ICancellable
{
    public static OrderListUI Instance;

    [SerializeField] private GameObject orderList;
    [SerializeField] private Transform orderTextPrefabParent;
    [SerializeField] private TextMeshProUGUI orderTextPrefab;
    [SerializeField] private ScrollRect orderScrollRect;

    private NPC currentNPC;
    private bool rPressed = false;

    void Awake()
    {
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame && !rPressed)
        {
            PrintAllActiveOrders();
            rPressed = true;
        }

        else if (Keyboard.current[Key.R].wasPressedThisFrame && rPressed)
        {
            CloseOrderList();
        }
    }


    // print out list of all active orders (debug log for now)
    public void PrintAllActiveOrders()
    {
        CancelManager.Instance.SetCancellable(this);

        CancelManager.Instance.cancelButton.gameObject.SetActive(true);
        orderList.SetActive(true);

        StringBuilder sb = new StringBuilder();

        if (NPC_Manager.Instance.activeNPCs.Count != 0)
        {
            // get order for each npc
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

                // iterate if npc has more than one item for order
                for (int j = 0; j < currentNPC.CurrentOrder.requestedItems.Count; j++)
                {
                    Item currentItem = currentNPC.CurrentOrder.requestedItems[j];

                    if (currentItem is Drink drink)
                    {
                        sb.AppendLine($"   - {HotbarManager.Instance.GetCurrentItemName(drink)}");
                    }
                    if (currentItem is Food food)
                    {
                        sb.AppendLine($"   - {HotbarManager.Instance.GetCurrentItemName(food)}");
                    }
                }

                // print order 
                // instantiate the text box prefab
                TextMeshProUGUI newTextBox = Instantiate(orderTextPrefab);
                newTextBox.transform.SetParent(orderTextPrefabParent, false);

                newTextBox.text = sb.ToString();

                sb.Clear();
            }

            Canvas.ForceUpdateCanvases(); // Forces layout recalculation before snapping
            orderScrollRect.verticalNormalizedPosition = 1f;

        }
        else
        {
            Debug.Log("No orders");
        }
    }


    private void CloseOrderList()
    {
        CancelManager.Instance.cancelButton.gameObject.SetActive(false);
        orderList.SetActive(false);
        rPressed = false;
        DestroyPrefabs();
    }


    // destroy all prefabs
    private void DestroyPrefabs()
    {
        for (int i = orderTextPrefabParent.childCount - 1; i >= 0; i--)
        {
            Destroy(orderTextPrefabParent.GetChild(i).gameObject);
        }
    }


    public void Cancel()
    {
        CloseOrderList();
    }
}
