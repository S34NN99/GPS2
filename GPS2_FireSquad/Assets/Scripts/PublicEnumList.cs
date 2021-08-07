using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEnumList
{
    public enum CharacterType {Extinguisher = 0, Medic, Demolisher }
    public enum CharacterSkill { Extinguish, Carry, Break, DemolishTrap, Heal, Press, InteractDoor}
    public enum CoroutineType { Main, Secondary, CarryingVictim, PressButton}
    public enum WallHP { Full, Crack, Broken }
    public enum LevelNum { Level_1, Level_2, Level_3, Level_4, Level_5, Level_6, Level_7}
}
