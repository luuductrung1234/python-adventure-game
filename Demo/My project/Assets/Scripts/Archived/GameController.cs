using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{   
    public GameObject inputField;
    public bool isGameOver;
    public bool isImmortal;
    public bool isPaused;
    public bool walkEnable;
    public bool canInteract;
    public bool isFreezeCam;

    private void Awake() {
        isGameOver = false;
        isImmortal = false;
        isPaused = false;
        walkEnable = true;
        canInteract = true;
        isFreezeCam = false;
    }


    public void GameRestart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame(){
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        isPaused = false;
    }
}
