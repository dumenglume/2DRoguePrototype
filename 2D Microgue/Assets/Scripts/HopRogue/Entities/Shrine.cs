using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue.Entities
{
public class Shrine : Entity, ICanInteract
{
    public void PerformInteraction(Player player) // TODO Can change Player to Entity if allowing other entities to interact with each other
    {
        Debug.Log("Health increased by 1");

        player.IncreaseHealthAndMaxHealh(1);
        
        ResolveDeath();
    }
}
}