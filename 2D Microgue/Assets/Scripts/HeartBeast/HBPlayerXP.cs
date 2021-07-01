using UnityEngine;

namespace HB
{
public class HBPlayerXP : MonoBehaviour
{
    [SerializeField] int currentXP = 0;
    public int CurrentXP { get { return currentXP; } set { currentXP = value; } }
    [SerializeField] int nextLevel = 10;
    public int NextLevel { get { return nextLevel; } set { nextLevel = value; } }
}
}