using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public float PlayerMoveSpeed = 100f;
    public float PlayerJumpForce = 600f;
    public float PlayerGravity = 3f;
    public float CannonBallSpeed = 5f;
    public float CannonBallDestroyDelay = 5f;

    [Header("Enemy Spawner")]
    public float MinDelay = 2f;
    public float MaxDelay = 5f;

    [Header("Enemies")]
    public float SkyFlyMoveSpeed = 5f;
    public float CloudBeetleMoveSpeed = 3f;
    public float WindWaspMoveSpeed = 8f;


}
