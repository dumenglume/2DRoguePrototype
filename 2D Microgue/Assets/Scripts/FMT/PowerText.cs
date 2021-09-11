using UnityEngine;

public class PowerText : MonoBehaviour
{
    [SerializeField] TextMesh powerSpriteText;
    [SerializeField] IntReference powerReference;

    void Update()
    {
        powerSpriteText.text = powerReference.Value.ToString();
    }
}
