using System;
using UnityEngine;

public interface IPlayer
{
    void Stun(PlayerMovement playerMovement);
    void UnStun(PlayerMovement playerMovement);
    void SpawnFire(PlayerMovement playerMovement, GameObject firePrefab);

    //ANIMATIONS
    void Walking(bool isWalking);
    void UsingMainSkill(bool isUsingSkill);
    void UsingSecondarySkill(bool isUsingSkill);
    void UniqueAnimation(string skillName, bool isUsingSkill);
}

public interface IFmod
{
    void StartAudioFmod(GameObject gameObject, string pathname);
    void StopAudioFmod(GameObject gameObject);
}

public interface IObjectives
{
    void AddToObjective();
}



