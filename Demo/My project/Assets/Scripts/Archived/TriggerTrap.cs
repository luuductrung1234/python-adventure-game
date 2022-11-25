using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrap : MonoBehaviour
{
    private Heath hp;

    private void Awake() {
        hp = GameObject.FindObjectOfType<Heath>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        hp.DeductHealth();
    }
}
