using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HopRogue
{
public class EntityFactory : MonoBehaviour
{
    public abstract class Ability
    {
        public abstract void Process();
    }

    public class StartFireAbility : Ability
    {
        public override void Process()
        {
            // Do fire stuff
        }
    }

    public class HealSelfAbility : Ability
    {
        public override void Process()
        {
            // self.Health++;
        }
    }

    public class AbilityFactory
    {
        public Ability GetAbilityOld(string abilityType)
        {
            switch (abilityType)
            {
                case "fire":
                    return new StartFireAbility();
                case "heal":
                    return new HealSelfAbility();
                default:
                    return null;
            }
        }
    }
}

public class AbilityFactoryOld
{
    public abstract class Ability
    {
        public abstract void Process();
    }

    public class StartFireAbility : Ability
    {
        public override void Process()
        {
            // Do fire stuff
        }
    }

    public class HealSelfAbility : Ability
    {
        public override void Process()
        {
            // self.Health++;
        }
    }

    public class AbilityFactory
    {
        public Ability GetAbilityOld(string abilityType)
        {
            switch (abilityType)
            {
                case "fire":
                    return new StartFireAbility();
                case "heal":
                    return new HealSelfAbility();
                default:
                    return null;
            }
        }
    }
}

}