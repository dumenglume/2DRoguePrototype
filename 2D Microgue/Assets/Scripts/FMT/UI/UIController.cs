using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://youtu.be/6ztY9-IX3Qg?t=1481

namespace FMT
{
public class UIController : MonoBehaviour
{
    public static UIController instance;
    public static UIController Instance => instance; // TODO Make not a singleton

    [SerializeField] Text textPowerLevel;
    [SerializeField] Text textHealth;
    [SerializeField] Text textAttackPower;
    [SerializeField] Text textXP;
    [SerializeField] Text textGold;

    Player player;
    EntityHealth playerHealth;
    EntityCombat playerCombat;
    PlayerXP playerXP;
    PlayerGold playerGold;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {
        Player.PlayerSpawned         += GetPlayer;
        EntityHealth.HealthChanged   += UpdateText;
        PlayerGold.GoldAmountChanged += UpdateText;
    }

    void OnDisable()
    {
        Player.PlayerSpawned         -= GetPlayer;
        EntityHealth.HealthChanged   -= UpdateText;
        PlayerGold.GoldAmountChanged -= UpdateText;
    }

    void GetPlayer(Player _player)
    {
        player       = _player;
        playerHealth = _player.EntityHealth;
        playerCombat = _player.EntityCombat;
        playerXP     = _player.PlayerXP;
        playerGold   = _player.PlayerGold;
    }

    void UpdateText()
    {
        textPowerLevel.text  = "Level: "  + playerCombat.PowerLevel.ToString();
        textHealth.text      = "Health: " + playerHealth.HealthCurrent.ToString() + "/" + playerHealth.HealthMax.ToString();
        textAttackPower.text = "Attack: " + playerCombat.AttackPower.ToString();
        textXP.text          = "XP: "     + playerXP.CurrentXP.ToString();
        textGold.text        = "Gold: "   + "0";
    }
}
}