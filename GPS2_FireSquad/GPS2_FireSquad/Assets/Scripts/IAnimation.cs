using System;
using UnityEngine;

public interface IAnimation 
{
    void Walking(bool isWalking);
    void UsingMainSkill(bool isUsingSkill);
    void UsingSecondarySkill(bool isUsingSkill);
    void UsingUniqueSkill(string skillName, bool isUsingSkill);
}

public interface IPlayer
{
    void Stun(PlayerMovement playerMovement);
    void UnStun(PlayerMovement playerMovement);
    void SpawnFire(PlayerMovement playerMovement, GameObject firePrefab);
    void RemoveFire(PlayerMovement playerMovement);
}


