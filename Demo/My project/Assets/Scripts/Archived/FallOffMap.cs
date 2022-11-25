using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffMap : MonoBehaviour
{
    public Vector3 setSpawn;
    public GameObject player;
    private Heath hp;
    private void Awake() {
        hp = GameObject.FindObjectOfType<Heath>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Player"){
            player.transform.position = setSpawn;
            hp.DeductHealth();
        }
    }
}
