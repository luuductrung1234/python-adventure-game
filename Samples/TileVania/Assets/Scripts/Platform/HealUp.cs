/***************************************************************
*Title: n/a
*Author: Pham Hoang Long
*Date: n/a
*Availability: n/a
*Code version: V1
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUp : MonoBehaviour
{
    public GameObject Target;
    private Health hp;
    private void Awake() {
        hp = GameObject.FindObjectOfType<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            hp.healUp();
            Target.SetActive(false);
        }
    }
}
