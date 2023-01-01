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

public class WinningScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            SceneManager.LoadScene("WinningScene");
        }
    }
}
