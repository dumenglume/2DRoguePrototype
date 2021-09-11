using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue
{
public class EntityState
{
    public enum PlayerState { normal, shoot, haste, wall };
    public PlayerState PlayerSkillState { get; set; } = PlayerState.normal;

    public void ChangePlayerState(bool stateEnabled, int stateIndex) // TODO Find better way to implement this
    {
        if (stateEnabled)
            PlayerSkillState = (PlayerState) stateIndex;

        else
            PlayerSkillState = PlayerState.normal;
    }

    public void GetPlayerState()
    {

    }

    public void SetPlayerState()
    {

    }
}
}