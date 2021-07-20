using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimation 
{
    void Walking(bool isWalking);
    void UsingMainSkill(bool isUsingSkill);
    void UsingSecondarySkill(bool isUsingSkill);
    void UsingUniqueSkill(string skillName, bool isUsingSkill);
}
