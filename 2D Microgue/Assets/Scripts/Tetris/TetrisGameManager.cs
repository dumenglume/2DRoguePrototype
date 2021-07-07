using System.Collections.Generic;
using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    static TetrisGameManager instance;
    public static TetrisGameManager Instance => instance;

    [SerializeField] TextMesh textMesh;

    [SerializeField] int overflowCount = 0;
    [SerializeField] int maxOverflowCount = 10;
    public int OverflowClound { get { return overflowCount; } set { overflowCount = value; } }

    List<GameObject> activeBlocks = new List<GameObject>();

    void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        UpdateOverflowText();
    }

    public void IncreaseOverflowCount(int _amount)
    {
        overflowCount += _amount;

        UpdateOverflowText();
        CheckOverflowCount();
    }

    void CheckOverflowCount()
    {
        if (overflowCount == maxOverflowCount)
        {
            Debug.Log("Stress level maxed.");
        }
    }

    void UpdateOverflowText()
    {
        textMesh.text = overflowCount + "/" + maxOverflowCount;
    }
}