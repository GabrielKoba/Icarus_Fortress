using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour {

    [Header("Settings")]
    [SerializeField]float destroyTimer = 2;

    void Start() {
        Destroy(gameObject, destroyTimer);
    }
}