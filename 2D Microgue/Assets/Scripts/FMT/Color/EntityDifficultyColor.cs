using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EntityDifficultyColor : MonoBehaviour
{
    [SerializeField] DifficultyColorScheme colorScheme;

    public enum DifficultyColor { white, green, yellow, red, purple }
    public DifficultyColor currentDifficultyColor = DifficultyColor.yellow;

    Dictionary<DifficultyColor, Color> difficultyColorDictionary;
    public Dictionary<DifficultyColor, Color> DifficultyColorDictionary => difficultyColorDictionary;

    SpriteRenderer spriteRenderer;
    Entity thisEntity;
    Entity player;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        InitializeDictionary();
    }

    void OnEnable()
    {
        UpdateDifficultyColor();
        SetSpriteColor();

        EntityHealth.HealthChanged += UpdateDifficultyColor;
    }

    void OnDisable()
    {
        EntityHealth.HealthChanged -= UpdateDifficultyColor;
    }

    void Start()
    {
        
    }

    void InitializeDictionary()
    {
        Color[] colorSchemeArray = colorScheme.ColorArray;

        difficultyColorDictionary = new Dictionary<DifficultyColor, Color>
        { 
            {DifficultyColor.white,  colorSchemeArray[0]},
            {DifficultyColor.green,  colorSchemeArray[1]},
            {DifficultyColor.yellow, colorSchemeArray[2]},
            {DifficultyColor.red,    colorSchemeArray[3]},
            {DifficultyColor.purple, colorSchemeArray[4]},
        };
    }

    void SetSpriteColor() // TODO Change this to change color of outline vs. entire sprite
    {
        if (spriteRenderer == null) { throw new System.Exception("Sprite Renderer is null"); }

        spriteRenderer.color = difficultyColorDictionary[currentDifficultyColor];
    }

    public void UpdateDifficultyColor() // TODO Make this cleaner
    {
        // Debug.Log("Updating difficulty colors");

        player     = FindObjectOfType<Player>();
        thisEntity = GetComponent<Entity>();

        int playerPowerLevel          = player.EntityCombat.PowerLevel;
        int playerAttackPower         = player.EntityCombat.AttackPower;
        int playerHealth              = player.EntityHealth.HealthCurrent;

        int thisEntityPowerLevel      = thisEntity.EntityCombat.PowerLevel;
        int thisEntityAttackPower     = thisEntity.EntityCombat.AttackPower;
        int thisEntityHealth          = thisEntity.EntityHealth.HealthCurrent;

        if (thisEntityAttackPower > playerHealth)          { currentDifficultyColor = DifficultyColor.purple; return; }

        else if (thisEntityPowerLevel > playerPowerLevel)  { currentDifficultyColor = DifficultyColor.red; return; }

        else if (thisEntityPowerLevel == playerPowerLevel) { currentDifficultyColor = DifficultyColor.yellow; return; }

        else if (thisEntityPowerLevel < playerPowerLevel)  { currentDifficultyColor = DifficultyColor.green; return; }

        else if (thisEntityHealth < playerAttackPower)     { currentDifficultyColor = DifficultyColor.white; return; }
    }
}
}