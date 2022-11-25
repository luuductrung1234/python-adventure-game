using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1Quizzes : MonoBehaviour
{   

    public int currentQuiz = 0;
    public string[,] quiz = {{"int", "float", "str", "bool"},{"False", "32", "5", "True"}};

    public int[] correctAns = {1, 4};

    /// quiz for room 2 as well
    public int currentQuiz2 = 0;
    public string[] correctAns2 = {"int(x)"};

    public void Proceed(){
        GameObject.Find("Border_r1").SetActive(false);
    }
}
