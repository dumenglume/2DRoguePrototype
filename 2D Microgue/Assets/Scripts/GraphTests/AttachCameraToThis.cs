using UnityEngine;

namespace GraphTest
{
public class AttachCameraToThis : MonoBehaviour
{
    void Start()
    {
        Camera.main.transform.SetParent(transform);
    }
}
}