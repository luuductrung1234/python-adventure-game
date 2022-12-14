using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Heath : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private GameController gameController;
    private Transform playerPos;
    [SerializeField]
    private GameObject camPos;
    private Vector3 tempPos;


    private void Awake() {
        currentHealth = maxHealth;
        playerPos = GameObject.Find("Player").transform;
        gameController = GameObject.FindObjectOfType<GameController>();
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
    }

    public void DeductHealth(){
        currentHealth -= 1;
        if (currentHealth <= 0){
            currentHealth = 0;
            gameController.walkEnable = false;
            gameController.canInteract = false;
        }
        else{
            gameController.walkEnable = true;
            gameController.canInteract = true;
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
}
