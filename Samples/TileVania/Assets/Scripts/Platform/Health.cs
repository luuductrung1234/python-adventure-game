using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Transform playerPos;


    private void Awake() {
        Time.timeScale = 1;
        currentHealth = maxHealth;
        playerPos = GameObject.Find("Player").transform;
    }

    private void Update() {
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }

        for(int i = 0; i < hearts.Length; i++){
            if (i<maxHealth){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }

            if (i < currentHealth){
                hearts[i].sprite = fullHeart;
            }else{
                hearts[i].sprite = emptyHeart;
            }
        }

        if (currentHealth <= 0){
            gameOver();
        }
    }

    public void DeductHealth(){
        currentHealth -= 1;
        if (currentHealth <= 0){
            currentHealth = 0;
        }
        else{
        }
        StartCoroutine(Shake());

    }

    public void HealUp(){
        currentHealth = maxHealth;
    }

    public void HealUp1(){
        currentHealth += 1;
    }

    IEnumerator Shake() {   
        for ( int i = 0; i < 5; i++)
           {
                playerPos.localPosition += new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
                playerPos.localPosition -= new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
                playerPos.localPosition -= new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
                playerPos.localPosition += new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
           }
     }

    private void gameOver(){
        Time.timeScale = 1;
    }
}
