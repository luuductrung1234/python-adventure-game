using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldScript : MonoBehaviour
{   

    [SerializeField]
    private Dialogue myDialogue;
    [SerializeField]
    private TMP_InputField myInput;
    private GameController gameController;
    private Room1Quizzes quiz;
    private Heath hp;
    private void Awake() {
        gameController = GameObject.FindObjectOfType<GameController>();
        quiz = GameObject.FindObjectOfType<Room1Quizzes>();
        hp = GameObject.FindObjectOfType<Heath>();
    }

    public void OnSelectField(){
        gameController.walkEnable = false;
        myInput.text = string.Empty;
        Debug.Log("stop");
    }

    public void OnDeselectField(){
        gameController.walkEnable = true;
        myInput.text = string.Empty;
        Debug.Log("go");
    }

    public void OnSubmitField(string s){
        if (quiz.currentQuiz2 < quiz.correctAns2.Length){
            if (s == quiz.correctAns2[quiz.currentQuiz2]){
                quiz.currentQuiz2++;
                myDialogue.NextLine();
            }else{
                hp.DeductHealth();
            }
        }
        gameController.walkEnable = true;
    }
}
