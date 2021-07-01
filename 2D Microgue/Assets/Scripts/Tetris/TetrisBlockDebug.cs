using UnityEngine;

public class TetrisBlockDebug : MonoBehaviour
{
    [SerializeField] TextMesh textMesh;
    TetrisBlockContainer blockContainer;

    void Awake()
    {
        blockContainer = GetComponentInParent<TetrisBlockContainer>();
        Debug.Log(blockContainer);

        UpdateText();
    }

    void OnEnable()
    {
        blockContainer.BlockContainerMoved   += UpdateText;
        blockContainer.BlockContainerRotated += UpdateText;
        blockContainer.BlockContainerStartedRotating += Rotate;
    }

    void OnDisable()
    {
        blockContainer.BlockContainerMoved   -= UpdateText;
        blockContainer.BlockContainerRotated -= UpdateText;
        blockContainer.BlockContainerStartedRotating -= Rotate;
    }

    void UpdateText()
    {
        float x = Mathf.RoundToInt(transform.position.x);
        float y = Mathf.RoundToInt(transform.position.y);

        textMesh.text = x + ", " + y;
    }

    void Rotate(Vector3 _directionToRotate, float _angle, float movementDuration)
    {
        LeanTween.rotateAround(gameObject, _directionToRotate, _angle, movementDuration).setEaseInOutCubic();
    }
}
