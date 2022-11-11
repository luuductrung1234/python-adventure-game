using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseGame : MonoBehaviour
{
    private GameController gameController;
    public TextMeshPro pauseMessage;
    
    private void LateUpdate() {
        if (gameController.isPaused){
            pauseMessage.text = "Resume";
        }else{
            pauseMessage.text = "Pause";
        }
    }

    private void Awake() {
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    private void OnMouseDown() {
        if (gameController.isPaused){
            gameController.ResumeGame();
        }else{
            gameController.PauseGame();
        }
    }


}
