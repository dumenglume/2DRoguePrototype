using UnityEngine;

[CreateAssetMenu(fileName = "HBDifficultyColorScheme", menuName = "2D Microgue/HBDifficultyColorScheme", order = 0)]
public class HBDifficultyColorScheme : ScriptableObject
{
    [Tooltip("Green, Yellow, Red, Purple")]
    [SerializeField] Color[] colorArray = new Color[]
    {
        Color.green,
        Color.yellow,
        Color.red,
        Color.magenta // * Purple
    };

    public Color[] ColorArray => colorArray;
}
