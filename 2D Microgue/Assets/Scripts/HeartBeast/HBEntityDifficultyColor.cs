using System.Collections.Generic;
using UnityEngine;

public class HBEntityDifficultyColor : MonoBehaviour
{
    [SerializeField] HBDifficultyColorScheme colorScheme;

    public enum DifficultyColor { green, yellow, red, purple }
    public DifficultyColor currentDifficultyColor = DifficultyColor.yellow;

    Dictionary<DifficultyColor, Color> difficultyColorDictionary;
    public Dictionary<DifficultyColor, Color> DifficultyColorDictionary => difficultyColorDictionary;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        InitializeDictionary();
    }

    void OnEnable()
    {
        SetSpriteColor();
    }

    void InitializeDictionary()
    {
        Color[] colorSchemeArray = colorScheme.ColorArray;

        difficultyColorDictionary = new Dictionary<DifficultyColor, Color>
        { 
            {DifficultyColor.green,  colorSchemeArray[0]},
            {DifficultyColor.yellow, colorSchemeArray[1]},
            {DifficultyColor.red,    colorSchemeArray[2]},
            {DifficultyColor.purple, colorSchemeArray[3]},
        };
    }

    void SetSpriteColor()
    {
        if (spriteRenderer == null) { throw new System.Exception("Sprite Renderer is null"); }

        spriteRenderer.color = difficultyColorDictionary[currentDifficultyColor];
    }

    public void UpdateDifficultyColor(int _enemyLevel, int _playerLevel)
    {
        // Set difficulty color
    }
}
