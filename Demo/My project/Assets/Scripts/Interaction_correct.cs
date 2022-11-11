using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_correct : MonoBehaviour
{

    private SpriteRenderer sr;
    private GameController gameController;
    
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindObjectOfType<GameController>();
;    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnswer();
    }
    
    bool isCollide = false;
    private void OnTriggerEnter2D(Collider2D other) {
        sr.material.color = Color.red;
        isCollide = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        sr.material.color = Color.gray;
        isCollide = false;
    }

    private void CheckAnswer(){
        if (isCollide){
            if (Input.GetKeyDown(KeyCode.E)){
                GameObject.Find("Door").SetActive(false);
                this.gameObject.SetActive(false);
                
                gameController.UpdateScore();
                gameController.HealUp();
                gameController.isImmortal = true;
            }
        }
    }
}
