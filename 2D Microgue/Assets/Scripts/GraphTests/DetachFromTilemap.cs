using UnityEngine;

namespace GraphTest
{
public class DetachFromTilemap : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null);
    }
}
}