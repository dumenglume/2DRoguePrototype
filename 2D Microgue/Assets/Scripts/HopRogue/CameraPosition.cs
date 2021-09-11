using UnityEngine;

namespace HopRogue
{
public class CameraPosition : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Camera _camera;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void SetCameraPosition(float width, float height) // TODO may not need to be public
    {
        // width is (float) CurrentDungeon.Width / 2 - 0.5f
        // height is (float) CurrentDungeon.Height / 2 - 0.5f

        _camera = _camera ?? Camera.main;
        _camera.transform.position = new Vector3(width , height, -10f);
    }
}
}