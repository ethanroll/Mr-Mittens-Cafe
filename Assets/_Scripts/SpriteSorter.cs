using UnityEngine;

/// <summary>
/// Automatically adjusts the SpriteRenderer sorting order based on Y-position for top-down 2.5D depth,
/// or sets a fixed sorting order offset so characters render above counters.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSorter : MonoBehaviour
{
    [Header("Sorting Settings")]
    [Tooltip("Set true to dynamically adjust sorting order based on Y-position (lower on screen = in front).")]
    [SerializeField] private bool dynamicSorting = true;

    [Tooltip("Base offset to add to the sorting order. Increase this (e.g. 10) to make this sprite render in front of counters.")]
    [SerializeField] private int sortingOrderOffset = 10;

    [Tooltip("If true, only calculates sorting order once in Start() instead of every frame.")]
    [SerializeField] private bool runOnce = false;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateSortingOrder();
    }

    private void LateUpdate()
    {
        if (!runOnce && dynamicSorting)
        {
            UpdateSortingOrder();
        }
    }

    public void UpdateSortingOrder()
    {
        if (spriteRenderer == null) return;

        if (dynamicSorting)
        {
            // Objects lower on screen (smaller Y) get a higher sorting order so they render in front
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + sortingOrderOffset;
        }
        else
        {
            spriteRenderer.sortingOrder = sortingOrderOffset;
        }
    }
}
