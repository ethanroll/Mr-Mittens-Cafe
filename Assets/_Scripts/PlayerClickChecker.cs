using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickChecker : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public static PlayerClickChecker Instance;

    public bool mouseHeld = false;
    public float mouseHeldTimer = 0f;
}
