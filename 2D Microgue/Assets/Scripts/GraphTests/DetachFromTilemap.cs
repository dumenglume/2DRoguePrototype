using UnityEngine;

public class DetachFromTilemap : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null);
    }
}
