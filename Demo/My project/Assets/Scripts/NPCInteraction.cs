using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Dialogue myDialogue;
    private GameController gameController;

    private void Awake() {
        gameController = GameObject.FindObjectOfType<GameController>();
    }
    

    void Update(){
        Talk();
    }

    private bool isCollide = false;
    private void OnTriggerEnter2D(Collider2D other) {
        isCollide = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        isCollide = false;
    }

    void Talk(){
        if (isCollide){
            if (Input.GetKeyDown(KeyCode.E)){
                if (myDialogue.textLoading){
                    myDialogue.SkipText();
                }else{
                    myDialogue.NextLine();
                }
            }
        }
    }
}
