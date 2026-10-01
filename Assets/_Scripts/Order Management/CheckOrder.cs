using UnityEngine;


// 1. go to counter and ui pops up
// 2. choose which order you are going to give
//      - wait if you want to give more
// 3. npc walks to counter
// 4. when at counter check if order matches

/* ---- how points work ----
 * 
 * 1. get point for each attribute you have
 * 2. max points = 100
 * 3. subtract for everything incorrect
 * 
 * 
*/ 


public class CheckOrder : MonoBehaviour, IInteractable
{
    public static CheckOrder Instance;

    private bool checkingOrder = false;

    void Awake()
    {
        Instance = this;
    }

    public bool CanInteract()
    {
        return true;
        // return !IsOpened;
    }

    public void Interact()
    {
        // 1. prompt shows up, place order
        // 2. prompt again if more than 1
        // 3. user click done placing order
        // 4. prompt for which npc order you would like to give


        checkingOrder = true;
        OrderListUI.Instance.PrintAllActiveOrders();

        // get order
    }


    // prompt user for the order(s) they want to place
    private PromptForOrder()
    {
        Debug.Log("Place the order or orders you want to complete.");
    }


    public bool isUserCheckingOrder()
    {
        return checkingOrder;
    }
}


/*
 * using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    readonly Stack<GameObject> stack = new();

    void Start()
    {
        // hide everything except the start screen in the inspector,
        // then:
        stack.Push(startScreen);
        startScreen.SetActive(true);
    }

    public void Open(GameObject next)
    {
        stack.Peek().SetActive(false); // hide, don't destroy
        stack.Push(next);
        next.SetActive(true);
    }

    public void Back()
    {
        if (stack.Count <= 1) return; // can't go back from start screen
        stack.Pop().SetActive(false);
        stack.Peek().SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
    }
} */

