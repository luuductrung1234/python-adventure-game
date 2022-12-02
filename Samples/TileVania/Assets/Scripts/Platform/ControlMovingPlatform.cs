using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMovingPlatform : MonoBehaviour
{
    public GameObject Target;
    private MovingPlatform platform;

    private void Awake() {
        platform = Target.GetComponent<MovingPlatform>();
    }
    private void Update() {
        switchButton();
    }

    private bool isCollide = false;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            isCollide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")){
            isCollide = false;
        }
    }

    private void switchButton(){
        if(isCollide){
            if (Input.GetKeyDown(KeyCode.E)){
                if(platform.isActive){
                    platform.isActive = false;
                }else{
                    platform.isActive = true;
                }
            }
        }
    }
}
