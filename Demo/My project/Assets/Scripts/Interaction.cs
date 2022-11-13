using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{   
    private SpriteRenderer sr;
    private GameController hp;
    private GameController gameController;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindObjectOfType<GameController>();
    }

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
                StartCoroutine(Shake());
            }
        }
    }
     IEnumerator Shake() {
           for ( int i = 0; i < 5; i++)
           {
               transform.localPosition += new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
               transform.localPosition -= new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
                transform.localPosition -= new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
                transform.localPosition += new Vector3(0.1f, 0, 0);
               yield return new WaitForSeconds(0.01f);
           }
     }


}
