using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RaycastDebug : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.scroll.ReadValue().y != 0f)
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ped, results);

            Debug.Log("--- Scroll hit ---");
            foreach (var r in results)
                Debug.Log(r.gameObject.name);
        }
    }
}