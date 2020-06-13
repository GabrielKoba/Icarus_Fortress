using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public float CannonBallSpeed = 5f;
    public float CannonBallDestroyDelay = 5f;

    [Header("Player")]
    public float PlayerMoveSpeed = 100f;
    public float PlayerJumpForce = 600f;
    public float PlayerGravity = 3f;
    public int PlayerStartingLives = 3;

    [Header("Enemy Spawner")]
    public float MinDelay = 2f;
    public float MaxDelay = 5f;

    [Header("Enemies")]
    public float SkyFlyMoveSpeed = 5f;
    public float CloudBeetleMoveSpeed = 4f;
    public float CloudBeetleSwitchLanesFrequencyMin = 0.5f;
    public float CloudBeetleSwitchLanesFrequencyMax = 3f;
    public float CloudBeetleSwitchLanesSpeed = 2f;
    public float WindWaspMoveSpeed = 8f;
}
