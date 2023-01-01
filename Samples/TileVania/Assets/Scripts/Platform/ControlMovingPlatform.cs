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
using UnityEngine.UI;

public class ControlMovingPlatform : MonoBehaviour
{   
    private SpriteRenderer sr;
    public GameObject Target;
    private MovingPlatform platform;
    public Sprite Disable;
    public Sprite Active;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
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
                    sr.sprite = Disable;
                }else{
                    platform.isActive = true;
                    sr.sprite = Active;
                }
            }
        }
    }
}
