using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;


    void Awake()
    {
        Instance = this;
    }
}
