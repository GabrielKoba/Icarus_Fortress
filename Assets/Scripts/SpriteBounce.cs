using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBounce : MonoBehaviour {

    [SerializeField]float frequency = 1.0f;  // Speed of sine movement
    [SerializeField]float magnitude = 0.01f;   // Size of sine movement
    private Vector3 axis;
    private Vector3 pos;

    void Start() {
        pos = transform.position;
        axis = transform.up;
    }

    void Update() {
        transform.position = pos + axis * Mathf.Sin (Time.deltaTime * frequency) * magnitude;        
    }
}
