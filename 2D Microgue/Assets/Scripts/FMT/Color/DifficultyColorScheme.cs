using UnityEngine;

namespace FMT
{
[CreateAssetMenu(fileName = "DifficultyColorScheme", menuName = "FMT/DifficultyColorScheme", order = 0)]
public class DifficultyColorScheme : ScriptableObject
{
    [Tooltip("Green, Yellow, Red, Purple")]
    [SerializeField] Color[] colorArray = new Color[]
    {
        Color.white,
        Color.green,
        Color.yellow,
        Color.red,
        Color.magenta // * Purple
    };

    public Color[] ColorArray => colorArray;
}
}