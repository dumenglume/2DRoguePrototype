using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue.Entities
{
public class Container : Entity, IBlockMovement, ITakeDamage
{
    void Start()
    {
        BlocksMovement = true;
    }
}
}