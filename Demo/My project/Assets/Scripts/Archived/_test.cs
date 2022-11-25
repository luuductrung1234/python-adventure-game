using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _test : MonoBehaviour
{
    private Heath heath;
    
    private void Awake() {
        heath = GameObject.FindObjectOfType<Heath>();
    }

    private void OnMouseDown() {
        heath.HealUp1();
    }
}
