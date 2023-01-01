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

public class OnCollisionMovePlatform : MonoBehaviour
{
    public GameObject Target;
    private MovingPlatform platform;

    private void Awake() {
        platform = Target.GetComponent<MovingPlatform>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
            platform.isActive = true;
        }
    }
}
