using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    private CanvasGroup hud;

    void Awake()
    {
        Instance = this;
        hud = GetComponent<CanvasGroup>();
    }

    public void SetHUDVisible(bool visible)
    {
        hud.alpha = visible ? 1f : 0f;
        hud.blocksRaycasts = visible;
        hud.interactable = visible;
    }
}
