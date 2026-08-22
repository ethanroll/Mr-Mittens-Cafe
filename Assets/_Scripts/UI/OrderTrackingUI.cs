using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls the Order Tracking UI popup window toggled by pressing the 'R' key.
/// Displays active customer orders, requested items, and fulfillment status across a spacious modal UI panel.
/// </summary>


public class OrderTrackingUI : MonoBehaviour
{
    public static OrderTrackingUI Instance;

    [SerializeField] private Transform ticketParent;
    [SerializeField] private GameObject ticketPrefab;

    void Awake()
    {
        Instance = this;
    }

    // add ticker per NPC
    public void AddTicket(NPC npc)
    {
        GameObject go = Instantiate(ticketPrefab, ticketParent);
        go.GetComponent<OrderTicketUI>().Setup(npc);
    }
    /*
    public static OrderTrackingUI Instance { get; private set; }

    [Header("UI Panels & Containers")]
    [SerializeField] private GameObject orderPanel;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private GameObject noOrdersMessage;

    [Header("Prefabs & Templates (Optional)")]
    [Tooltip("Optional prefab for order cards. If null, UI cards will be created dynamically.")]
    [SerializeField] private GameObject orderCardPrefab;

    [Header("Font & Display Settings")]
    [SerializeField] private float cardFontSize = 32f;
    [SerializeField] private float headerFontSize = 38f;

    [Header("Input Settings")]
    [SerializeField] private Key toggleKey = Key.R;

    private bool isWindowOpen = false;
    private TextMeshProUGUI noOrdersTextComp;
    private TextMeshProUGUI headerTextComp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupPanelLayouts();
    }

    private void Start()
    {
        if (orderPanel != null)
        {
            orderPanel.SetActive(false);
        }
        isWindowOpen = false;
    }

    private void Update()
    {
        // Detect key press using Unity's New Input System Keyboard class
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleOrderWindow();
        }
    }

    /// <summary>
    /// Configures RectTransforms and layouts dynamically so the UI takes up most of the screen.
    /// </summary>
    private void SetupPanelLayouts()
    {

        // If orderPanel is attached directly to this script or unassigned, resolve panel reference
        if (orderPanel == null)
        {
            orderPanel = gameObject;
        }

        // Configure Main Modal Panel to stretch across 80% of screen width and 75% height
        RectTransform panelRect = orderPanel.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            panelRect.anchorMin = new Vector2(0.1f, 0.12f);
            panelRect.anchorMax = new Vector2(0.9f, 0.88f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            // Ensure background image exists
            Image bg = orderPanel.GetComponent<Image>();
            if (bg == null) bg = orderPanel.AddComponent<Image>();
            bg.color = new Color(0.12f, 0.1f, 0.09f, 0.94f); // Deep cozy dark brown
        }

        // Setup Header Text
        Transform headerTransform = orderPanel.transform.Find("OrderUIHeader");
        if (headerTransform == null)
        {
            GameObject headerObj = new GameObject("OrderUIHeader", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            headerObj.transform.SetParent(orderPanel.transform, false);

            RectTransform headerRect = headerObj.GetComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0.03f, 0.88f);
            headerRect.anchorMax = new Vector2(0.97f, 0.98f);
            headerRect.offsetMin = Vector2.zero;
            headerRect.offsetMax = Vector2.zero;

            headerTextComp = headerObj.GetComponent<TextMeshProUGUI>();
            headerTextComp.fontSize = headerFontSize;
            headerTextComp.fontStyle = FontStyles.Bold;
            headerTextComp.color = new Color(1f, 0.84f, 0f); // Gold color
            headerTextComp.alignment = TextAlignmentOptions.Left;
            headerTextComp.text = "☕ ACTIVE CUSTOMER ORDERS (Press 'R' to Close)";
        }

        // Setup Cards Container with VerticalLayoutGroup and Scroll View
        if (cardsContainer == null)
        {
            Transform containerTransform = orderPanel.transform.Find("CardsContainer");
            if (containerTransform != null)
            {
                cardsContainer = containerTransform;
            }
            else
            {
                GameObject containerObj = new GameObject("CardsContainer", typeof(RectTransform), typeof(CanvasRenderer), typeof(VerticalLayoutGroup));
                containerObj.transform.SetParent(orderPanel.transform, false);
                cardsContainer = containerObj.transform;
            }
        }

        RectTransform containerRect = cardsContainer.GetComponent<RectTransform>();
        if (containerRect != null)
        {
            containerRect.anchorMin = new Vector2(0.03f, 0.04f);
            containerRect.anchorMax = new Vector2(0.97f, 0.86f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;
        }

        VerticalLayoutGroup vlg = cardsContainer.GetComponent<VerticalLayoutGroup>();
        if (vlg == null) vlg = cardsContainer.gameObject.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(20, 20, 16, 16);
        vlg.spacing = 16f;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // Setup No Orders Message
        if (noOrdersMessage == null)
        {
            Transform noOrdersTransform = orderPanel.transform.Find("NoOrdersText");
            if (noOrdersTransform != null)
            {
                noOrdersMessage = noOrdersTransform.gameObject;
            }
            else
            {
                GameObject noOrdersObj = new GameObject("NoOrdersText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
                noOrdersObj.transform.SetParent(orderPanel.transform, false);

                RectTransform noOrdersRect = noOrdersObj.GetComponent<RectTransform>();
                noOrdersRect.anchorMin = new Vector2(0.1f, 0.3f);
                noOrdersRect.anchorMax = new Vector2(0.9f, 0.7f);
                noOrdersRect.offsetMin = Vector2.zero;
                noOrdersRect.offsetMax = Vector2.zero;

                noOrdersTextComp = noOrdersObj.GetComponent<TextMeshProUGUI>();
                noOrdersTextComp.fontSize = headerFontSize;
                noOrdersTextComp.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
                noOrdersTextComp.alignment = TextAlignmentOptions.Center;
                noOrdersTextComp.text = "No active orders at the moment.\n<i>Enjoy a quiet cafe!</i>";
                noOrdersMessage = noOrdersObj;
            }
        }
    }

    /// <summary>
    /// Toggles the Order Tracking UI window open/closed.
    /// </summary>
    public void ToggleOrderWindow()
    {
        isWindowOpen = !isWindowOpen;

        if (orderPanel != null)
        {
            orderPanel.SetActive(isWindowOpen);
        }

        if (isWindowOpen)
        {
            RefreshOrderList();
        }
    }

    /// <summary>
    /// Re-populates the order cards list with current active customer orders.
    /// </summary>
    public void RefreshOrderList()
    {
        if (cardsContainer == null) return;

        // Clear existing card elements
        foreach (Transform child in cardsContainer)
        {
            Destroy(child.gameObject);
        }

        List<NPC> activeNPCs = OrderManager.Instance != null ? OrderManager.Instance.GetActiveNPCsWithOrders() : new List<NPC>();

        if (activeNPCs == null || activeNPCs.Count == 0)
        {
            if (noOrdersMessage != null)
            {
                noOrdersMessage.SetActive(true);
            }
            return;
        }

        if (noOrdersMessage != null)
        {
            noOrdersMessage.SetActive(false);
        }

        foreach (NPC npc in activeNPCs)
        {
            CreateOrderCard(npc);
        }
    }

    /// <summary>
    /// Creates a spacious UI card entry taking full width of the modal panel.
    /// </summary>
    private void CreateOrderCard(NPC npc)
    {
        if (npc == null) return;

        GameObject cardObj;

        if (orderCardPrefab != null)
        {
            cardObj = Instantiate(orderCardPrefab, cardsContainer);
            TMP_Text tmpText = cardObj.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.fontSize = cardFontSize;
                tmpText.enableWordWrapping = true;
                tmpText.text = GetFormattedCardContent(npc);
            }
        }
        else
        {
            // Create full-width dynamic UI card
            cardObj = new GameObject($"OrderCard_Customer_{npc.NPC_Number}", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            cardObj.transform.SetParent(cardsContainer, false);

            Image bgImage = cardObj.GetComponent<Image>();
            bgImage.color = new Color(0.22f, 0.19f, 0.17f, 0.95f); // Warm dark cafe card color

            VerticalLayoutGroup layout = cardObj.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(24, 24, 20, 20);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;

            ContentSizeFitter csf = cardObj.GetComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Add Text Mesh Pro component for content
            GameObject textObj = new GameObject("CardText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            textObj.transform.SetParent(cardObj.transform, false);

            TextMeshProUGUI textComp = textObj.GetComponent<TextMeshProUGUI>();
            textComp.fontSize = cardFontSize;
            textComp.color = Color.white;
            textComp.enableWordWrapping = true;
            textComp.overflowMode = TextOverflowModes.Overflow;
            textComp.alignment = TextAlignmentOptions.TopLeft;
            textComp.lineSpacing = 10f;
            textComp.text = GetFormattedCardContent(npc);
        }
    }

    /// <summary>
    /// Formats the order details string for a given NPC.
    /// </summary>
    private string GetFormattedCardContent(NPC npc)
    {
        string header = $"<b><size={headerFontSize}><color=#FFD700>Customer #{npc.NPC_Number}</color></size></b>\n";
        List<string> itemLines = npc.GetFormattedOrderDetails();

        if (itemLines.Count == 0)
        {
            return header + "<i>No items requested.</i>";
        }

        string details = string.Join("\n", itemLines);
        return header + details;
    }
    */ 
}
