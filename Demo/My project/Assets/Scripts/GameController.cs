using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{   
    public TextMeshPro healthPointText;
    public bool isGameOver;
    public bool isImmortal;
    public bool isPaused;


    [SerializeField]
    private int maxHealth = 3;
    private int currentHealth;

    private void Awake() {
        isGameOver = false;
        isImmortal = false;
        isPaused = false;
        currentHealth = maxHealth;
        Time.timeScale = 1;
    }

    private void LateUpdate() {
        healthPointText.text = "Health: " + currentHealth.ToString();
        if (currentHealth == 0){
            isGameOver = true;
        }
    }

    public void DeductHealth(int x){
        if (!isImmortal){
            if (currentHealth - x <= 0){
                currentHealth = 0;
            }else{
                currentHealth -= x;
            }
        }
    }

    public void HealUp(){
        currentHealth = maxHealth;
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
