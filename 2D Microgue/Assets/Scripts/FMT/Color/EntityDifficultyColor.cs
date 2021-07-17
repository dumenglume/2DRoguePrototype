using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EntityDifficultyColor : MonoBehaviour
{
    [SerializeField] DifficultyColorScheme colorScheme;

    public enum DifficultyColor { green, yellow, red }
    public DifficultyColor currentDifficultyColor = DifficultyColor.yellow;

    Dictionary<DifficultyColor, Color> difficultyColorDictionary;
    public Dictionary<DifficultyColor, Color> DifficultyColorDictionary => difficultyColorDictionary;

    SpriteRenderer spriteRenderer;
    Enemy thisEntity;
    Player player;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        thisEntity     = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        Player.PlayerSpawned     += GetPlayer;
        PlayerPower.PowerChanged += UpdateDifficultyColor;
    }

    void OnDisable()
    {
        Player.PlayerSpawned     -= GetPlayer;
        PlayerPower.PowerChanged -= UpdateDifficultyColor;
    }

    void Start() => InitializeDictionary();

    void GetPlayer(Player playerEntity)
    {
        player = playerEntity;

        if (player == null) return;

        UpdateDifficultyColor();
    }

    public void InitializeDictionary()
    {
        Color[] colorSchemeArray = colorScheme.ColorArray;

        difficultyColorDictionary = new Dictionary<DifficultyColor, Color>
        { 
            {DifficultyColor.green,  colorSchemeArray[1]},
            {DifficultyColor.yellow, colorSchemeArray[2]},
            {DifficultyColor.red,    colorSchemeArray[3]}
        };
    }

    public void UpdateDifficultyColor() // TODO Make this cleaner
    {
        if (player == null) return;

        int playerPower     = player.PlayerPower.PowerCurrent;
        int thisEntityPower = thisEntity.EnemyPower.PowerCurrent;

        if (thisEntityPower >= playerPower) { currentDifficultyColor = DifficultyColor.red; }
        else { currentDifficultyColor = DifficultyColor.green; }

        UpdateSpriteColor();
    }

    void UpdateSpriteColor() // TODO Change this to change color of outline vs. entire sprite
    {
        if (spriteRenderer == null) { throw new System.Exception("Sprite Renderer is null"); }

        //Debug.Log($"Sprite Renderer: {spriteRenderer}");
        //Debug.Log($"Difficulty Color: {difficultyColorDictionary[currentDifficultyColor]}");

        spriteRenderer.color = difficultyColorDictionary[currentDifficultyColor];
    }
}
}