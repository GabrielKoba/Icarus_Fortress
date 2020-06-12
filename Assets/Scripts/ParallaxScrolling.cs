using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] float scrollingSpeed = 1f;
    [SerializeField] Vector3 loopStartPosition;
    [SerializeField] Vector3 loopEndPosition;

    void Update() {
        Scroll();
    }

    void Scroll() {
        transform.position = Vector3.MoveTowards(transform.position, loopEndPosition, scrollingSpeed * Time.deltaTime);

        if (transform.position.x <= loopEndPosition.x) {
            transform.position = new Vector3 (loopStartPosition.x, loopStartPosition.y, transform.position.z);
        }
    }
}