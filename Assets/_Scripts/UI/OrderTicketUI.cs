using UnityEngine;
using TMPro;

public class OrderTicketUI : MonoBehaviour
{
    /*
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI itemListText;
    [SerializeField] private Slider timerBar; /*/ //optional

    [SerializeField] private GameObject orderTextParent;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text orderText;

    // setup order ticket prefab
    public void Setup(NPC npc)
    {
        npcNameText.text = "NPC No. " + npc.NPC_Number;

        // build up order item list as block of text
        orderText.text = string.Join("\n", npc.GetFormattedOrderDetails());
    }
}
