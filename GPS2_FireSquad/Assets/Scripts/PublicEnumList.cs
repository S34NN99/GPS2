using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEnumList
{
    public enum CharacterType {Extinguisher = 0, Medic, Demolisher }
    public enum CharacterSkill { Extinguish, Carry, Break, DemolishTrap, Heal, Press, InteractDoor}
    public enum CoroutineType { Main, Secondary, CarryingVictim, PressButton}
}
