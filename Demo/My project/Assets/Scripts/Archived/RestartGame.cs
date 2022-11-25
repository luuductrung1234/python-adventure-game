using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    private GameController gameController;

    private void Awake() {
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    private void OnMouseDown() {
        gameController.GameRestart();
        gameController.isImmortal = false;
    }
    
}
