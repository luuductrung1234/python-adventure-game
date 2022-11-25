using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction2 : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;

    private void Awake() {
        Canvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Canvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        Canvas.SetActive(false);
    }
}
