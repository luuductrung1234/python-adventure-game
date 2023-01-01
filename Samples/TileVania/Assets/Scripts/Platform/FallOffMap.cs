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

public class FallOffMap : MonoBehaviour
{
    public Vector3 setSpawn;
    private GameObject player;
    private Health hp;
    private void Awake() {
        player = GameObject.Find("Player");
        hp = GameObject.FindObjectOfType<Health>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Player"){
            player.transform.position = setSpawn;
            hp.DeductHealth();
        }
    }
}
