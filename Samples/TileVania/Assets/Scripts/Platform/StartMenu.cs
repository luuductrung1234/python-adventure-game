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
}
