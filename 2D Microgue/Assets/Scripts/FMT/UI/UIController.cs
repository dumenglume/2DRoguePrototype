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

    [SerializeField] Text textPower;
    [SerializeField] Text textXP;
    [SerializeField] Text textGold;

    Player player;
    EntityPower playerPower;
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
        EntityPower.PowerChanged     += UpdateText;
        PlayerGold.GoldAmountChanged += UpdateText;
    }

    void OnDisable()
    {
        Player.PlayerSpawned         -= GetPlayer;
        EntityPower.PowerChanged     -= UpdateText;
        PlayerGold.GoldAmountChanged -= UpdateText;
    }

    void GetPlayer(Player _player)
    {
        player       = _player;
        playerPower  = _player.EntityPower;
        playerXP     = _player.PlayerXP;
        playerGold   = _player.PlayerGold;
    }

    void UpdateText()
    {
        textPower.text = "Power: " + playerPower.PowerCurrent.ToString() + "/" + playerPower.PowerMax.ToString();
        textXP.text    = "XP: "    + playerXP.CurrentXP.ToString();
        textGold.text  = "Gold: "  + "0";
    }
}
}