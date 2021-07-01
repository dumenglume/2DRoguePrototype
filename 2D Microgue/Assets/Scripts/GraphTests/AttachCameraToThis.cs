using UnityEngine;

public class AttachCameraToThis : MonoBehaviour
{
    void Start()
    {
        Camera.main.transform.SetParent(transform);
    }
}
