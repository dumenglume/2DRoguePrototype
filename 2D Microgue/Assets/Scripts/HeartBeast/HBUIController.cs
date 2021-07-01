using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://youtu.be/6ztY9-IX3Qg?t=1481

namespace HB
{
public class HBUIController : MonoBehaviour
{
    public static HBUIController instance;
    public static HBUIController Instance => instance;

    [SerializeField] Text textPowerLevel;
    [SerializeField] Text textHealth;
    [SerializeField] Text textAttackPower;
    [SerializeField] Text textXP;
    [SerializeField] Text textGold;

    HBPlayer player;
    HBEntityHealth playerHealth;
    HBEntityCombat playerCombat;
    HBPlayerXP playerXP;
    HBPlayerGold playerGold;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {
        HBPlayer.PlayerSpawned         += GetPlayer;
        HBEntityHealth.HealthChanged   += UpdateText;
        HBPlayerGold.GoldAmountChanged += UpdateText;
    }

    void OnDisable()
    {
        HBPlayer.PlayerSpawned         -= GetPlayer;
        HBEntityHealth.HealthChanged   -= UpdateText;
        HBPlayerGold.GoldAmountChanged -= UpdateText;
    }

    void GetPlayer(HBPlayer _player)
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