using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public float CannonBallBigSpeed = 2.5f;
    public float CannonBallSpeed = 5f;
    public float CannonBallDestroyDelay = 5f;

    [Header("Player")]
    public float PlayerMoveSpeed = 100f;
    public float PlayerJumpForce = 600f;
    public float PlayerGravity = 3f;
    public int PlayerStartingLives = 3;
    public float PlayerPickingUpCannonBallCoolDown = 0.9f;
    public float PlayerPickingUpBiggestCannonBallCoolDown = 0.9f;

    [Header("Enemy Spawner")]
    public float MinDelay = 7f;
    public float MaxDelay = 13f;
    public float TimeBetweenDifficultyIncrease = 5f;
    public float TimeDeltaDecreaseDelays = 0.5f;
    public int WeightBasicEnemy = 5;
    public int WeightMediumEnemy = 2;
    public int WeightHardEnemy = 0;


    [Header("Enemies")]
    public float SkyFlyMoveSpeed = 5f;
    public float CloudBeetleMoveSpeed = 4f;
    public float CloudBeetleSwitchLanesFrequencyMin = 0.5f;
    public float CloudBeetleSwitchLanesFrequencyMax = 3f;
    public float CloudBeetleSwitchLanesSpeed = 2f;
    public float WindWaspMoveSpeed = 8f;
}
