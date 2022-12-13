using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemy : MonoBehaviour
{
    GameObject player;
    SpriteRenderer sr;

    [SerializeField]
    float xPos;
    private void Awake() {
        player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (player.transform.position.x >= xPos){
            sr.flipX = true;
        }else{
            sr.flipX = false;
        }
    }
}
