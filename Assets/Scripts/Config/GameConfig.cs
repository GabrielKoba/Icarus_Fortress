using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public float PlayerMoveSpeed = 100f;
    public float PlayerJumpForce = 600f;
    public float PlayerGravity = 3f;
}
