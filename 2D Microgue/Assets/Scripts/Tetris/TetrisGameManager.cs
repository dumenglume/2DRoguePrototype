using System.Collections.Generic;
using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    static TetrisGameManager instance;
    public static TetrisGameManager Instance => instance;

    [SerializeField] TextMesh textMesh;

    [SerializeField] int stressLevel = 0;
    [SerializeField] int maxStressLevel = 10;
    public int StressLevel { get { return stressLevel; } set { stressLevel = value; } }

    List<GameObject> activeBlocks = new List<GameObject>();

    void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {
        
    }

    void Start()
    {
        UpdateStressLevelText();
    }

    public void IncreaseStressLevel(int _amount)
    {
        stressLevel += _amount;

        UpdateStressLevelText();
        CheckStressLevel();
    }

    void CheckStressLevel()
    {
        if (stressLevel == maxStressLevel)
        {
            Debug.Log("Stress level maxed.");
        }
    }

    void UpdateStressLevelText()
    {
        textMesh.text = stressLevel + "/" + maxStressLevel;
    }
}