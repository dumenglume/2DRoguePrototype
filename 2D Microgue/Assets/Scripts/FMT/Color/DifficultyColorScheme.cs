using UnityEngine;

namespace FMT
{
[CreateAssetMenu(fileName = "DifficultyColorScheme", menuName = "FMT/DifficultyColorScheme", order = 0)]
public class DifficultyColorScheme : ScriptableObject
{
    [Tooltip("Green, Yellow, Red, Purple")]
    [SerializeField] Color[] colorArray = new Color[]
    {
        Color.green,
        Color.yellow,
        Color.red
    };

    public Color[] ColorArray => colorArray;
}
}