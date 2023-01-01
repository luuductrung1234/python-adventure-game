/***************************************************************
*Title: n/a
*Author: Pham Hoang Long
*Date: n/a
*Availability: n/a
*Code version: V1
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void PlayButton(){
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitButton(){
        Application.Quit();
    }

    public void ReviveButton(){
        SceneManager.LoadScene(ToScene.SampleQuizScene.ToString());
    }
}
