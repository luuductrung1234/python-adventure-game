using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{   
    private SpriteRenderer sr;
    private GameController gameController;
    private Room1Quizzes myQuiz;
    public Dialogue myDialogue;
    private Heath hp;


    [SerializeField]
    private int choiceIndex;
    private bool isCorrectChoice;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindObjectOfType<GameController>();
        myQuiz = GameObject.FindObjectOfType<Room1Quizzes>();
        hp = GameObject.FindObjectOfType<Heath>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnswer();
        if (myQuiz.currentQuiz == 2){
            myQuiz.Proceed();
            myQuiz.currentQuiz=99;
        }

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
            if (Input.GetKeyDown(KeyCode.E) && myQuiz.currentQuiz < 3){
                myDialogue.SkipText();
                if (choiceIndex == myQuiz.correctAns[myQuiz.currentQuiz]){
                    myQuiz.currentQuiz++;
                    myDialogue.NextLine();
                }else{
                    hp.DeductHealth();
                }
            }
        }
    }
}
