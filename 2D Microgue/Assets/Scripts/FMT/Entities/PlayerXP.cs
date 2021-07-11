using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class PlayerXP : MonoBehaviour
{
    [SerializeField] int currentXP = 0;
    public int CurrentXP { get { return currentXP; } set { currentXP = value; } }
    [SerializeField] int nextLevel = 10;
    public int NextLevel { get { return nextLevel; } set { nextLevel = value; } }
}
}